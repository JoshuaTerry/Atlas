using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.Model;

namespace DriveCentric.Utilities.Data
{
    public interface IDataRepository<T> where T : IBaseModel
    {
        IEnumerable<T> Get(
            int? limit = null,
            int? offset = null,
            Expression predicate = null);

        Task<IEnumerable<T>> GetAsync(
            int? limit = null,
            int? offset = null,
            Expression predicate = null);

        Task<(long count, IEnumerable<T> data)> GetAsync(Expression predicate, IPageable paging, string[] fields = null);
        Task<T> GetByIdAsync(int id);
        Task<bool> DeleteByIdAsync(int id);
        Task<long> InsertAsync(T item);
        Task<bool> UpdateAsync(T item);
        void Save();
    }
}
