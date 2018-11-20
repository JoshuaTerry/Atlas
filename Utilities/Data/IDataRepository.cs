﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.Model;

namespace DriveCentric.Utilities.Data
{
    public interface IDataRepository<T> where T : IBaseModel
    {
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate, IPageable paging); 
        Task<(long count, IEnumerable<T> data)> GetAllAsync(Expression<Func<T, bool>> predicate, IPageable paging, string[] referenceFields = null);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, string[] referenceFields = null);
        Task<bool> DeleteByIdAsync(int id);
        Task<long> InsertAsync(T item);
        Task<bool> UpdateAsync(T item);
        void Save();
    }
}
