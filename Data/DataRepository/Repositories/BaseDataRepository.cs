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

namespace DriveCentric.Data.SqlORM.Repositories
{
    public interface IRepository
    {
        SqlExpression<T> GetEntities<T>();
        Task<IEnumerable<T>> GetAllAsync<T>(IDbConnection connection, SqlExpression<T> expression, string[] referenceFields = null) where T : IBaseModel, new();
        Task<long> GetCount<T>(IDbConnection connection, SqlExpression<T> expression) where T : IBaseModel, new();
        Task<long> DeleteByIdAsync<T>(IDbConnection connection, int id) where T : IBaseModel, new();
        Task<long> DeleteAsync<T>(IDbConnection connection, SqlExpression<T> expression) where T : IBaseModel, new();
        Task<T> GetSingleAsync<T>(IDbConnection connection, SqlExpression<T> expression, string[] referenceFields = null) where T : IBaseModel, new();
        Task<long> InsertAsync<T>(IDbConnection connection, T item) where T : IBaseModel, new();
        Task<long> UpdateAsync<T>(IDbConnection connection, T item) where T : IBaseModel, new();            
    }
    public class Repository : IRepository
    {
        private readonly IContextInfoAccessor contextInfoAccessor;
        private readonly IDbConnectionFactory dbConnectionFactory; 
        public Repository(IContextInfoAccessor contextInfoAccessor, IDbConnectionFactory dbConnectionFactory)
        {
            this.contextInfoAccessor = contextInfoAccessor;
            this.dbConnectionFactory = dbConnectionFactory;
        }
        public SqlExpression<T> GetEntities<T>() => dbConnectionFactory.CreateDbConnection().From<T>();

        public async Task<IEnumerable<T>> GetAllAsync<T>(IDbConnection connection, SqlExpression<T> expression, string[] referenceFields = null) where T : IBaseModel, new()
            => await connection.LoadSelectAsync(expression, referenceFields);

        public async Task<long> GetCount<T>(IDbConnection connection, SqlExpression<T> expression) where T : IBaseModel, new() 
            => await connection.CountAsync<T>(expression);

        [MonitorAsyncAspect]
        public async Task<long> DeleteByIdAsync<T>(IDbConnection connection, int id) where T : IBaseModel, new()
            => await connection.DeleteByIdAsync<T>(id);

        [MonitorAsyncAspect]
        public async Task<long> DeleteAsync<T>(IDbConnection connection, SqlExpression<T> expression) where T : IBaseModel, new()
            => await connection.DeleteAsync(expression);  

        [MonitorAsyncAspect]
        public virtual async Task<T> GetSingleAsync<T>(IDbConnection connection, SqlExpression<T> expression, string[] referenceFields = null) where T : IBaseModel, new()
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

    public abstract class BaseDataRepository<T> : IDataRepository<T> where T : IBaseModel, new() 
    {
        protected readonly IDataAccessor dataAccessor;
        public IContextInfoAccessor ContextInfoAccessor { get; }

        protected BaseDataRepository(IContextInfoAccessor contextInfoAccessor)  
        { 
            this.ContextInfoAccessor = contextInfoAccessor;
        }

        [MonitorAsyncAspect]
        public virtual async Task<bool> DeleteByIdAsync(int id)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {   
                return await db.DeleteByIdAsync<T>(id) > 0;
            }
        }

        [MonitorAsyncAspect]
        public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, string[] referenceFields = null)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                var result = await db.SingleAsync<T>(predicate);
                await db.LoadReferencesAsync(result, referenceFields);
                return result;
            }
        }

        [MonitorAsyncAspect]
        public virtual async Task<(long count, IEnumerable<T> data)> GetAllAsync(Expression<Func<T, bool>> predicate, IPageable paging, string[] referenceFields = null)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                var count = db.Count<T>(predicate);
                bool isDescending = paging.OrderBy.StartsWith("-");

                if (isDescending)
                    return (count, await db.LoadSelectAsync(db.From<T>().Where(predicate).Limit(skip: paging.Offset, rows: paging.Limit).OrderByDescending(GetModelOrderByField(db, paging)), referenceFields));
                else
                    return (count, await db.LoadSelectAsync(db.From<T>().Where(predicate).Limit(skip: paging.Offset, rows: paging.Limit).OrderBy(GetModelOrderByField(db, paging)), referenceFields));
            }
        }

        [MonitorAsyncAspect]
        public virtual async Task<long> InsertAsync(T item)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                try
                {
                    return await db.InsertAsync(item, selectIdentity: true);
                }
                catch (Exception exception)
                {
                    throw new Exception($"Error trying to insert {typeof(T)}.", exception);
                }
            }
        }

        [MonitorAsyncAspect]
        public virtual async Task<bool> UpdateAsync(T item)
        {
            using (IDbConnection db = GetDbFactory().OpenDbConnection())
            {
                try
                {
                    return await db.UpdateAsync(item) > 0;
                }
                catch (Exception exception)
                {
                    throw new Exception($"Error trying to update {typeof(T)}.", exception);
                }
            }
        }

        public abstract IDbConnectionFactory GetDbFactory();

        private string GetModelOrderByField(IDbConnection connection, IPageable paging)
            => connection.From<T>().ModelDef.AllFieldDefinitionsArray.Where(f => f.Name.TrimStart('-') == paging.OrderBy).Select(f => string.IsNullOrWhiteSpace(f.Alias) ? f.Name : f.Alias).FirstOrDefault();
    }
}
