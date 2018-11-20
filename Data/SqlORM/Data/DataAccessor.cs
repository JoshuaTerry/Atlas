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
        private string GetModelOrderByField<T>(IDbConnection connection, IPageable paging)
            => connection.From<T>().ModelDef.AllFieldDefinitionsArray.Where(f => f.Name.TrimStart('-') == paging.OrderBy).Select(f => string.IsNullOrWhiteSpace(f.Alias) ? f.Name : f.Alias).FirstOrDefault();

        public async Task<T> GetSingleAsync<T>(IDbConnection connection, Expression<Func<T, bool>> predicate, string[] referenceFields = null) where T : IBaseModel
        { 
            var result = await connection.SingleAsync<T>(predicate);
            await connection.LoadReferencesAsync(result, referenceFields);
            return result;
            //return await connection.SingleAsync<T>(predicate );   
        }

        public async Task<(long count, IEnumerable<T> data)> GetAllAsync<T>(IDbConnection connection, Expression<Func<T, bool>> predicate, IPageable paging, string[] referenceFields = null)
            where T : IBaseModel
        {
            var count = connection.Count<T>(predicate);
            bool isDescending = paging.OrderBy.StartsWith("-");  

            if (isDescending)
                return (count, await connection.LoadSelectAsync(connection.From<T>().Where(predicate).Limit(skip: paging.Offset, rows: paging.Limit).OrderByDescending(GetModelOrderByField<T>(connection, paging)), referenceFields));
            else
                return (count, await connection.LoadSelectAsync(connection.From<T>().Where(predicate).Limit(skip: paging.Offset, rows: paging.Limit).OrderBy(GetModelOrderByField<T>(connection, paging)), referenceFields));
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

        public IEnumerable<T> Get<T>(IDbConnection connection, Expression<Func<T, bool>> predicate, IPageable paging) where T : IBaseModel
        {
            throw new NotImplementedException();
        } 
    }
}
