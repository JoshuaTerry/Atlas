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
        public async Task<IEnumerable<T>> GetAsync(
            int? limit = null,
            int? offset = null,
            Expression predicate = null)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                return (IEnumerable<T>) await dataAccessor.GetAsync(db, limit, offset, (Expression<Func<T, bool>>)predicate);
            }
        }


        [MonitorAsyncAspect]
        public async Task<(long count, IEnumerable<T> data)> GetAsync(Expression<Func<T, bool>> predicate, IPageable paging, string[] fields = null)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                //Expression<Func<T, bool>> expression = x => true; 
                ////parameter that will be used in generated expression
                //var param = Expression.Parameter(typeof(U));
                ////visiting body of original expression that gives us body of the new expression
                //var body = new Visitor<U>(param).Visit(expression.Body);
                ////generating lambda expression form body and parameter 
                ////notice that this is what you need to invoke the Method_2
                //Expression<Func<U, bool>> lambda = Expression.Lambda<Func<U, bool>>(body, param);
                ////compilation and execution of generated method just to prove that it works
                //var boolValue = lambda.Compile()(new U());

                ////var resultBody = Expression.Convert(predicate.Parameters[0], typeof(U));
                ////var resultExpression = Expression.Lambda<Func<T, bool>>(resultBody, predicate.Parameters);

                //var result = await dataAccessor.GetAsync(db, lambda, paging);
                var result = await dataAccessor.GetAsync(db, predicate, paging);
                return (result.count, (IEnumerable<T>)result.data); 
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
                return (IEnumerable<T>)dataAccessor.GetAsync(db, limit, offset, (Expression<Func<T, bool>>)predicate);
            }
        }

        [MonitorAsyncAspect]
        public async Task<T> GetByIdAsync(int id)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                var item = await dataAccessor.GetByIdAsync<T>(id, db);

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
    }
}
