﻿using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.Model;

namespace DriveCentric.BaseService.Interfaces
{
    public interface IBaseService<T> where T : IBaseModel
    {
        Task<T> GetAsync(int id);

        Task<IEnumerable<T>> GetAsync(
            int? limit = null,
            int? offset = null,
            Expression predicate = null);

        Task<bool> DeleteAsync(int id);

        Task<bool> InsertAsync(T item);

        Task<bool> UpdateAsync(T item);

        Task<bool> SaveAsync();
    }
}
