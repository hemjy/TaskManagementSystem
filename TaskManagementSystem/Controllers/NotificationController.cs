using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Core.Interfaces.Core;
using TaskManagementSystem.Core.Models;
using TaskManagementSystem.Core.Services;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPut("mark/{notificationId}")]
        [ProducesResponseType(typeof(PagedList<TaskToReturn>), StatusCodes.Status200OK)]
        public async Task<IActionResult> MarK([FromRoute]Guid notificationId, [FromQuery] bool IsRead, [FromQuery] Guid? userId)
        {
            await _notificationService.Mark(IsRead, notificationId, userId ?? Guid.Empty);
            return Ok();
        }
    }
}
