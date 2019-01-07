using DriveCentric.Core.Interfaces;
using DriveCentric.Data.DataRepository.Interfaces;
using DriveCentric.Data.DataRepository.Repositories;
using DriveCentric.Utilities.Context;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DriveCentric.Data.DataRepository
{
    public class UnitOfWork : IUnitOfWork, IContextAccessible
    {
        private Dictionary<string, IRepository> repositories;
        private readonly Queue<Func<Task<long>>> actions;

        public IContextInfoAccessor ContextInfoAccessor { get; }
        private IDriveServerCollection DriveServerCollection { get; }

        public UnitOfWork(IContextInfoAccessor contextInfoAccessor, IConfiguration configuration, IDriveServerCollection driveServerCollection)
        {
            ContextInfoAccessor = contextInfoAccessor;
            DriveServerCollection = driveServerCollection;
            LoadRepositories();
            actions = new Queue<Func<Task<long>>>();
        }

        public async Task<Dictionary<string, bool>> GetDatabaseHealthCheck()
        {
            var connections = new Dictionary<string, bool>();

            foreach (var kvp in repositories)
            {
                connections.Add(kvp.Key, await kvp.Value.IsDatabaseAvailable());
            }

            return connections;
        }

        private void LoadRepositories()
        {
            var driveServerId = Convert.ToInt32(ContextInfoAccessor.ContextInfo.User.Claims.Single(c => c.Type == "custom:DriveServerId").Value);

            repositories = new Dictionary<string, IRepository>
            {
                { "Galaxy", new SqlRepository(DriveServerCollection.GalaxyConnectionString) },
                { "Star", new SqlRepository(DriveServerCollection.GetConnectionStringById(driveServerId)) }
            };
        }

        private IRepository GetRepoByEntityType(Type type)
        {
            if (typeof(IGalaxyEntity).IsAssignableFrom(type))
                return repositories["Galaxy"];
            else if (typeof(IStarEntity).IsAssignableFrom(type))
                return repositories["Star"];
            else
                throw new Exception("Type requested does not have an assigned Repository.");
        }

        public Task<long> GetCount<T>(Expression<Func<T, bool>> expression) where T : IBaseModel, new()
            => GetRepoByEntityType(typeof(T)).GetCountAsync<T>(expression);

        public async Task<T> GetEntity<T>(Expression<Func<T, bool>> expression, string[] referenceFields = null) where T : IBaseModel, new()
            => await GetRepoByEntityType(typeof(T)).GetSingleAsync<T>(expression, referenceFields);

        public async Task<IEnumerable<T>> GetEntities<T>(Expression<Func<T, bool>> expression, IPageable paging, string[] referenceFields) where T : class, IBaseModel, new()
            => await GetRepoByEntityType(typeof(T)).GetAllAsync<T>(expression, paging, referenceFields);

        public void Insert<T>(T entity) where T : IBaseModel, new()
            => actions.Enqueue(new Func<Task<long>>(async () => await GetRepoByEntityType(typeof(T)).InsertAsync(entity)));

        public void Update<T>(T entity) where T : IBaseModel, new()
        => actions.Enqueue(new Func<Task<long>>(async () => await GetRepoByEntityType(typeof(T)).UpdateAsync(entity)));

        public void Delete<T>(int id) where T : IBaseModel, new()
            => actions.Enqueue(new Func<Task<long>>(async () => await GetRepoByEntityType(typeof(T)).DeleteByIdAsync<T>(id)));

        public async Task<long> SaveChanges()
        {
            long result = 0;

            foreach (var action in actions)
                result += await action();

            return result;
        }
    }
}