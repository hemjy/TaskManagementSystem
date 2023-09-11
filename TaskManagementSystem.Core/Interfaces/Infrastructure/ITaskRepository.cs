

using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Core.Models;

namespace TaskManagementSystem.Core.Interfaces.Infrastructure
{
    public interface ITaskRepository : IGenericRepository<Entities.Task>
    {
        Task<PagedList<TaskToReturn>> GetTasksByStatusOrPriorityAsync(int status, int priorty, int pageNumber, int pageSize);
        Task<PagedList<TaskToReturn>> GetTasksDueForCurrentWeekAsync(int pageNumber, int pageSize);
        
    }
}
