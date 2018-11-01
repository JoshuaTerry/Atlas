using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DriveCentric.Data.SqlORM.Model;
using DriveCentric.Model;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Configuration;
using DriveCentric.Utilities.Context;
using DriveCentric.Utilities.Data;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace DriveCentric.Data.SqlORM.Repositories
{
    public class StarDataRepository<T, U> : BaseDataRepository, IDataRepository<T>
        where T : IBaseModel where U : T, new()
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
                return await dataAccessor.DeleteByIdAsync<U>(id, db) > 0;
            }
        }

        [MonitorAsyncAspect]
        public async Task<IEnumerable<T>> GetAsync(
            int? limit = null,
            int? offset = null,
            Expression predicate = null)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
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
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                return (IEnumerable<T>)dataAccessor.GetAsync(db, limit, offset, (Expression<Func<U, bool>>)predicate);
            }
        }

        [MonitorAsyncAspect]
        public async Task<T> GetByIdAsync(int id)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
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
        public async Task<long> InsertAsync(T item)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                return await dataAccessor.InsertAsync(db, ConvertToDataModel(item));
            }
        }

        [MonitorAsyncAspect]
        public Task<bool> UpdateAsync(T item)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                return dataAccessor.UpdateAsync(db, ConvertToDataModel(item));
            }
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        private U ConvertToDataModel(T item)
        {
            U instance = new U();
            var writableProperties = typeof(T).GetProperties();
            writableProperties.ToList().ForEach(property => property.SetValue(instance, property.GetValue(item)));
            return instance;
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
    }
}
