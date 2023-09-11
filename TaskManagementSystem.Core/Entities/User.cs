
namespace TaskManagementSystem.Core.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // Collection of tasks created by this user
        public ICollection<Task> TasksCreated { get; set; }

        // Collection of notifications received by this user
        public ICollection<Notification> NotificationsReceived { get; set; }
    }
}
