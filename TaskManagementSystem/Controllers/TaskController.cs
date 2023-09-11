using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Core.Interfaces.Core;
using TaskManagementSystem.Core.Models;

namespace TaskManagementSystem.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        [HttpGet("statusorpriorty")]
        [ProducesResponseType(typeof(PagedList<TaskToReturn>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTasksByStatusOrPriority(int status, int prority, int pageNumber = 1, int pageSize = 50)
        {
            return Ok(await _taskService.GetTasksByStatusOrPriorityAsync(status, prority, pageNumber, pageSize));
        }
        [HttpGet("duethisweek")]
        [ProducesResponseType(typeof(PagedList<TaskToReturn>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTasksDueForCurrentWeek(int pageNumber = 1, int pageSize = 50)
        {
            return Ok(await _taskService.GetTasksDueForCurrentWeekAsync(pageNumber, pageSize));
        }
        [HttpPut("assignorremove")]
        [ProducesResponseType(typeof(PagedList<TaskToReturn>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AssignOrRemove(TaskAction payload)
        {
            await _taskService.AssignOrRemoveToProject(payload);
            return Ok();
        }
    }
}
