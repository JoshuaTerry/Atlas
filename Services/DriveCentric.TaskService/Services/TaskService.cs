using DriveCentric.BaseService.Services;
using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model.Interfaces;
using DriveCentric.Utilities.Context;

namespace DriveCentric.TaskService.Services
{
    public class TaskService : BaseService<Core.Models.Task>, ITaskService
    {
        public TaskService(
            IContextInfoAccessor contextInfoAccessor,
            IUnitOfWork unitOfWork,
            ITaskLogic businessLogic
            ) : base(contextInfoAccessor, unitOfWork)
        {
             
        }

        public bool MergeTasks(Task task1, Task task2)
        {
            return true;
        }
    }
}
