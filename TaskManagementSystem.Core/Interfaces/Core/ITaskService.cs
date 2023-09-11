using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Core.Models;

namespace TaskManagementSystem.Core.Interfaces.Core
{
    public interface ITaskService
    {
        Task<PagedList<TaskToReturn>> GetTasksByStatusOrPriorityAsync(int status, int priorty, int pageNumber, int pageSize);
        Task<PagedList<TaskToReturn>> GetTasksDueForCurrentWeekAsync(int pageNumber, int pageSize);
        Task AssignOrRemoveToProject(TaskAction taskAction);
        Task AssignToUser(Guid userId, Guid taskId);
        Task UpdateStatus(int status, Guid taskId);
    }
}
