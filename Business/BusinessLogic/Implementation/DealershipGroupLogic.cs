using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Utilities.Context;
using DriveCentric.Core.Interfaces;

namespace DriveCentric.BusinessLogic.Implementation
{
    public class DealershipGroupLogic : BaseLogic<DealershipGroup>, IDealershipGroupLogic
    {
        public DealershipGroupLogic(
            IContextInfoAccessor contextInfoAccessor,
            IDataRepository<DealershipGroup> dataRepository
            ) : base(contextInfoAccessor, dataRepository)
        { }
    }
}
