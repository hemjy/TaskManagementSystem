using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Core.Models
{
    public class TaskAction
    {
        public Guid TaskId { get; set; }
        public Guid ProjectId { get; set; }
        public int TaskState { get; set; }
    }
}
