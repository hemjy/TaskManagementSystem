using Moq;
using System.Linq.Expressions;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Core.Exceptions;
using TaskManagementSystem.Core.Interfaces.Core;
using TaskManagementSystem.Core.Interfaces.Infrastructure;
using TaskManagementSystem.Core.Services;
using TaskManagementSystem.Test.Helper;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Test.UnitTests
{
    public class NotificationServiceTest
    {
        private readonly INotificationService _sut;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public NotificationServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _sut = new NotificationService(_mockUnitOfWork.Object);
        }
        [Fact]
        public async Task Mark_NotificationDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var notificationId = Guid.NewGuid();
            var readerId = Guid.NewGuid();
            var includes = new List<Expression<Func<Notification, object>>>
                                {
                                    n => n.User
                                };

            _mockUnitOfWork
                .Setup(uow => uow.Notification.GetAsync(
                    It.IsAny<Expression<Func<Notification, bool>>>(),
                    true,
                    includes
                ))
                .ReturnsAsync((Notification)null); 

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _sut.Mark(true, notificationId, readerId));
            _mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Never); 
        }

      
    }
}
