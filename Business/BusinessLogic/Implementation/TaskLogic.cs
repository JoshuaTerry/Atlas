using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.Utilities.Context;
using DriveCentric.Utilities.Data;

namespace DriveCentric.BusinessLogic.Implementation
{
    class TaskLogic : BaseLogic<Task>, ITaskLogic
    {
        public TaskLogic(
            IContextInfoAccessor contextInfoAccessor,
            IDataRepository<Task> dataRepository
            ) : base(contextInfoAccessor, dataRepository)
        { }
    }
}
