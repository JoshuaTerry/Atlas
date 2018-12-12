using DriveCentric.BaseService.Services;
using DriveCentric.Core.Interfaces;
using DriveCentric.Core.Models;
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