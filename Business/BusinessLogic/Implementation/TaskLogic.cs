using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Utilities.Context;
using DriveCentric.Core.Interfaces;

namespace DriveCentric.BusinessLogic.Implementation
{
    public class TaskLogic : BaseLogic<Task>, ITaskLogic
    {
        public TaskLogic(
            IContextInfoAccessor contextInfoAccessor,
            IDataRepository<Task> dataRepository
            ) : base(contextInfoAccessor, dataRepository)
        { }
    }
}
