using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using TaskManagementSystem.Infrastructure.Data.Context;

namespace TaskManagementSystem.Test.Infrastructure
{
    public class TaskManagementSystemApplication<T> : WebApplicationFactory<T> where T : class
    {
        private readonly string _dbName = Guid.NewGuid().ToString();
      
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing DbContext registration if it exists.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>)
                );
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Register the DbContext with an in-memory database.
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_dbName); // Use the unique database name
                });

                // Other services configuration for testing
            });

            // Apply migrations to the in-memory database when the application is being built.
            //builder.ConfigureServices(services =>
            //{
            //    using (var scope = services.BuildServiceProvider().CreateScope())
            //    {
            //        var serviceProvider = scope.ServiceProvider;
            //        var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            //        dbContext.Database.Migrate();
            //    }
            //});
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
