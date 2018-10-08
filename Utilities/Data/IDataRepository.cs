using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.Model;

namespace DriveCentric.Utilities.Data
{
    public interface IDataRepository<T> where T : IBaseExternalIdModel
    {
        IEnumerable<T> Get(
            int? limit = null,
            int? offset = null,
            Expression predicate = null);

        Task<IEnumerable<T>> GetAsync(
            int? limit = null,
            int? offset = null,
            Expression predicate = null);

        Task<T> GetByIdAsync(int id);
        Task<bool> DeleteByIdAsync(int id);
        void Insert(T item);
        void Update(T item);
        void Save();
    }
}
