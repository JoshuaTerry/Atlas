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
    public class GalaxyDataRepository<T> : BaseDataRepository<T>, IDataRepository<T>
        where T : IBaseModel, new()
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


        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate, IPageable paging)
        {
            throw new NotImplementedException();
        }
        [MonitorAsyncAspect]
        public async Task<bool> DeleteByIdAsync(int id)
        {
            using (IDbConnection db = dbFactory.OpenDbConnection())
            {
                return await dataAccessor.DeleteByIdAsync<T>(id, db) > 0;
            }
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, string[] referenceFields = null)
        {
            using (IDbConnection db = dbFactory.OpenDbConnection())
            {
                return await dataAccessor.GetSingleAsync(db, predicate); 
            }
        }

        [MonitorAsyncAspect]
        public async Task<(long count, IEnumerable<T> data)> GetAllAsync(Expression<Func<T, bool>> predicate, IPageable paging, string[] fields = null)
        {
            using (IDbConnection db = dbFactory.OpenDbConnection())
            {
                var foo = await dataAccessor.GetAllAsync(db, predicate, paging);
                var bar = (foo.count, (IEnumerable<T>)foo.data);
                return bar;
            }
        }
         
        [MonitorAsyncAspect]
        public async Task<long> InsertAsync(T item)
        {
            using (IDbConnection db = dbFactory.OpenDbConnection())
            {
                return await dataAccessor.InsertAsync(db, item);
            }
        }

        [MonitorAsyncAspect]
        public async Task<bool> UpdateAsync(T item)
        {
            using (IDbConnection db = dbFactory.OpenDbConnection())
            {
                return await dataAccessor.UpdateAsync(db, item);
            }
        }

        public void Save()
        {
            throw new NotImplementedException();
        } 
    }
}
