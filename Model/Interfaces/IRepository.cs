using DriveCentric.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DriveCentric.Core.Interfaces
{
    public interface IRepository
    { 
        Task<IEnumerable<T>> GetAllAsync<T>(IDbConnection connection, Expression<Func<T, bool>> expression, IPageable paging, string[] referenceFields = null) where T : class, IBaseModel, new();
        Task<long> GetCount<T>(IDbConnection connection, Expression<Func<T, bool>> expression) where T : IBaseModel, new();
        Task<long> DeleteByIdAsync<T>(IDbConnection connection, int id) where T : IBaseModel, new();
        Task<long> DeleteAsync<T>(IDbConnection connection, Expression<Func<T, bool>> expression) where T : IBaseModel, new();
        Task<T> GetSingleAsync<T>(IDbConnection connection, Expression<Func<T, bool>> expression, string[] referenceFields = null) where T : IBaseModel, new();
        Task<long> InsertAsync<T>(IDbConnection connection, T item) where T : IBaseModel, new();
        Task<long> UpdateAsync<T>(IDbConnection connection, T item) where T : IBaseModel, new();
    }
}
