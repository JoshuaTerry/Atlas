using DriveCentric.Core.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Data.DataRepository.Repositories;
using DriveCentric.Utilities.Context;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DriveCentric.Data.DataRepository
{
    public class UnitOfWork : IUnitOfWork, IContextAccessible
    {
        private readonly IConfiguration configuration;
        private Dictionary<string, IRepository> repositories;
        private Queue<Func<Task<long>>> actions;

        public UnitOfWork(IContextInfoAccessor contextInfoAccessor, IConfiguration configuration)
        {
            this.ContextInfoAccessor = contextInfoAccessor;
            this.configuration = configuration;
            actions = new Queue<Func<Task<long>>>();

            repositories = new Dictionary<string, IRepository>();
            repositories.Add("Galaxy", new SqlRepository(configuration.GetSection("SqlDBInfo:ConnectionString").Value));
            repositories.Add("Star", new SqlRepository(GetStarConnectionString()));
        }

        //var driveServerId = Convert.ToInt32(ContextInfoAccessor.ContextInfo.User.Claims.Single(c => c.Type == "custom:DriveServerId"));
        private string GetStarConnectionString()
            => repositories["Galaxy"].GetSingle<DriveServer>(x => x.Id == 21).ConnectionString;

        public IContextInfoAccessor ContextInfoAccessor { get; }

        private IRepository GetRepoByEntityType(Type type)
        {
            if (typeof(IGalaxyEntity).IsAssignableFrom(type))
                return repositories["Galaxy"];
            else if (typeof(IStarEntity).IsAssignableFrom(type))
                return repositories["Star"];
            else
                throw new Exception("Type requested does not have an assiged Repository.");
        }

        public Task<long> GetCount<T>(Expression<Func<T, bool>> expression) where T : IBaseModel, new()
            => GetRepoByEntityType(typeof(T)).GetCount<T>(expression);

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