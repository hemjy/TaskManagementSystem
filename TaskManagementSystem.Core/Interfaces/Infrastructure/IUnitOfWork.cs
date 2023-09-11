using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Core.Interfaces.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IProjectRepository Project { get; }
        ITaskRepository Task { get; }
        INotificationRepository Notification { get; }
        IUserRepository User { get; }
        Task<int> SaveAsync();

    }
}
