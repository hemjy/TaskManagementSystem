using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Enums;

namespace TaskManagementSystem.Core.DTOs
{
    public class NotificationToReturn
    {
        public int NotificationId { get; set; }
        public NotificationType Type { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string TaskTitle { get; set; } 
        public string UserName { get; set; }
    }
}
