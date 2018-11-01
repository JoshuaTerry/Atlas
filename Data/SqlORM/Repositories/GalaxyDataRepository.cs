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
    public class GalaxyDataRepository<T, U> : BaseDataRepository, IDataRepository<T>
        where T : IBaseModel where U : T
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
        public Task<long> InsertAsync(T item)
        {
            using (IDbConnection db = dbFactory.OpenDbConnection())
            {
                return dataAccessor.InsertAsync(db, item);
            }
        }

        [MonitorAsyncAspect]
        public Task<bool> UpdateAsync(T item)
        {
            using (IDbConnection db = dbFactory.OpenDbConnection())
            {
                return dataAccessor.UpdateAsync(db, item);
            }
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
