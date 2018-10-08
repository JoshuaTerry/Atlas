using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.BusinessLogic.Deal;
using DriveCentric.Model;
using DriveCentric.Utilities.Context;

namespace DriveCentric.ServiceLayer.Deal
{
    public class DealService : BaseService, IDealService
    {
        private readonly IDealBusinessObject businessObject;

        public DealService(
            IContextInfoAccessor contextInfoAccessor,
            IDealBusinessObject businessObject
            ) : base(contextInfoAccessor)
        {
            this.businessObject = businessObject;
        }

        public Task<bool> DeleteDeal(int id)
        {
            return businessObject.DeleteDealAsync(id);
        }

        public Task<IEnumerable<IDeal>> GetCustomersAsync(
            int? limit = null,
            int? offset = null,
            Expression predicate = null)
        {
            return businessObject.GetDealsAsync(limit, offset, predicate);
        }

        public Task<IDeal> GetDeal(int id)
        {
            return businessObject.GetDealAsync(id);
        }
    }
}
