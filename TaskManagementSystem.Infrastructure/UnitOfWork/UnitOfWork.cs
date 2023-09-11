using TaskManagementSystem.Core.Interfaces.Infrastructure;
using TaskManagementSystem.Infrastructure.Data.Context;
using TaskManagementSystem.Infrastructure.Repositories;

namespace TaskManagementSystem.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private Lazy<ITaskRepository> _task;
        private Lazy<IUserRepository> _user;
        private Lazy<INotificationRepository> _notification;
        private Lazy<IProjectRepository> _project;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _task = new Lazy<ITaskRepository>(() => new TaskRepository(context));
            _notification = new Lazy<INotificationRepository>(() => new NotificationRepository(context));
            _user = new Lazy<IUserRepository>(() => new UserRepository(context));
            _project = new Lazy<IProjectRepository>(() => new ProjectRepository(context));
        }

        public ITaskRepository Task => _task.Value;
        public IUserRepository User => _user.Value;
        public INotificationRepository Notification => _notification.Value;
        public IProjectRepository Project => _project.Value;
        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
