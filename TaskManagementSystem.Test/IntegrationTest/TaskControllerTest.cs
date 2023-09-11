using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Core.DTOs;
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
    }
}
