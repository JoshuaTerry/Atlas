using DriveCentric.BaseService.Services;
using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.Model.Interfaces;
using DriveCentric.Utilities.Context;

namespace DriveCentric.ModuleService.Services
{
    public class ModuleService : BaseService<Module>, IModuleService
    {
        public ModuleService(
            IContextInfoAccessor contextInfoAccessor,
            IUnitOfWork unitOfWork
            ) : base(contextInfoAccessor, unitOfWork)
        { }
    }
}
