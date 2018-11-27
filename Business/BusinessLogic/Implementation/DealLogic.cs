using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Utilities.Context;
using DriveCentric.Core.Interfaces;

namespace DriveCentric.BusinessLogic.Implementation
{
    public class DealLogic : BaseLogic<Deal>, IDealLogic
    {
        public DealLogic(
            IContextInfoAccessor contextInfoAccessor,
            IDataRepository<Deal> dataRepository
            ) : base(contextInfoAccessor, dataRepository)
        { }
    }
}
