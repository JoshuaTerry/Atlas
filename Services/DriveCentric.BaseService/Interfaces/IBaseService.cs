using DriveCentric.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DriveCentric.BaseService.Interfaces
{
    public interface IBaseService<T> where T : class, IBaseModel, new()
    {
        Task<IDataResponse<IEnumerable<T>>> GetAllByExpressionAsync(Expression<Func<T, bool>> expression = null, IPageable paging = null, string[] referenceFields = null);

        Task<IDataResponse<T>> GetSingleByExpressionAsync(Expression<Func<T, bool>> expression = null, string[] referenceFields = null);

        Task<IDataResponse<long>> DeleteAsync(int id);

        Task<IDataResponse<long>> InsertAsync(T entity);

        Task<IDataResponse<long>> UpdateAsync(T entity);
    }
}