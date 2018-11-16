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
            where T : IBaseModel
        {
            return connection.LoadSingleByIdAsync<T>(id);
        }

        public Task<int> DeleteByIdAsync<T>(int id, IDbConnection connection, string query = null)
            where T : IBaseModel
        {
            return connection.DeleteByIdAsync<T>(id);
        }

        public async Task<IEnumerable<T>> GetAsync<T>(
            IDbConnection connection,
            int? limit = null,
            int? offset = null,
            Expression<Func<T, bool>> predicate = null
            )
            where T : IBaseModel
        {
            return await connection.SelectAsync(connection.From<T>().Where(predicate).Limit(skip: offset, rows: limit));
        }

        public async Task<(long count, IEnumerable<T> data)> GetAsync<T>(IDbConnection connection, Expression<Func<T, bool>> predicate, IPageable paging, string[] fields = null)
            where T : IBaseModel
        {
            // This isn't working for some reason.  How do we get the count?
            //var count = await connection.ScalarAsync<long>(connection.From<T>().Where(predicate).ToCountStatement());
            var count = 4;

            // Fields works but only if you don't reference properties of complex types e.g. for a Task, Customer.FirstName.  
            // How do you pass in these fields restrictions for complex types?
            fields = null;
            
            // How do you get Order By to work for properties with Alias's?  For instance ordering on UserID but the alias is fkUserId
            if (paging.OrderBy.StartsWith("-"))
                return (count, await connection.LoadSelectAsync(connection.From<T>().Where(predicate).Limit(skip: paging.Offset, rows: paging.Limit)));
            //return (count, await connection.LoadSelectAsync(connection.From<T>().Where(predicate).Limit(skip: paging.Offset, rows: paging.Limit).OrderByDescending(paging.OrderBy.Replace("-", string.Empty))));
            else
                return (count, await connection.LoadSelectAsync(connection.From<T>().Where(predicate).Limit(skip: paging.Offset, rows: paging.Limit), fields));
            //return (count, await connection.LoadSelectAsync(connection.From<T>().Where(predicate).Limit(skip: paging.Offset, rows: paging.Limit).OrderBy(paging.OrderBy), fields));
        }

        public IEnumerable<T> Get<T>(
            IDbConnection connection,
            int? limit = null,
            int? offset = null,
            Expression<Func<T, bool>> predicate = null
            )
            where T : IBaseModel
        {
            return  connection.Select(connection.From<T>().Where(predicate).Limit(skip: offset, rows: limit));
        }

        public async Task<long> InsertAsync<T>(IDbConnection connection, T item) where T : IBaseModel
        {
            try
            {
                return await connection.InsertAsync(item, selectIdentity: true);
            }
            catch(Exception exception)
            {
                throw new Exception($"Error trying to insert {typeof(T)}.", exception);
            }
        }

        public async Task<bool> UpdateAsync<T>(IDbConnection connection, T item) where T : IBaseModel
        {
            try
            {
                return await connection.UpdateAsync(item) > 0L;
            }
            catch (Exception exception)
            {
                throw new Exception($"Error trying to update {typeof(T)}.", exception);
            }
        }
    }
}
