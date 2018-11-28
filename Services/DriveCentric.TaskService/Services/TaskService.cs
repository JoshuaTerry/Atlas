using DriveCentric.BaseService.Services;
using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Utilities.Context;

namespace DriveCentric.TaskService.Services
{
    public class TaskService : BaseService<Core.Models.Task>, ITaskService
    {
        public TaskService(
            IContextInfoAccessor contextInfoAccessor,
            ITaskLogic businessLogic
            ) : base(contextInfoAccessor, businessLogic)
        { }
    }
}
