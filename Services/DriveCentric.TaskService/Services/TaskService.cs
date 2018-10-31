using System.Threading.Tasks;
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

        // Remove this, only here for development
        public override Task<bool> UpdateAsync(ITask item)
        {
            return System.Threading.Tasks.Task.FromResult(true);
        }    
    }
}
