using DriveCentric.Core.Interfaces;
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
    public class SqlRepository : IRepository
    {
        private readonly IDbConnectionFactory factory;

        public SqlRepository(string connectionString)
        {
            factory = new OrmLiteConnectionFactory(connectionString, SqlServerDialect.Provider);
        }

        public async Task<bool> IsDatabaseAvailable()
        {
            try
            {
                using (var connection = factory.CreateDbConnection())
                    connection.Open();

                return await Task.FromResult<bool>(true);
            }
            catch { return await Task.FromResult<bool>(false); }
        }

        public async Task<long> DeleteAsync<T>(Expression<Func<T, bool>> expression) where T : IBaseModel, new()
        {
            using (var connection = factory.OpenDbConnection())
                return await connection.DeleteAsync(expression);
        }

        public async Task<long> DeleteByIdAsync<T>(int id) where T : IBaseModel, new()
        {
            using (var connection = factory.OpenDbConnection())
                return await connection.DeleteByIdAsync<T>(id);
        }

        public async Task<long> GetCountAsync<T>(Expression<Func<T, bool>> expression) where T : IBaseModel, new()
        {
            using (var connection = factory.OpenDbConnection())
                return await connection.CountAsync<T>(expression);
        }

        public async Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> expression, string[] referenceFields = null) where T : IBaseModel, new()
        {
            using (var connection = factory.OpenDbConnection())
            {
                var result = await connection.SingleAsync<T>(expression);
                await connection.LoadReferencesAsync(result, referenceFields);

                return result;
            }
        }

        public T GetSingle<T>(Expression<Func<T, bool>> expression, string[] referenceFields = null) where T : IBaseModel, new()
        {
            using (var connection = factory.OpenDbConnection())
            {
                var result = connection.Single<T>(expression);
                connection.LoadReferencesAsync(result, referenceFields);

                return result;
            }
        }

        public async Task<long> InsertAsync<T>(T item) where T : IBaseModel, new()
        {
            using (var connection = factory.OpenDbConnection())
                return await connection.InsertAsync(item, selectIdentity: true);
        }

        public async Task<long> UpdateAsync<T>(T item) where T : IBaseModel, new()
        {
            using (var connection = factory.OpenDbConnection())
                return await connection.SaveAsync(item) ? 1 : 0;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> expression, IPageable paging, string[] referenceFields) where T : class, IBaseModel, new()
        {
            using (var connection = factory.OpenDbConnection())
            {
                bool isDescending = paging.OrderBy.StartsWith("-");
                var sortFields = GetModelOrderByField<T>(connection, paging);
                var sqlExpression = connection.From<T>().Where(expression).Limit(skip: paging.Offset, rows: paging.Limit);
                sqlExpression = isDescending ? sqlExpression.OrderByDescending(sortFields) : sqlExpression.OrderBy(sortFields);

                return await connection.LoadSelectAsync(sqlExpression, referenceFields);
            }
        }

        private string GetModelOrderByField<T>(IDbConnection conn, IPageable paging)
                => conn.From<T>().ModelDef.AllFieldDefinitionsArray.Where(f => f.Name.TrimStart('-') == paging.OrderBy).Select(f => string.IsNullOrWhiteSpace(f.Alias) ? f.Name : f.Alias).FirstOrDefault();
    }
}