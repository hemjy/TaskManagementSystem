using Hangfire;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Core.Enums;
using TaskManagementSystem.Core.Interfaces.Infrastructure;
using TaskManagementSystem.Core.Interfaces.Infrastructure.ExternalServices;
using TaskManagementSystem.Core.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Infrastructure.ExternalServices
{
    public class HangfireBackgroundJobService : IBackgroundJobService
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        public HangfireBackgroundJobService(
            IBackgroundJobClient backgroundJobClient, 
            IEmailService emailService,
            IUnitOfWork unitOfWork
            )
        {
            _backgroundJobClient = backgroundJobClient;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }
        public void EnqueueNotificationJob(NotificationObj notification)
        {
           
            _backgroundJobClient.Enqueue(() => SendNotificationJob(notification));
        }

        public async Task  ScheduleTaskDailyRemaiderJob()
        {
            RecurringJob.AddOrUpdate(
           "TaskDailyRemainderJob", 
           () => SendDailyRemainder().GetAwaiter(), 
           Cron.Daily);
        }

        private async Task SendDailyRemainder()
        {
            var includes = new List<Expression<Func<Core.Entities.Task, object>>>
            {
                t => t.UserCreated
            };
            var tasks = await  _unitOfWork.Task.GetAll(t => t.DueDate <= DateTime.Now.AddHours(48), includes: includes).ToListAsync();

            var notifications = new List<Notification>();
            foreach (var task in tasks)
            {
                NotificationObj notification = new()
                {
                    Sender ="Admin",
                    Recipient= task.UserCreated.Email,
                    Subject = "Remainder",
                    Body = $"Your task with the below details will due on: {task.DueDate.ToLongDateString()}  \n id: {task.TaskId} \n Description: {task.Description}",
                };
                notifications.Add(new() 
                { 
                    IsRead = false, 
                    TaskId = task.TaskId, 
                    Timestamp = DateTime.Now, 
                    UserId = task.CreatedId, 
                    Type = NotificationType.DueDateReminder,
                    Message = notification.Body
                });
                _backgroundJobClient.Enqueue(() => SendNotificationJob(notification));
            }
           await _unitOfWork.Notification.AddRangeAsync(notifications);
            await _unitOfWork.SaveAsync();
        }

        private void SendNotificationJob(NotificationObj notification)
        {
            _emailService.Send(notification.Sender, notification.Recipient, notification.Subject, notification.Body);
           
        }
    }
}
