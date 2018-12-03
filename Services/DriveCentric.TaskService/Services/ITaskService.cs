using DriveCentric.BaseService.Interfaces;
using DriveCentric.Model;

namespace DriveCentric.TaskService.Services
{
    public interface ITaskService : IBaseService<Task>
    {
        bool MergeTasks(Task task1, Task task2);
    }
}
