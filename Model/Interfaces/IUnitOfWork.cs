using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DriveCentric.Core.Interfaces
{
    public interface IUnitOfWork
    {
        Task<Dictionary<string, bool>> GetDatabaseHealthCheck();

        Task<long> SaveChanges();

        void Delete<T>(int id) where T : IBaseModel, new();

        void Update<T>(T entity) where T : IBaseModel, new();

        void Insert<T>(T entity) where T : IBaseModel, new();

        Task<long> GetCount<T>(Expression<Func<T, bool>> expression) where T : IBaseModel, new();

        Task<IEnumerable<T>> GetEntities<T>(Expression<Func<T, bool>> expression, IPageable paging, string[] referenceFields = null) where T : class, IBaseModel, new();

        Task<T> GetEntity<T>(Expression<Func<T, bool>> expression, string[] referenceFields = null) where T : IBaseModel, new();
    }
}