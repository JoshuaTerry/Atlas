using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.Utilities.Context;
using DriveCentric.Utilities.Data;

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
