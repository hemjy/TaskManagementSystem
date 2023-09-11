using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Core.Enums;
using TaskManagementSystem.Core.Exceptions;
using TaskManagementSystem.Core.Interfaces.Core;
using TaskManagementSystem.Core.Interfaces.Infrastructure;
using TaskManagementSystem.Core.Interfaces.Infrastructure.ExternalServices;
using TaskManagementSystem.Core.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Core.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBackgroundJobService _backgroundJobService;
        private readonly ILogger<TaskService> _logger;
        public TaskService(IUnitOfWork unitOfWork, 
            IBackgroundJobService backgroundJobService,
            ILogger<TaskService> logger)
        {
            _unitOfWork = unitOfWork;
            _backgroundJobService = backgroundJobService;
            _logger = logger;   
        }
        public async Task<PagedList<TaskToReturn>> GetTasksByStatusOrPriorityAsync(int status, int priorty, int pageNumber, int pageSize)
        {
            var result = await _unitOfWork.Task.GetTasksByStatusOrPriorityAsync(status, priorty, pageNumber, pageSize);
            return result;
        }

        public async Task<PagedList<TaskToReturn>> GetTasksDueForCurrentWeekAsync(int pageNumber, int pageSize)
        {
            var result = await _unitOfWork.Task.GetTasksDueForCurrentWeekAsync(pageNumber, pageSize);
            return result;
        }

        public async Task AssignOrRemoveToProject(TaskAction taskAction)
        {
            var project = await _unitOfWork.Project.GetAsync(p => p.ProjectId == taskAction.ProjectId);
            if (project == null)
            {
                throw new BadRequestException("Invalid Project Id");
            }
            var task = await _unitOfWork.Task.GetAsync(x => x.TaskId == taskAction.TaskId, true);
            if (task == null)
            {
                throw new BadRequestException("Invalid Task Id");
            }
            Enum.TryParse(taskAction.TaskState.ToString(), true, out Assignment parsedStatus);
            if (parsedStatus == Assignment.Assign)
            {
                task.ProjectId = taskAction.ProjectId;
            }
            else if (parsedStatus == Assignment.Remove)
            {
                task.ProjectId = null;
            }
            await _unitOfWork.SaveAsync();
        }

        public async Task AssignToUser(Guid userId, Guid taskId)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(userId));
            ArgumentException.ThrowIfNullOrEmpty(nameof(taskId));
            var task = await _unitOfWork.Task.GetAsync(p => p.TaskId == taskId, true);
            var user = await _unitOfWork.User.GetAsync(p => p.UserId == userId);
            if (task == null)
            {
                throw new NotFoundException("Task not exist");
            }
            if (user == null)
            {
                throw new NotFoundException("User not exist");
            }
            if (user.UserId == userId)
            {
                throw new BadRequestException("User already assigned");
            }
            task.CreatedId = userId;
            await _unitOfWork.SaveAsync();

            var obj = new NotificationObj()
            {
                Sender = "admin",
                Body = $"A new task with the below details has been assigned to you \n id: {task.TaskId} \n Description: {task.Description}.",
                Recipient = task.UserCreated.Email,
                Subject = "Task Completion"
            };
            _backgroundJobService.EnqueueNotificationJob(obj);
        }
        public async Task UpdateStatus(int status, Guid taskId)
        {
            // Validate input parameters
            if (taskId == Guid.Empty)
            {
                throw new BadRequestException("Task Id is required");
            }

            if (!Enum.TryParse(status.ToString(), true, out Status parsedStatus))
            {
                throw new BadRequestException("Invalid status");
            }

            // Fetch the task with related user information
            var includes = new List<Expression<Func<Entities.Task, object>>>
    {
        t => t.UserCreated
    };

            var task = await _unitOfWork.Task.GetAsync(x => x.TaskId == taskId, true, includes);

            if (task == null)
            {
                throw new NotFoundException("Task not found");
            }

            // Update the task status
            task.Status = parsedStatus;

            // Handle status-specific actions
            if (parsedStatus == Status.Completed)
            {
                // Create a notification
                var notificationMessage = $"Your task with the following details has been completed:\n" +
                                           $"ID: {task.TaskId}\n" +
                                           $"Description: {task.Description}";

                var notification = new Notification
                {
                    IsRead = false,
                    Timestamp = DateTime.Now,
                    Type = NotificationType.StatusUpdate,
                    Message = notificationMessage,
                    TaskId = task.TaskId,
                    UserId = task.CreatedId
                };

                // Add the notification to the database
                await _unitOfWork.Notification.AddAsync(notification);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("Add the notification {0} to the database", notification.NotificationId);

                // Enqueue a notification job
                var notificationObj = new NotificationObj
                {
                    Sender = "admin",
                    Body = notificationMessage,
                    Recipient = task.UserCreated.Email,
                    Subject = "Task Completion"
                };
                _logger.LogInformation("notification is enqueue to send");
                _backgroundJobService.EnqueueNotificationJob(notificationObj);
            }
            else
            {
                // Save changes to the database
                await _unitOfWork.SaveAsync();
            }
        }


    }
}
