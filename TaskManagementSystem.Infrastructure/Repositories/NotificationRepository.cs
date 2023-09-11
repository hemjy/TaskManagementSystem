using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Core.Interfaces.Infrastructure;
using TaskManagementSystem.Infrastructure.Data.Context;

namespace TaskManagementSystem.Infrastructure.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(AppDbContext context) : base(context)
        {
        }
    }
}
