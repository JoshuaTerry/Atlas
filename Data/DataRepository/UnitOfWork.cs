using DriveCentric.Core.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Utilities.Context;
using Microsoft.Extensions.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DriveCentric.Data.DataRepository
{
    public class UnitOfWork : IUnitOfWork, IContextAccessible
    {
        private readonly IConfiguration configuration;
        private Dictionary<int, DriveServer> servers;
        private readonly Dictionary<string, IDbConnectionFactory> connectionFactories;
        private readonly Queue<(string Database, Func<IDbConnection, Task<long>> Action)> saveActions = new Queue<(string Database, Func<IDbConnection, Task<long>> Action)>();
        private readonly Dictionary<IDbConnectionFactory, Queue<Func<IDbConnection, Task<long>>>> saveActionsByFactory = new Dictionary<IDbConnectionFactory, Queue<Func<IDbConnection, Task<long>>>>();
        private readonly IRepository repository;

        public UnitOfWork(IContextInfoAccessor contextInfoAccessor,
                            IConfiguration configuration,
                            IRepository repository)
        {
            this.ContextInfoAccessor = contextInfoAccessor;
            this.configuration = configuration;
            this.repository = repository;
            connectionFactories = new Dictionary<string, IDbConnectionFactory>();
            CreateConnectionFactories();

            foreach (var factory in connectionFactories)
            {
                saveActionsByFactory.Add(factory.Value, new Queue<Func<IDbConnection, Task<long>>>());
            }
        }

        private string GetConnectionStringById(int id)
        {
            var server = servers.FirstOrDefault(item => item.Key == id).Value;

            if (server != null)
            {
                return server.ConnectionString;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        private void CreateConnectionFactories()
        {
            AddGalaxyFactory(connectionFactories);
            this.servers = GetEntities<DriveServer>(null, PageableSearch.Default).Result.ToDictionary(server => server.Id);
            AddStarFactory(connectionFactories);
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
                        GetConnectionStringById(driveServerId),
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
            => saveActionsByFactory[GetFactoryByEntityType(typeof(T))].Enqueue(new Func<IDbConnection, Task<long>>(async (connection) => await repository.InsertAsync(connection, entity)));

        public void Update<T>(T entity) where T : IBaseModel, new()
            => saveActionsByFactory[GetFactoryByEntityType(typeof(T))].Enqueue(new Func<IDbConnection, Task<long>>(async (connection) => await repository.UpdateAsync(connection, entity)));

        public void Delete<T>(int id) where T : IBaseModel, new()
            => saveActionsByFactory[GetFactoryByEntityType(typeof(T))].Enqueue(new Func<IDbConnection, Task<long>>(async (connection) => await repository.DeleteByIdAsync<T>(connection, id)));

        private async Task<long> ProcessTransaction(IDbConnectionFactory factory, Queue<Func<IDbConnection, Task<long>>> saveActions)
        {
            long result = 0;

            using (var conn = factory.OpenDbConnection())
            {
                using (var tran = conn.OpenTransaction())
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
            var actionsByFactory = saveActionsByFactory.Where(x => x.Value.Count >= 1).First();

            return await ProcessTransaction(actionsByFactory.Key, actionsByFactory.Value);
        }

        public IContextInfoAccessor ContextInfoAccessor { get; }
    }
}