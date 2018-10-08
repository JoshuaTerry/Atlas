using System;
using System.Threading.Tasks;
using DriveCentric.BusinessLogic.DealershipGroup;
using DriveCentric.Model;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;

namespace DriveCentric.ServiceLayer.DealershipGroup
{
    public class DealershipGroupService : BaseService, IDealershipGroupService
    {
        private readonly IDealershipGroupBusinessObject dealershipGroupBusinessObject;

        public DealershipGroupService(
            IContextInfoAccessor contextInfoAccessor,
            IDealershipGroupBusinessObject dealershipGroupBusinessObject
            ) : base(contextInfoAccessor)
        {
            this.dealershipGroupBusinessObject = dealershipGroupBusinessObject;
        }

        [MonitorAsyncAspect]
        public Task<bool> DeleteDealershipGroupAsync(int id)
        {
            return dealershipGroupBusinessObject.DeleteDealershipGroupAsync(id);
        }

        [MonitorAsyncAspect]
        public Task<IDealershipGroup> GetDealershipGroupAsync(int id)
        {
            return dealershipGroupBusinessObject.GetDealershipGroupAsync(id);
        }
    }
}
