using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Core.Enums;
using TaskManagementSystem.Core.Models;
using TaskManagementSystem.Infrastructure.Data.Context;
using TaskManagementSystem.Test.Helper;
using TaskManagementSystem.Test.IntegrationTest.Base;

namespace TaskManagementSystem.Test.IntegrationTest
{
    public class TaskControllerTest : TMSBaseTest<TaskController>
    {
        private readonly AppDbContext _context;
        public TaskControllerTest()
        {
            _context = _application.Services.CreateScope().ServiceProvider
                        .GetRequiredService<AppDbContext>();
            TestDataGenerator.PopulateDb(_context);
        }

        [Theory]
        [InlineData(5, 5)]
        public async Task GetTasksByStatusOrPriority_ShouldReturnEmpty_WhenPassedInvalidEnum(int status, int priority)
        {
            var tasks = TestDataGenerator.Tasks();
            await _context.Tasks.AddRangeAsync(tasks);
            await _context.SaveChangesAsync();

            var response = await _client
               .GetAsync($"/api/tasks/statusorpriorty?status={status}&priority={priority}");
            var content = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var pagedTasks = JsonConvert.DeserializeObject<PagedList<TaskToReturn>>(content);
            pagedTasks.MetaData.TotalCount.Should().Be(0);
            pagedTasks.Data.Should().HaveCount(0); 
        }

        [Fact]
        public async Task AssignOrRemoveTaskToProject_ShouldReturnOk_WhenAssign()
        {
            // Arrange
            var payload = new TaskAction
            {
                TaskId = _context.Tasks.First().TaskId,
                ProjectId = _context.Projects.First().ProjectId,
                TaskState = (int) Assignment.Assign
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/tasks/assignorremoveproject", content);

            var task = _context.Tasks.Include(p => p.Project).FirstOrDefault(t => t.TaskId == payload.TaskId);
            // Assert
            task.Should().NotBeNull();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AssignOrRemoveTaskToProject_ShouldReturnOk_WhenRemove()
        {
            // Arrange
            var payload = new TaskAction
            {
                TaskId = _context.Tasks.First().TaskId,
                ProjectId = _context.Projects.First().ProjectId,
                TaskState = (int)Assignment.Assign
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/tasks/assignorremoveproject", content);
            var task = _context.Tasks.FirstOrDefault(t => t.TaskId == payload.TaskId);
            // Assert
            task.Should().NotBeNull();
            task.ProjectId.Should().BeNull();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
