using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.ServiceLayer.Interfaces;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;

namespace DriveCentric.ServiceLayer.Services
{
    public abstract class BaseService<T> : BaseWithContextInfoAccessor, IBaseService<T>
        where T : IBaseModel
    {
        protected readonly IBaseLogic<T> businessObject;

        protected BaseService(
            IContextInfoAccessor contextInfoAccessor,
            IBaseLogic<T> businessObject
            ) : base(contextInfoAccessor)
        {
            this.businessObject = businessObject;
        }

        [MonitorAsyncAspect]
        public Task<bool> DeleteAsync(int id)
        {
            return businessObject.DeleteAsync(id);
        }

        [MonitorAsyncAspect]
        public Task<T> GetAsync(int id)
        {
            return businessObject.GetAsync(id);
        }

        [MonitorAsyncAspect]
        public Task<IEnumerable<T>> GetAsync(
            int? limit = null,
            int? offset = null,
            Expression predicate = null)
        {
            return businessObject.GetAsync(limit, offset, predicate);
        }

        [MonitorAsyncAspect]
        public Task<bool> InsertAsync(T item)
        {
            return businessObject.InsertAsync(item);
        }

        [MonitorAsyncAspect]
        public Task<bool> SaveAsync()
        {
            return businessObject.SaveAsync();
        }

        [MonitorAsyncAspect]
        public Task<bool> UpdateAsync(T item)
        {
            return businessObject.UpdateAsync(item);
        }
    }
}
