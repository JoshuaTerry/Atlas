using System;
using System.Threading.Tasks;
using DriveCentric.Model;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using DriveCentric.Utilities.Data;

namespace DriveCentric.BusinessLogic.DealershipGroup
{
    public class DealershipGroupBusinessObject : BaseBusinessObject, IDealershipGroupBusinessObject
    {
        private readonly IDataRepository<IDealershipGroup> dealershipGroupDataRepository;

        public DealershipGroupBusinessObject(
            IContextInfoAccessor contextInfoAccessor,
            IDataRepository<IDealershipGroup> dealershipGroupDataRepository
            ) : base(contextInfoAccessor)
        {
            this.dealershipGroupDataRepository = dealershipGroupDataRepository ?? throw new ArgumentNullException(nameof(dealershipGroupDataRepository));
        }

        [MonitorAsyncAspect]
        public Task<bool> DeleteDealershipGroupAsync(int id)
        {
            return dealershipGroupDataRepository.DeleteByIdAsync(id);
        }

        [MonitorAsyncAspect]
        public Task<IDealershipGroup> GetDealershipGroupAsync(int id)
        {
            return dealershipGroupDataRepository.GetByIdAsync(id);
        }
    }
}
