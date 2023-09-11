using TaskManagementSystem.Core.Enums;

namespace TaskManagementSystem.Core.Entities
{
    public class Task
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }

        // Foreign key for the associated project
        public Guid? ProjectId { get; set; }
        public Project Project { get; set; }

        // Foreign key for the user who created the task
        public Guid CreatedId { get; set; }
        public User UserCreated { get; set; }

        // Collection of notifications related to this task
        public ICollection<Notification> Notifications { get; set; }
    }

}
