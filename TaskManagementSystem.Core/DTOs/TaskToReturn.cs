using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Core.Enums;

namespace TaskManagementSystem.Core.DTOs
{
    public class TaskToReturn
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid UserId { get; set; }
        public string ProjectName { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }
    }
}
