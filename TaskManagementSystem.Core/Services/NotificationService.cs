using System.Linq.Expressions;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Core.Exceptions;
using TaskManagementSystem.Core.Interfaces.Core;
using TaskManagementSystem.Core.Interfaces.Infrastructure;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _uow;
        public NotificationService(IUnitOfWork uow) 
        {
            _uow = uow;
        }

        public async Task Mark(bool IsRead, Guid notificationId, Guid readerId)
        {
            var includes = new List<Expression<Func<Notification, object>>>
            {
                n => n.User
            };
            var notification = await _uow.Notification.GetAsync(n => n.NotificationId == notificationId, true, includes);
            if (notification == null)
            {
                throw new NotFoundException("notificationId doesn't exist");
            }
            bool isOwner = notification.UserId == readerId;
            if (IsRead && !isOwner)
            {
                throw new BadRequestException("This notification doesn't belong to you");
            }
            notification.IsRead = IsRead;
            await _uow.SaveAsync();
        }
    }
}
