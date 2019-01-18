using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DriveCentric.Core.Interfaces
{
    public interface IReadOnlyUnitOfWork
    {
        Task<long> GetCount<T>(Expression<Func<T, bool>> expression)
            where T : IBaseModel, new();

        Task<IEnumerable<T>> GetEntities<T>(Expression<Func<T, bool>> expression, IPageable paging, string[] referenceFields = null)
            where T : class, IBaseModel, new();

        Task<T> GetEntity<T>(Expression<Func<T, bool>> expression, string[] referenceFields = null)
            where T : IBaseModel, new();
    }
}