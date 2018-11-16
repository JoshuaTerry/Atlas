using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.Model;

namespace DriveCentric.BusinessLogic.Interfaces
{
    public interface IBaseLogic<T> where T : IBaseModel
    {
        Task<T> GetAsync(int id);
        Task<(long count, IEnumerable<T> data)> GetAsync(Expression<Func<T, bool>> predicate, IPageable paging, string[] fields = null);
        Task<IEnumerable<T>> GetAsync(
            int? limit = null,
            int? offset = null,
            Expression predicate = null);

        Task<bool> DeleteAsync(int id);

        Task<long> InsertAsync(T item);

        Task<bool> UpdateAsync(T item);

        Task<bool> SaveAsync();
    }
}
