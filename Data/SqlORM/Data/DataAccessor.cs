using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.Model;
using DriveCentric.Utilities.Data;
using ServiceStack.OrmLite;

namespace DriveCentric.Data.SqlORM.Data
{
    public class DataAccessor : IDataAccessor
    {
        public Task<T> GetByIdAsync<T>(int id, IDbConnection connection, string query = null)
            where T : IBaseExternalIdModel
        {
            return connection.LoadSingleByIdAsync<T>(id);
        }

        public Task<int> DeleteByIdAsync<T>(int id, IDbConnection connection, string query = null)
            where T : IBaseExternalIdModel
        {
            return connection.DeleteByIdAsync<T>(id);
        }

        public async Task<IEnumerable<T>> GetAsync<T>(
            IDbConnection connection,
            int? limit = null,
            int? offset = null,
            Expression<Func<T, bool>> predicate = null
            )
            where T : IBaseExternalIdModel
        {
            return await connection.SelectAsync(connection.From<T>().Where(predicate).Limit(skip: offset, rows: limit));
        }

        public IEnumerable<T> Get<T>(
            IDbConnection connection,
            int? limit = null,
            int? offset = null,
            Expression<Func<T, bool>> predicate = null
            )
            where T : IBaseExternalIdModel
        {
            return  connection.Select(connection.From<T>().Where(predicate).Limit(skip: offset, rows: limit));
        }
    }
}
