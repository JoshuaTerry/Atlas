using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.Utilities.Context;
using DriveCentric.Utilities.Data;

namespace DriveCentric.BusinessLogic.Implementation
{
    class TaskLogic : BaseLogic<ITask>, ITaskLogic
    {
        public TaskLogic(
            IContextInfoAccessor contextInfoAccessor,
            IDataRepository<ITask> dataRepository
            ) : base(contextInfoAccessor, dataRepository)
        { }
    }
}
