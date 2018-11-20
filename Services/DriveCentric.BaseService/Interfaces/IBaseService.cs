using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.Model;

namespace DriveCentric.BaseService.Interfaces
{
    public interface IBaseService<T> where T : IBaseModel
    { 
        Task<IDataResponse<IEnumerable<T>>> GetAllByExpressionAsync(Expression<Func<T, bool>> predicate = null, IPageable paging = null, string[] referenceFields = null);
        Task<IDataResponse<T>> GetSingleByExpressionAsync(Expression<Func<T, bool>> predicate = null, string[] referenceFields = null);
        Task<bool> DeleteAsync(int id);

        Task<long> InsertAsync(T item);

        Task<bool> UpdateAsync(T item);

        Task<bool> SaveAsync();
    }
}
