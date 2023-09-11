using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Infrastructure.Data.Context;
using TaskManagementSystem.Test.Helper;
using TaskManagementSystem.Test.IntegrationTest.Base;

namespace TaskManagementSystem.Test.IntegrationTest
{
    public class NotificationControllerTest : TMSBaseTest<TaskController>
    {
        private readonly AppDbContext _context;
        public NotificationControllerTest()
        {

            _context = _application.Services.CreateScope().ServiceProvider
                        .GetRequiredService<AppDbContext>(); 
             TestDataGenerator.PopulateDb(_context);
            
        }

        [Fact]
        public async Task Mark_NotificationMarkedAsRead_ReturnsOk()
        {
            // Arrange
            var item = TestDataGenerator.Notifications().First();
           
            // Create and send the HTTP request to your API
            var response = await _client.PutAsync($"/api/notification/mark/{item.NotificationId}?IsRead={true}&userId={item.UserId}", null);

            var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.NotificationId == item.NotificationId);
            // Assert
            Assert.True(notification != null);
            Assert.True(notification.IsRead);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task Mark_NotificationMarkedAsRead_ReturnsBadRequest()
        {
            // Arrange
            var item = TestDataGenerator.Notifications().First();

            // Create and send the HTTP request to your API
            var response = await _client.PutAsync($"/api/notification/mark/{item.NotificationId}?IsRead={true}", null);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [Fact]
        public async Task Mark_NotificationMarkedAsUnRead_ReturnsOk()
        {
            // Arrange
            var item = TestDataGenerator.Notifications().First();

            // Create and send the HTTP request to your API
            var response = await _client.PutAsync($"/api/notification/mark/{item.NotificationId}?IsRead={false}&userId={item.UserId}", null);

            var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.NotificationId == item.NotificationId);
            // Assert
            Assert.True(notification != null);
            Assert.False(notification.IsRead);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
    }
}
