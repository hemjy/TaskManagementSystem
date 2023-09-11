using Hangfire;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Core.Interfaces.Core;
using TaskManagementSystem.Core.Interfaces.Infrastructure;
using TaskManagementSystem.Core.Interfaces.Infrastructure.ExternalServices;
using TaskManagementSystem.Core.Services;
using TaskManagementSystem.Infrastructure.Data;
using TaskManagementSystem.Infrastructure.Data.Context;
using TaskManagementSystem.Infrastructure.ExternalServices;
using TaskManagementSystem.Infrastructure.Repositories;
using TaskManagementSystem.Infrastructure.UnitOfWork;

namespace TaskManagementSystem
{
    public static class ConfigurationExtensions
    {
        public static void  AddScheduleJobs(this IServiceCollection services)
        {
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var jobService = serviceProvider.GetRequiredService<IBackgroundJobService>();
                jobService.ScheduleTaskDailyRemaiderJob();
            }
        }
        public static void AddTaskManagementSystemServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("TaskManagementSystem"));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("TaskManagementSystem")));
            services.AddHangfireServer();

            services.AddScoped<IBackgroundJobService, HangfireBackgroundJobService>();

            services.AddScheduleJobs();


        }

        public static void EnsureDatabaseSetup(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var db = services.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
            DataSeeder.Seed(db);
        }
    }
}
