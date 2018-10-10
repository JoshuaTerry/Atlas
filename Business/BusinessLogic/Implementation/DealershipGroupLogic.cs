using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.Utilities.Context;
using DriveCentric.Utilities.Data;

namespace DriveCentric.BusinessLogic.Implementation
{
    public class DealershipGroupLogic : BaseLogic<IDealershipGroup>, IDealershipGroupLogic
    {
        public DealershipGroupLogic(
            IContextInfoAccessor contextInfoAccessor,
            IDataRepository<IDealershipGroup> dataRepository
            ) : base(contextInfoAccessor, dataRepository)
        { }
    }
}
