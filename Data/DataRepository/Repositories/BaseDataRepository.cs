using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.Core.Interfaces;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace DriveCentric.Data.SqlORM.Repositories
{
    public abstract class BaseDataRepository<T> where T : IBaseModel, new()
    {
        protected readonly IDataAccessor dataAccessor;
        public IContextInfoAccessor ContextInfoAccessor { get; }

        protected BaseDataRepository(IContextInfoAccessor contextInfoAccessor)
        {
            this.ContextInfoAccessor = contextInfoAccessor;
        }

        [MonitorAsyncAspect]
        public virtual async Task<bool> DeleteByIdAsync(int id)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                return await db.DeleteByIdAsync<T>(id) > 0;
            }
        }

        [MonitorAsyncAspect]
        public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, string[] referenceFields = null)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                var result = await db.SingleAsync<T>(predicate);
                await db.LoadReferencesAsync(result, referenceFields);
                return result;
            }
        }

        [MonitorAsyncAspect]
        public virtual async Task<(long count, IEnumerable<T> data)> GetAllAsync(Expression<Func<T, bool>> predicate, IPageable paging, string[] referenceFields = null)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                var count = db.Count<T>(predicate);
                bool isDescending = paging.OrderBy.StartsWith("-");

                if (isDescending)
                    return (count, await db.LoadSelectAsync(db.From<T>().Where(predicate).Limit(skip: paging.Offset, rows: paging.Limit).OrderByDescending(GetModelOrderByField(db, paging)), referenceFields));
                else
                    return (count, await db.LoadSelectAsync(db.From<T>().Where(predicate).Limit(skip: paging.Offset, rows: paging.Limit).OrderBy(GetModelOrderByField(db, paging)), referenceFields));
            }
        }

        [MonitorAsyncAspect]
        public virtual async Task<long> InsertAsync(T item)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                try
                {
                    return await db.InsertAsync(item, selectIdentity: true);
                }
                catch (Exception exception)
                {
                    throw new Exception($"Error trying to insert {typeof(T)}.", exception);
                }
            }
        }

        [MonitorAsyncAspect]
        public virtual async Task<bool> UpdateAsync(T item)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                try
                {
                    return await db.UpdateAsync(item) > 0;
                }
                catch (Exception exception)
                {
                    throw new Exception($"Error trying to update {typeof(T)}.", exception);
                }
            }
        }

        public abstract IDbConnectionFactory GetDbFactory();

        private string GetModelOrderByField(IDbConnection connection, IPageable paging)
            => connection.From<T>().ModelDef.AllFieldDefinitionsArray.Where(f => f.Name.TrimStart('-') == paging.OrderBy).Select(f => string.IsNullOrWhiteSpace(f.Alias) ? f.Name : f.Alias).FirstOrDefault();
    }
}
