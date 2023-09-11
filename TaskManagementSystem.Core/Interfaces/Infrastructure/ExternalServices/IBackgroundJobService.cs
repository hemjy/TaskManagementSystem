using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Models;

namespace TaskManagementSystem.Core.Interfaces.Infrastructure.ExternalServices
{
    public interface IBackgroundJobService
    {
        void EnqueueNotificationJob(NotificationObj notification);
        Task ScheduleTaskDailyRemaiderJob();
    }
}
