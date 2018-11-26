using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.Model;

namespace DriveCentric.BusinessLogic.Interfaces
{
    public interface IBaseLogic<T> where T : IBaseModel
    { 
        Task<(long count, IEnumerable<T> data)> GetAllAsync(Expression<Func<T, bool>> predicate, IPageable paging, string[] referenceFields = null);

        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, string[] referenceFields = null);
        Task<bool> DeleteAsync(int id);

        Task<long> InsertAsync(T item);

        Task<bool> UpdateAsync(T item);

        Task<bool> SaveAsync();
    }
}
