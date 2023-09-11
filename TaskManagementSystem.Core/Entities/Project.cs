namespace TaskManagementSystem.Core.Entities
{
    public class Project
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Collection of tasks associated with this project
        public ICollection<Task> Tasks { get; set; }
    }
}
