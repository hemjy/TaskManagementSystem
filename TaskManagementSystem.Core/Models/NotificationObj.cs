using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Core.Models
{
    public class NotificationObj
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}
