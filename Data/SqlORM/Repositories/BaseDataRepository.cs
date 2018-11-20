using DriveCentric.Model;
using DriveCentric.Model.Interfaces;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using ServiceStack.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DriveCentric.Data.SqlORM.Repositories
{
    public abstract class BaseDataRepository<T> where T : IBaseModel, new()
    {
        protected readonly IDataAccessor dataAccessor;
        public IContextInfoAccessor ContextInfoAccessor { get; }

        protected BaseDataRepository(IContextInfoAccessor contextInfoAccessor, IDataAccessor dataAccessor)  
        {
            this.dataAccessor = dataAccessor;
            this.ContextInfoAccessor = contextInfoAccessor;
        }

        [MonitorAsyncAspect]
        public virtual async Task<bool> DeleteByIdAsync(int id)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                return await dataAccessor.DeleteByIdAsync<T>(id, db) > 0;
            }
        }

        [MonitorAsyncAspect]
        public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, string[] referenceFields = null)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                return await dataAccessor.GetSingleAsync(db, predicate, referenceFields);
            }
        }

        [MonitorAsyncAspect]
        public virtual async Task<(long count, IEnumerable<T> data)> GetAllAsync(Expression<Func<T, bool>> predicate, IPageable paging, string[] fields = null)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                return await dataAccessor.GetAllAsync(db, predicate, paging);
            }
        }

        [MonitorAsyncAspect]
        public virtual async Task<long> InsertAsync(T item)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                return await dataAccessor.InsertAsync(db, item);
            }
        }

        [MonitorAsyncAspect]
        public virtual async Task<bool> UpdateAsync(T item)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                return await dataAccessor.UpdateAsync(db, item);
            }
        }

        public abstract IDbConnectionFactory GetDbFactory();
    }
}
