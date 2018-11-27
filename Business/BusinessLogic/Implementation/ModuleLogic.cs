using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.Utilities.Context;
using DriveCentric.Model.Interfaces;

namespace DriveCentric.BusinessLogic.Implementation
{
    class ModuleLogic : BaseLogic<Module>, IModuleLogic
    {
        public ModuleLogic(
            IContextInfoAccessor contextInfoAccessor,
            IDataRepository<Module> dataRepository
            ) : base(contextInfoAccessor, dataRepository)
        { }
    }
}
