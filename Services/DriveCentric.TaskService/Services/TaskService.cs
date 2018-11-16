using DriveCentric.BaseService.Services;
using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.Utilities.Context;

namespace DriveCentric.TaskService.Services
{
    public class TaskService : BaseService<Task>, ITaskService
    {
        public TaskService(
            IContextInfoAccessor contextInfoAccessor,
            ITaskLogic businessLogic
            ) : base(contextInfoAccessor, businessLogic)
        { }
    }
}
