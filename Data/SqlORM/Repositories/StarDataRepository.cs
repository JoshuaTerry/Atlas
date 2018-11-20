using System;
using System.Collections.Generic;
using System.Data;
using System.Linq; 
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using DriveCentric.Model;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Configuration;
using DriveCentric.Utilities.Context;
using DriveCentric.Utilities.Data;
using ServiceStack.OrmLite;

namespace DriveCentric.Data.SqlORM.Repositories
{
    public class StarDataRepository<T> : BaseDataRepository<T>, IDataRepository<T>
        where T : IBaseModel, new()
    {
        private readonly IDriveServerCollection driveServerCollection;

        public StarDataRepository(
            IContextInfoAccessor contextInfoAccessor,
            IDataAccessor dataAccessor,
            IDriveServerCollection driveServerCollection
            ) : base(contextInfoAccessor, dataAccessor)
        {
            this.driveServerCollection = driveServerCollection;
        }

        [MonitorAsyncAspect]
        public async Task<bool> DeleteByIdAsync(int id)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                return await dataAccessor.DeleteByIdAsync<T>(id, db) > 0;
            }
        }

        [MonitorAsyncAspect]
        public async  Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, string[] referenceFields = null)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                return await dataAccessor.GetSingleAsync(db, predicate, referenceFields);
            }
        }

        [MonitorAsyncAspect]
        public async Task<(long count, IEnumerable<T> data)> GetAllAsync(Expression<Func<T, bool>> predicate, IPageable paging, string[] fields = null)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            { 
                return await dataAccessor.GetAllAsync(db, predicate, paging); 
            }
        } 
         
        [MonitorAsyncAspect]
        public async Task<long> InsertAsync(T item)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                return await dataAccessor.InsertAsync(db,item);
            }
        }

        [MonitorAsyncAspect]
        public async Task<bool> UpdateAsync(T item)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            { 
                return await dataAccessor.UpdateAsync(db, item);
            }
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        private OrmLiteConnectionFactory GetDbFactory()
        {
            int driveServerGuid = GetDriveServerGuid();
            return new OrmLiteConnectionFactory(
                    driveServerCollection.GetConnectionStringById(driveServerGuid),
                    SqlServerDialect.Provider
                    );
        }

        private static int GetDriveServerGuid()
        {
            // This must be changed when we have the claims working.
            return 21;
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate, IPageable paging)
        {
            throw new NotImplementedException();
        }
    }
}
