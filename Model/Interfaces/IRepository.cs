using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DriveCentric.Core.Interfaces
{
    public interface IRepository
    {
        Task<bool> IsDatabaseAvailable();

        Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> expression, IPageable paging, string[] referenceFields = null)
            where T : class, IBaseModel, new();

        Task<long> GetCountAsync<T>(Expression<Func<T, bool>> expression)
            where T : IBaseModel, new();

        Task<long> DeleteByIdAsync<T>(int id)
            where T : IBaseModel, new();

        Task<long> DeleteAsync<T>(Expression<Func<T, bool>> expression)
            where T : IBaseModel, new();

        Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> expression, string[] referenceFields = null)
            where T : IBaseModel, new();

        T GetSingle<T>(Expression<Func<T, bool>> expression, string[] referenceFields = null)
            where T : IBaseModel, new();

        Task<long> InsertAsync<T>(T item)
            where T : IBaseModel, new();

        Task<long> UpdateAsync<T>(T item)
            where T : IBaseModel, new();
    }
}