namespace TaskManagementSystem.Core.Interfaces.Core
{
    public interface INotificationService
    {
        Task Mark(bool IsRead, Guid notificationId, Guid readerId);
    }
}
