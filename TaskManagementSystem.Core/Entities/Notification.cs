using TaskManagementSystem.Core.Enums;

namespace TaskManagementSystem.Core.Entities
{
    public class Notification
    {
        public Guid NotificationId { get; set; }
        public NotificationType Type { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }

        // Foreign key for the associated task
        public Guid TaskId { get; set; }
        public Task Task { get; set; }

        // Foreign key for the user who should receive the notification
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
