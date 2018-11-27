using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Utilities.Context;
using DriveCentric.Core.Interfaces;

namespace DriveCentric.BusinessLogic.Implementation
{
    public class ModuleLogic : BaseLogic<Module>, IModuleLogic
    {
        public ModuleLogic(
            IContextInfoAccessor contextInfoAccessor,
            IDataRepository<Module> dataRepository
            ) : base(contextInfoAccessor, dataRepository)
        { }
    }
}
