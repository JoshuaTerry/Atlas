using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.Core.Interfaces;
using DriveCentric.Utilities.Context;
using Microsoft.Extensions.Configuration;

namespace DriveCentric.Data.DataRepository
{
    public class UnitOfWork : IUnitOfWork, IContextAccessible
    {
        private readonly Queue<Func<Task<long>>> actions;

        private readonly Dictionary<string, IRepository> repositories;

        private readonly IDatabaseCollectionManager dbManager;

        public UnitOfWork(IContextInfoAccessor contextInfoAccessor, IConfiguration configuration, IDatabaseCollectionManager manager)
        {
            ContextInfoAccessor = contextInfoAccessor;
            dbManager = manager;
            repositories = manager.Repositories;
            actions = new Queue<Func<Task<long>>>();
        }

        public IContextInfoAccessor ContextInfoAccessor { get; }

        public async Task<Dictionary<string, bool>> GetDatabaseHealthCheck()
        {
            var connections = new Dictionary<string, bool>();

            foreach (var kvp in repositories)
            {
                connections.Add(kvp.Key, await kvp.Value.IsDatabaseAvailable());
            }

            return connections;
        }

        public Task<long> GetCount<T>(Expression<Func<T, bool>> expression)
            where T : IBaseModel, new()
            => GetRepoByEntityType(typeof(T)).GetCountAsync<T>(expression);

        public async Task<T> GetEntity<T>(Expression<Func<T, bool>> expression, string[] referenceFields = null)
            where T : IBaseModel, new()
            => await GetRepoByEntityType(typeof(T)).GetSingleAsync<T>(expression, referenceFields);

        public async Task<IEnumerable<T>> GetEntities<T>(Expression<Func<T, bool>> expression, IPageable paging, string[] referenceFields = null)
            where T : class, IBaseModel, new()
            => await GetRepoByEntityType(typeof(T)).GetAllAsync<T>(expression, paging, referenceFields);

        public void Insert<T>(T entity)
            where T : IBaseModel, new()
            => actions.Enqueue(new Func<Task<long>>(async () => await GetRepoByEntityType(typeof(T)).InsertAsync(entity)));

        public void Update<T>(T entity)
            where T : IBaseModel, new()
        => actions.Enqueue(new Func<Task<long>>(async () => await GetRepoByEntityType(typeof(T)).UpdateAsync(entity)));

        public void Delete<T>(int id)
            where T : IBaseModel, new()
            => actions.Enqueue(new Func<Task<long>>(async () => await GetRepoByEntityType(typeof(T)).DeleteByIdAsync<T>(id)));

        public async Task<long> SaveChanges()
        {
            long result = 0;

            foreach (var action in actions)
            {
                result += await action();
            }

            return result;
        }

        private IRepository GetRepoByEntityType(Type type)
        {
            if (typeof(IGalaxyEntity).IsAssignableFrom(type))
            {
                return repositories["Galaxy"];
            }
            else if (typeof(IStarEntity).IsAssignableFrom(type))
            {
                return repositories["Star"];
            }
            else
            {
                throw new ArgumentException("Type requested does not have an assigned Repository.");
            }
        }
    }
}