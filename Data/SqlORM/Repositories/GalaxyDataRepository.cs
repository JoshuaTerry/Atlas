using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.Model;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using DriveCentric.Utilities.Data;
using ServiceStack.Data;

namespace DriveCentric.Data.SqlORM.Repositories
{
    public class GalaxyDataRepository<T, U> : BaseDataRepository<T, U>, IDataRepository<T>
        where T : IBaseModel where U : T, new()
    {
        protected readonly IDbConnectionFactory dbFactory;

        public GalaxyDataRepository(
            IContextInfoAccessor contextInfoAccessor,
            IDbConnectionFactory dbFactory,
            IDataAccessor dataAccessor
            ) : base(contextInfoAccessor, dataAccessor)
        {
            this.dbFactory = dbFactory;
        }

        [MonitorAsyncAspect]
        public async Task<bool> DeleteByIdAsync(int id)
        {
            using (IDbConnection db = dbFactory.OpenDbConnection())
            {
                return await dataAccessor.DeleteByIdAsync<U>(id, db) > 0;
            }
        }

        [MonitorAsyncAspect]
        public async Task<T> GetByIdAsync(int id)
        {
            using (IDbConnection db = dbFactory.OpenDbConnection())
            {
                var item = await dataAccessor.GetByIdAsync<U>(id, db);

                if (EqualityComparer<T>.Default.Equals(item, default))
                {
                    throw new KeyNotFoundException();
                }

                return item;
            }
        }

        [MonitorAsyncAspect]
        public async Task<IEnumerable<T>> GetAsync(
            int? limit = null,
            int? offset = null,
            Expression predicate = null)
        {
            using (IDbConnection db = dbFactory.OpenDbConnection())
            {
                return (IEnumerable<T>) await dataAccessor.GetAsync(db, limit, offset, (Expression<Func<U, bool>>)predicate);
            }
        }

        [MonitorAsyncAspect]
        public async Task<(long count, IEnumerable<T> data)> GetAsync(Expression<Func<T, bool>> predicate, IPageable paging, string[] fields = null)
        {
            using (IDbConnection db = dbFactory.OpenDbConnection())
            {
                var foo = await dataAccessor.GetAsync(db, predicate, paging);
                var bar = (foo.count, (IEnumerable<T>)foo.data);
                return bar;
            }
        }

        [MonitorAspect]
        public IEnumerable<T> Get(
            int? limit = null,
            int? offset = null,
            Expression predicate = null)
        {
            using (IDbConnection db = dbFactory.OpenDbConnection())
            {
                return (IEnumerable<T>)dataAccessor.Get(db, limit, offset, (Expression<Func<U, bool>>)predicate);
            }
        }

        [MonitorAsyncAspect]
        public async Task<long> InsertAsync(T item)
        {
            using (IDbConnection db = dbFactory.OpenDbConnection())
            {
                return await dataAccessor.InsertAsync(db, ConvertToDataModel(item));
            }
        }

        [MonitorAsyncAspect]
        public async Task<bool> UpdateAsync(T item)
        {
            using (IDbConnection db = dbFactory.OpenDbConnection())
            {
                return await dataAccessor.UpdateAsync(db, ConvertToDataModel(item));
            }
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
         
    }
}
