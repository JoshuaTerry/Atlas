using DriveCentric.BaseService.Services;
using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.Utilities.Context;

namespace DriveCentric.ModuleService.Services
{
    public class ModuleService : BaseService<Module>, IModuleService
    {
        public ModuleService(
            IContextInfoAccessor contextInfoAccessor,
            IModuleLogic businessLogic
            ) : base(contextInfoAccessor, businessLogic)
        { }
    }
}
