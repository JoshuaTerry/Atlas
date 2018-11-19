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

        public async Task<(long count, IEnumerable<T> data)> GetAsync<T>(IDbConnection connection, Expression<Func<T, bool>> predicate, IPageable paging, string[] referenceFields = null)
            where T : IBaseModel
        { 
            var count = connection.Count<T>(predicate);
            bool isDescending = paging.OrderBy.StartsWith("-"); 
            var fieldDefinition = connection.From<T>().ModelDef.AllFieldDefinitionsArray.First(f => paging.OrderBy == f.Name);  
            var orderByField = string.IsNullOrEmpty(fieldDefinition.Alias) ? fieldDefinition.Name : fieldDefinition.Alias; 
              
            var filterFields = referenceFields.Select(f =>
            { 
                var index = f.IndexOf(".");
                return index > 0 ? f.Substring(0, index) : f; 
            }).Distinct().ToArray();

            if (isDescending)
                return (count, await connection.LoadSelectAsync(connection.From<T>().Where(predicate).Limit(skip: paging.Offset, rows: paging.Limit).OrderByDescending(orderByField), filterFields));
            else
                return (count, await connection.LoadSelectAsync(connection.From<T>().Where(predicate).Limit(skip: paging.Offset, rows: paging.Limit).OrderBy(orderByField), filterFields)); 
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
