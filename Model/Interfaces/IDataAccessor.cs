using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DriveCentric.Core.Interfaces
{
    public interface IDataAccessor
    {
        Task<(long count, IEnumerable<T> data)> GetAllAsync<T>(IDbConnection connection, Expression<Func<T, bool>> predicate, IPageable paging, string[] referenceFields = null)
            where T : IBaseModel;
        Task<T> GetSingleAsync<T>(IDbConnection connection, Expression<Func<T, bool>> predicate, string[] referenceFields = null)
            where T : IBaseModel;

        Task<int> DeleteByIdAsync<T>(int id, IDbConnection connection, string query = null)
            where T : IBaseModel;

        Task<long> InsertAsync<T>(IDbConnection connection, T item) where T : IBaseModel;

        Task<bool> UpdateAsync<T>(IDbConnection connection, T item) where T : IBaseModel;
    }
}
