﻿using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Core.Enums;
using TaskManagementSystem.Core.Exceptions;
using TaskManagementSystem.Core.Interfaces.Infrastructure;
using TaskManagementSystem.Core.Models;
using TaskManagementSystem.Infrastructure.Data.Context;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Infrastructure.Repositories
{
    public class TaskRepository : GenericRepository<Core.Entities.Task>, ITaskRepository
    {
        public TaskRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PagedList<TaskToReturn>> GetTasksByStatusOrPriorityAsync(int status, int priority, int pageNumber, int pageSize)
        {
            
            IQueryable<Core.Entities.Task> query = _context.Tasks;

            if (Enum.TryParse(status.ToString(), true, out Status parsedStatus))
            {
                query = query.Where(t => t.Status == parsedStatus);
            }

            if (Enum.TryParse(priority.ToString(), true, out Priority parsedPriority))
            {
                query = query.Where(t => t.Priority == parsedPriority);
            }

            var count = await query.CountAsync();

            // map Task to TaskToReturn 
            var tasks = await query
                         .Include(x => x.UserCreated)
                         .GroupJoin(
                             query.Include(x => x.Project),
                             task => task.ProjectId,
                             project => project.ProjectId,
                             (task, projectGroup) => new { task, projectGroup }
                         )
                         .SelectMany(
                             x => x.projectGroup.DefaultIfEmpty(),
                             (x, project) => new TaskToReturn
                             {
                                 Id = x.task.TaskId,
                                 Status = x.task.Status,
                                 Priority = x.task.Priority,
                                 Description = x.task.Description,
                                 DueDate = x.task.DueDate,
                                 ProjectId = project != null ? project.ProjectId : Guid.Empty,
                                 ProjectName = project != null ? project.Title : string.Empty,
                                 Created = x.task.UserCreated.Name,
                                 Title = x.task.Title,
                                 UserId = x.task.CreatedId,
                             }
                         )
                         .Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize)
                         .AsNoTracking()
                         .ToListAsync();


            return new PagedList<TaskToReturn>(tasks, count, pageNumber, pageSize);
        }

        public async Task<PagedList<TaskToReturn>> GetTasksDueForCurrentWeekAsync(int pageNumber, int pageSize)
        {
            var query = _context.Tasks;

            var count = await query.CountAsync();
            // Calculate the start and end dates of the current week
            DateTime currentDate = DateTime.Now;
            DateTime startOfWeek = currentDate.Date.AddDays(-(int)currentDate.DayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            // map Task to TaskToReturn 
            var tasks = await query
                .Include(x => x.UserCreated)
                .Include(x => x.Project)
                .Where(t => t.DueDate >= startOfWeek && t.DueDate <= endOfWeek)
                .Select(s => new TaskToReturn
                {
                    Id = s.TaskId,
                    Status = s.Status,
                    Priority = s.Priority,
                    Description = s.Description,
                    DueDate = s.DueDate,
                    ProjectId = s.ProjectId,
                    ProjectName = s.Project.Name,
                    Created = s.UserCreated.Name,
                    Title = s.Title,
                    UserId = s.CreatedId,
                }).Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PagedList<TaskToReturn>(tasks, count, pageNumber, pageSize);
        }
    }
}
