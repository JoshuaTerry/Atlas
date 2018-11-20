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
          
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, string[] referenceFields = null)
        {
            return await dataRepository.GetSingleAsync(predicate, referenceFields); 
        }
        public async Task<(long count, IEnumerable<T> data)> GetAllAsync(Expression<Func<T, bool>> predicate, IPageable paging, string[] fields = null)
        {
            return await dataRepository.GetAllAsync(predicate, paging);
        }

        [MonitorAsyncAspect]
        public virtual Task<long> InsertAsync(T item)
        {
            return dataRepository.InsertAsync(item);
        }

        [MonitorAsyncAspect]
        public virtual Task<bool> SaveAsync()
        {
            throw new NotImplementedException();
        }

        [MonitorAsyncAspect]
        public virtual Task<bool> UpdateAsync(T item)
        {
            return dataRepository.UpdateAsync(item);
        }
    }
}
