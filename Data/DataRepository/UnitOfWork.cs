using DriveCentric.Data.SqlORM.Repositories;
using DriveCentric.Model;
using DriveCentric.Model.Interfaces;
using DriveCentric.Utilities.Configuration;
using DriveCentric.Utilities.Context;
using Microsoft.Extensions.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveCentric.Data.DataRepository
{
    public class UnitOfWork : IContextAccessible
    {
        private readonly IDriveServerCollection driveServerCollection;
        private readonly IContextInfoAccessor contextInfoAccessor;
        private readonly IConfiguration configuration;
        private readonly Dictionary<string, IDbConnectionFactory> connectionFactories;
        private readonly Dictionary<string, IRepository> repositories;
        private readonly Dictionary<IDbConnectionFactory, IRepository> factoryRepositories;
        private readonly Queue<(string Database, Func<IRepository, IDbConnection, Task<long>> Action)> saveActions = new Queue<(string Database, Func<IRepository, IDbConnection, Task<long>> Action)>();
        private Dictionary<IDbConnectionFactory, Queue<Func<IRepository, IDbConnection, Task<long>>>> saveActionsByFactory = new Dictionary<IDbConnectionFactory, Queue<Func<IRepository, IDbConnection, Task<long>>>>();


        public UnitOfWork(IContextInfoAccessor contextInfoAccessor,
                            IConfiguration configuration,
                            IDriveServerCollection driveServerCollection)
        {
            this.contextInfoAccessor = contextInfoAccessor;
            this.configuration = configuration;
            this.driveServerCollection = driveServerCollection;
            connectionFactories = CreateConnectionFactories();
            factoryRepositories = new Dictionary<IDbConnectionFactory, IRepository>
            {
                { connectionFactories["Galaxy"], new Repository(contextInfoAccessor, connectionFactories["Galaxy"]) },
                { connectionFactories["Star"], new Repository(contextInfoAccessor, connectionFactories["Star"]) }
            };

            repositories = new Dictionary<string, IRepository>
            {
                { "Galaxy", new Repository(contextInfoAccessor, connectionFactories["Galaxy"]) },
                { "Star", new Repository(contextInfoAccessor, connectionFactories["Star"]) }
            };

            foreach (var factory in connectionFactories)
            {
                saveActionsByFactory.Add(factory.Value, new Queue<Func<IRepository, IDbConnection, Task<long>>>());
            } 
        }

        private Dictionary<string, IDbConnectionFactory> CreateConnectionFactories()
        {
            var connectionFactories = new Dictionary<string, IDbConnectionFactory>();
            AddGalaxyFactory(connectionFactories);
            AddStarFactory(connectionFactories);

            return connectionFactories;
        }

        private void AddGalaxyFactory(Dictionary<string, IDbConnectionFactory> connectionFactories)
            => connectionFactories.Add("Galaxy", new OrmLiteConnectionFactory(configuration.GetSection("SqlDBInfo:ConnectionString").Value, SqlServerDialect.Provider));

        private void AddStarFactory(Dictionary<string, IDbConnectionFactory> connectionFactories)
        {
            try
            {
                if (connectionFactories.ContainsKey("Star"))
                    connectionFactories.Remove("Star");

                var driveServerId = 21;
                //var driveServerId = Convert.ToInt32(ContextInfoAccessor.ContextInfo.User.Claims.Single(c => c.Type == "custom:DriveServerId"));
                connectionFactories.Add("Star", new OrmLiteConnectionFactory(
                        driveServerCollection.GetConnectionStringById(driveServerId),
                        SqlServerDialect.Provider
                        ));
            }
            catch (Exception ex)
            {
                throw new Exception("No DriveServerId was found in the token.", ex);
            }
        }

        public IRepository GetRepository<T>() where T : IBaseModel
            => repositories[GetRepositoryType(typeof(T))];

        private IDbConnectionFactory GetFactoryByEntityType(Type type)
        {
            if (typeof(IGalaxyEntity).IsAssignableFrom(type))
                return connectionFactories["Galaxy"];
            else if (typeof(IStarEntity).IsAssignableFrom(type))
                return connectionFactories["Star"];  
            else
                throw new Exception("Type requested does not have an assiged connection factory."); 
        }

        private string GetRepositoryType(Type type)
        {
            if (typeof(IGalaxyEntity).IsAssignableFrom(type))
                return "Galaxy";
            else if (typeof(IStarEntity).IsAssignableFrom(type))
                return "Star";
            else
                throw new Exception("Type requested is not assigned to a Repository.");
        }

        public void Insert<T>(T entity) where T : IBaseModel, new()
            => saveActionsByFactory[GetFactoryByEntityType(typeof(T))].Enqueue(new Func<IRepository, IDbConnection, Task<long>>(async (repository, connection) =>
                {
                    return await repository.InsertAsync(connection, entity);
                }));  

        public void Update<T>(T entity) where T : IBaseModel, new()
            => saveActionsByFactory[GetFactoryByEntityType(typeof(T))].Enqueue(new Func<IRepository, IDbConnection, Task<long>>(async (repository, connection) =>
            {
                return await repository.UpdateAsync(connection, entity);
            }));

        public void Delete<T>(int id) where T : IBaseModel, new()
            => saveActionsByFactory[GetFactoryByEntityType(typeof(T))].Enqueue(new Func<IRepository, IDbConnection, Task<long>>(async (repository, connection) =>
            {
                return await repository.DeleteByIdAsync<T>(connection, id);
            }));
         

        private async Task<long> ProcessTransaction(IDbConnectionFactory factory, Queue<Func<IRepository, IDbConnection, Task<long>>> saveActions)
        {
            long result = 0;

            using (var conn = factory.OpenDbConnection())
            {
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var saveAction in saveActions)
                        {
                            result += await saveAction(factoryRepositories[factory], conn);
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new Exception("Transaction Failed", ex);
                    }
                }
            }

            return result;
        }

        public async Task<long> SaveChangesWithTransactionForSingleDatabase()
        {
            if (saveActions.Select(x => x.Database).Distinct().Count() > 1)
                throw new Exception("Transactions cannot span multiple databases");

            var database = saveActions.Select(x => x.Database).FirstOrDefault();
            
            if (string.IsNullOrEmpty(database)  || !connectionFactories.ContainsKey(database))
                throw new Exception("No Database is specified for the requested actions.");

            var queue = saveActionsByFactory[connectionFactories[database]];

            return await ProcessTransaction(connectionFactories[database], queue); 
        }

        // Save changes and attempt to handle transactions for multiple databases.
        public async Task<long> SaveChanges_WithTransactionsFromMultipleSources()
        {
            var transactions = new Dictionary<string, IDbTransaction>();
            long results = 0;

            foreach (var action in saveActions)
            {
                try
                {
                    // If we haven't created a transaction for this database, create one now.
                    if (!transactions.ContainsKey(action.Database))
                    {
                        // Get the connection, open it, and start a transaction.
                        var connection = connectionFactories[action.Database].OpenDbConnection();
                        transactions.Add(action.Database, connection.BeginTransaction());
                    }

                    // Execute the action passing in the repository for the database and the open connection.
                    results += await action.Action(repositories[action.Database], transactions[action.Database].Connection);
                }
                catch (Exception ex)
                {
                    // Rollback and close connections.
                    transactions.ToList().ForEach(t => { t.Value.Rollback(); t.Value.Connection.Close(); });
                    throw ex;
                }
            }

            // Commit and close connections.
            transactions.ToList().ForEach(t => { t.Value.Commit(); t.Value.Connection.Close(); });

            return results;
        }

        public IContextInfoAccessor ContextInfoAccessor { get { return contextInfoAccessor; } }
    }
}
