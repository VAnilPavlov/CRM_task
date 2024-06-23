using CRMsystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CRMsystem
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationContext context)
        {
            if (context.employees.Any() && context.tasks.Any())
            {
                return;
            }

            var employeesList = new List<Employee>()
            {
                new Employee{Id = Guid.NewGuid(), FullName = "Kak", Title = "Pr"},
                new Employee{Id = Guid.NewGuid(), FullName = "Ran", Title = "pt"},
                new Employee{Id = Guid.NewGuid(), FullName = "Lan", Title = "xs"}
            };

            context.employees.AddRange(employeesList);

            var tasksList = new List<Models.Task>()
            {
                new Models.Task {
                    Id = Guid.NewGuid(),
                    EmployeeId = employeesList[0].Id,
                    Title = "Разработка ТЗ",
                    Description="Рассписать требования, технологии и т.д.",
                    StartDate = new DateTime(2024, 6, 20, 14, 30, 0),
                    EndDate = new DateTime(2024, 7, 20, 14, 30, 0),
                    Percent = 20 },
                new Models.Task {
                    Id = Guid.NewGuid(),
                    EmployeeId = employeesList[2].Id,
                    Title = "Разработка системы",
                    Description="Рассписать требования, технологии и т.д.",
                    StartDate = new DateTime(2024, 5, 20, 14, 0, 0),
                    EndDate = new DateTime(2024, 8, 10, 14, 0, 0),
                    Percent = 50 },
                new Models.Task {
                    Id = Guid.NewGuid(),
                    EmployeeId = employeesList[0].Id,
                    Title = "Резюме совещания км 1",
                    Description="Рассписать требования, технологии и т.д.",
                    StartDate = new DateTime(2024, 6, 1, 14, 0, 0),
                    EndDate = new DateTime(2024, 6, 25, 14, 0, 0),
                    Percent = 60 },
            };

            context.tasks.AddRange(tasksList);
            context.SaveChanges();
        }
    }
}
