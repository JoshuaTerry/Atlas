using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using DriveCentric.Utilities.Data;

namespace DriveCentric.BusinessLogic.Implementation
{
    public abstract class BaseLogic<T> : BaseWithContextInfoAccessor, IBaseLogic<T>
        where T : IBaseModel
    {
        protected readonly IDataRepository<T> dataRepository;

        protected BaseLogic(
            IContextInfoAccessor contextInfoAccessor,
            IDataRepository<T> dataRepository
            ) : base(contextInfoAccessor)
        {
            this.dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));
        }

        [MonitorAsyncAspect]
        public virtual Task<bool> DeleteAsync(int id)
        {
            return dataRepository.DeleteByIdAsync(id);
        }

        [MonitorAsyncAspect]
        public virtual Task<T> GetAsync(int id)
        {
            return dataRepository.GetByIdAsync(id);
        }

        [MonitorAsyncAspect]
        public virtual Task<IEnumerable<T>> GetAsync(
            int? limit = null,
            int? offset = null,
            Expression predicate = null)
        {
            return dataRepository.GetAsync(limit, offset, predicate);
        }

        [MonitorAsyncAspect]
        public virtual Task<bool> InsertAsync(T item)
        {
            throw new NotImplementedException();
        }

        [MonitorAsyncAspect]
        public virtual Task<bool> SaveAsync()
        {
            throw new NotImplementedException();
        }

        [MonitorAsyncAspect]
        public virtual Task<bool> UpdateAsync(T item)
        {
            throw new NotImplementedException();
        }
    }
}
