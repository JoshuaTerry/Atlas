using DriveCentric.BaseService.Services;
using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.Utilities.Context;

namespace DriveCentric.Task.Services
{
    public class TaskService : BaseService<ITask>, ITaskService
    {
        public TaskService(
            IContextInfoAccessor contextInfoAccessor,
            ITaskLogic businessLogic
            ) : base(contextInfoAccessor, businessLogic)
        { }
    }
}
