using DriveCentric.Model;
using DriveCentric.Model.Interfaces;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DriveCentric.Data.DataRepository.Repositories
{
    public class Repository : IRepository
    {       
        public Repository()
        {           
        } 
        public async Task<IEnumerable<T>> GetAllAsync<T>(IDbConnection connection, Expression<Func<T, bool>> expression, IPageable paging, string[] referenceFields = null) where T : class, IBaseModel, new() 
        {
            bool isDescending = paging.OrderBy.StartsWith("-");
            var sortFields = GetModelOrderByField<T>(connection, paging);
            var sqlExpression = connection.From<T>().Where(expression).Limit(skip: paging.Offset, rows: paging.Limit); 
            sqlExpression = isDescending ? sqlExpression.OrderByDescending(sortFields) : sqlExpression.OrderBy(sortFields);

            return await connection.LoadSelectAsync(sqlExpression, referenceFields); 
        }
        public async Task<long> GetCount<T>(IDbConnection connection, Expression<Func<T, bool>> expression) where T : IBaseModel, new()
            => await connection.CountAsync<T>(expression);

        [MonitorAsyncAspect]
        public async Task<long> DeleteByIdAsync<T>(IDbConnection connection, int id) where T : IBaseModel, new()
            => await connection.DeleteByIdAsync<T>(id);

        [MonitorAsyncAspect]
        public async Task<long> DeleteAsync<T>(IDbConnection connection, Expression<Func<T, bool>> expression) where T : IBaseModel, new()
            => await connection.DeleteAsync(expression);

        [MonitorAsyncAspect]
        public virtual async Task<T> GetSingleAsync<T>(IDbConnection connection, Expression<Func<T, bool>> expression, string[] referenceFields = null) where T : IBaseModel, new()
        {
            var result = await connection.SingleAsync<T>(expression);
            await connection.LoadReferencesAsync(result, referenceFields);
            return result;
        }

        [MonitorAsyncAspect]
        public virtual async Task<long> InsertAsync<T>(IDbConnection connection, T item) where T : IBaseModel, new()
            => await connection.InsertAsync(item, selectIdentity: true);

        [MonitorAsyncAspect]
        public virtual async Task<long> UpdateAsync<T>(IDbConnection connection, T item) where T : IBaseModel, new()
            => await connection.SaveAsync(item) ? 1 : 0;

        private string GetModelOrderByField<T>(IDbConnection connection, IPageable paging)
            => connection.From<T>().ModelDef.AllFieldDefinitionsArray.Where(f => f.Name.TrimStart('-') == paging.OrderBy).Select(f => string.IsNullOrWhiteSpace(f.Alias) ? f.Name : f.Alias).FirstOrDefault();
    }
}
