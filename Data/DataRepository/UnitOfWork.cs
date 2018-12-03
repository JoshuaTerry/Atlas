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
using DriveCentric.Data.DataRepository.Repositories;
using System.Linq.Expressions;

namespace DriveCentric.Data.DataRepository
{
    public class UnitOfWork : IUnitOfWork, IContextAccessible
    {
        private readonly IDriveServerCollection driveServerCollection;
        private readonly IConfiguration configuration;
        private readonly Dictionary<string, IDbConnectionFactory> connectionFactories; 
        private readonly Queue<(string Database, Func<IDbConnection, Task<long>> Action)> saveActions = new Queue<(string Database, Func<IDbConnection, Task<long>> Action)>();
        private Dictionary<IDbConnectionFactory, Queue<Func<IDbConnection, Task<long>>>> saveActionsByFactory = new Dictionary<IDbConnectionFactory, Queue<Func<IDbConnection, Task<long>>>>();
        private IRepository repository;

        public UnitOfWork(IContextInfoAccessor contextInfoAccessor,
                            IConfiguration configuration,
                            IRepository repository,
                            IDriveServerCollection driveServerCollection)
        {
            this.ContextInfoAccessor = contextInfoAccessor;
            this.configuration = configuration;
            this.driveServerCollection = driveServerCollection;
            this.repository = repository;
            connectionFactories = CreateConnectionFactories();
           
            foreach (var factory in connectionFactories)
            {
                saveActionsByFactory.Add(factory.Value, new Queue<Func<IDbConnection, Task<long>>>());
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
          
        public async Task<T> GetEntity<T>(Expression<Func<T, bool>> expression, string[] referenceFields = null) where T : IBaseModel, new()
        {
            using (var connection = GetFactoryByEntityType(typeof(T)).OpenDbConnection())
                return await repository.GetSingleAsync<T>(connection, expression, referenceFields);
        }
        public async Task<long> GetCount<T>(Expression<Func<T, bool>> expression) where T : IBaseModel, new()
        {
            using (var connection = GetFactoryByEntityType(typeof(T)).OpenDbConnection())
                return await repository.GetCount<T>(connection, expression);
        }
        public async Task<IEnumerable<T>> GetEntities<T>(Expression<Func<T, bool>> expression, IPageable paging, string[] referenceFields = null) where T : class, IBaseModel, new() 
        {
            using (var connection = GetFactoryByEntityType(typeof(T)).OpenDbConnection())
                return await repository.GetAllAsync<T>(connection, expression, paging, referenceFields);
        }

        private IDbConnectionFactory GetFactoryByEntityType(Type type)
        {
            if (typeof(IGalaxyEntity).IsAssignableFrom(type))
                return connectionFactories["Galaxy"];
            else if (typeof(IStarEntity).IsAssignableFrom(type))
                return connectionFactories["Star"];  
            else
                throw new Exception("Type requested does not have an assiged connection factory."); 
        }
         
        public void Insert<T>(T entity) where T : IBaseModel, new()
            => saveActionsByFactory[GetFactoryByEntityType(typeof(T))].Enqueue(new Func<IDbConnection, Task<long>>(async (connection) =>
                {
                    return await repository.InsertAsync(connection, entity);
                }));  

        public void Update<T>(T entity) where T : IBaseModel, new()
            => saveActionsByFactory[GetFactoryByEntityType(typeof(T))].Enqueue(new Func<IDbConnection, Task<long>>(async (connection) =>
            {
                return await repository.UpdateAsync(connection, entity);
            }));

        public void Delete<T>(int id) where T : IBaseModel, new()
            => saveActionsByFactory[GetFactoryByEntityType(typeof(T))].Enqueue(new Func<IDbConnection, Task<long>>(async (connection) =>
            {
                return await repository.DeleteByIdAsync<T>(connection, id);
            }));         

        private async Task<long> ProcessTransaction(IDbConnectionFactory factory, Queue<Func<IDbConnection, Task<long>>> saveActions)
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
                            result += await saveAction(conn);
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

        public async Task<long> SaveChanges()
        {
            if (saveActions.Select(x => x.Database).Distinct().Count() > 1)
                throw new Exception("Transactions cannot span multiple databases");

            var database = saveActions.Select(x => x.Database).FirstOrDefault();
            
            if (string.IsNullOrEmpty(database)  || !connectionFactories.ContainsKey(database))
                throw new Exception("No Database is specified for the requested actions.");

            var queue = saveActionsByFactory[connectionFactories[database]];

            return await ProcessTransaction(connectionFactories[database], queue); 
        }
         
        public IContextInfoAccessor ContextInfoAccessor { get; }
    } 
}
