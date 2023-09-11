using Hangfire;
using Microsoft.AspNetCore.Builder;
using Serilog;
using TaskManagementSystem;
using TaskManagementSystem.Middlewares;
using ILogger = Microsoft.Extensions.Logging.ILogger;

var builder = WebApplication.CreateBuilder(args);

IConfiguration config = builder.Configuration;

// Get log settings from appsettings
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .CreateLogger();
builder.Logging.ClearProviders();
// Add serilog
builder.Logging.AddSerilog(logger);
// Add services to the container.
builder.Services.AddTaskManagementSystemServices(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.EnsureDatabaseSetup();
}
app.UseExceptionHandler();
app.UseHangfireDashboard("/jobs");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
