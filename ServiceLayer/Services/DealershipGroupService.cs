using DriveCentric.BaseService.Services;
using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.ServiceLayer.Interfaces;
using DriveCentric.Utilities.Context;

namespace DriveCentric.ServiceLayer.Services
{
    public class DealershipGroupService : BaseService<DealershipGroup>, IDealershipGroupService
    {
        public DealershipGroupService(
            IContextInfoAccessor contextInfoAccessor,
            IDealershipGroupLogic businessLogic
            ) : base(contextInfoAccessor, businessLogic)
        { }
    }
}
