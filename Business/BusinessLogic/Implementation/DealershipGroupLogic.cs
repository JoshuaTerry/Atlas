using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.Utilities.Context;
using DriveCentric.Model.Interfaces;

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
