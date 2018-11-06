using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.BaseService.Interfaces;
using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;

namespace DriveCentric.BaseService.Services
{
    public abstract class BaseService<T> : BaseWithContextInfoAccessor, IBaseService<T>
        where T : IBaseModel
    {
        protected readonly IBaseLogic<T> businessLogic;

        protected BaseService(
            IContextInfoAccessor contextInfoAccessor,
            IBaseLogic<T> businessLogic
            ) : base(contextInfoAccessor)
        {
            this.businessLogic = businessLogic;
        }

        [MonitorAsyncAspect]
        public virtual Task<bool> DeleteAsync(int id)
        {
            return businessLogic.DeleteAsync(id);
        }

        [MonitorAsyncAspect]
        public virtual Task<T> GetAsync(int id)
        {
            return businessLogic.GetAsync(id);
        }

        [MonitorAsyncAspect]
        public virtual Task<IEnumerable<T>> GetAsync(
            int? limit = null,
            int? offset = null,
            Expression predicate = null)
        {
            return businessLogic.GetAsync(limit, offset, predicate);
        }

        [MonitorAsyncAspect]
        public virtual Task<long> InsertAsync(T item)
        {
            return businessLogic.InsertAsync(item);
        }

        [MonitorAsyncAspect]
        public virtual Task<bool> SaveAsync()
        {
            return businessLogic.SaveAsync();
        }

        [MonitorAsyncAspect]
        public virtual Task<bool> UpdateAsync(T item)
        {
            return businessLogic.UpdateAsync(item);
        }
    }
}
