﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Core.Enums;
using TaskManagementSystem.Infrastructure.Data.Context;
using Task = TaskManagementSystem.Core.Entities.Task;


namespace TaskManagementSystem.Test.Helper
{
    public class TestDataGenerator
    {
        public static Task[] Tasks()
        {
            return new[]
            {
            new Task
            {
                TaskId = new Guid("58798030-8F41-4A34-5ECF-08DBB21C49C8"),
                Title = "Task 1",
                Description = "Description of Task 1",
                DueDate = DateTime.Now.AddDays(5),
                Priority = Priority.High,
                Status = Status.Pending,
                CreatedId = new Guid("70e98e9e-3256-4a1e-9b45-1c4a7e35e440"),

            },
             new Task
            {
                TaskId = new Guid("018BDFDA-3547-42CA-5ED0-08DBB21C49C8"),
                Title = "Task 2",
                Description = "Description of Task 2",
                DueDate = DateTime.Now.AddDays(5),
                Priority = Priority.Low,
                Status = Status.Pending,
                CreatedId = new Guid("70e98e9e-3256-4a1e-9b45-1c4a7e35e440"),

            },
              new Task
            {
                TaskId = new Guid("2A9DD064-012D-4A1A-5ED1-08DBB21C49C8"),
                Title = "Task 2",
                Description = "Description of Task 2",
                DueDate = DateTime.Now.AddDays(2),
                Priority = Priority.Low,
                Status = Status.Pending,
                CreatedId = new Guid("6a3e7b8d-2c27-4f59-93b0-8ac0c52e6d68"),

            },
                new Task
            {
                TaskId= new Guid("DA2A0AFE-0989-4563-5ED2-08DBB21C49C8"),
                Title = "Task 4",
                Description = "Description of Task 4",
                DueDate = DateTime.Now,
                Priority = Priority.Low,
                Status = Status.Pending,
                CreatedId = new Guid("6a3e7b8d-2c27-4f59-93b0-8ac0c52e6d68"),

            },
                  new Task
            {
                TaskId= new Guid("58D15982-67A3-4174-5ED3-08DBB21C49C8"),
                Title = "Task 5",
                Description = "Description of Task 5",
                DueDate = DateTime.Now.AddDays(1),
                Priority = Priority.Low,
                Status = Status.Pending,
                CreatedId = new Guid("6a3e7b8d-2c27-4f59-93b0-8ac0c52e6d68"),

            },
            // Add more tasks as needed
        };

           
        }

        public static List<Project> Projects()
        {
            return new List<Project>()
            {
            new Project
            {
                ProjectId = new Guid("6DDDEA07-A5FC-46B5-47A6-08DBB21C49D9"),
                Name = "Project 1",
                Description = "Description of Project 1"
            },
                new Project
            {
                ProjectId = new Guid("CC06410E-76BB-4AF1-47A7-08DBB21C49D9"),
                Name = "Project 2",
                Description = "Description of Project 2"
            },     new Project
            {
                ProjectId = new Guid("A2B84975-FCB3-4014-47A8-08DBB21C49D9"),
                Name = "Project 3",
                Description = "Description of Project 3"
            }
        };
        }

        public static List<User> Users()
        {
            return new List<User>()
            {
            new User
            {
                Name = "User 1",
                Email = "user1@example.com",
                UserId = new Guid("6a3e7b8d-2c27-4f59-93b0-8ac0c52e6d68")

            },
            new User
            {
                Name = "User 2",
                Email = "user2@example.com",
                UserId = new Guid("70e98e9e-3256-4a1e-9b45-1c4a7e35e440")

            },
            new User
            {
                Name = "User 3",
                Email = "user3@example.com",
                UserId = new Guid("8ffab4fc-7c8e-481a-a076-9f6d87d0f9f8")

            }
        };

        }

        public static List<Notification> Notifications()
        {
            return new List<Notification>()
            {
            new Notification
            {
                UserId = new Guid("6a3e7b8d-2c27-4f59-93b0-8ac0c52e6d68"),
                NotificationId = new Guid("70e98e9e-3256-4a1e-9b45-1c4a7e35e440"),
                TaskId = new Guid("8ffab4fc-7c8e-481a-a076-9f6d87d0f9f8"),
                Timestamp = DateTime.Now,
                Message = "Testing",
                Type    = NotificationType.StatusUpdate
                

            }
        };

        }

        public static void PopulateDb(AppDbContext context)
        {
            if (!context.Tasks.Any())
            {
                context.Tasks.AddRange(Tasks());
            }
            if (!context.Projects.Any())
            {
                context.Projects.AddRange(Projects());
            }
            if (!context.Users.Any())
            {
                context.Users.AddRange(Users());
            }
            if (!context.Notifications.Any())
            {
                context.Notifications.AddRange(Notifications());
            }

            context.SaveChanges();
        }
    }
}
