using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Entities;

namespace TaskManagementSystem.Core.Interfaces.Infrastructure
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
    }
}
