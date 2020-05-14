using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TaskManager.Domain;
using TaskManager.Data;
using Microsoft.EntityFrameworkCore;

namespace TaskManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            RunMigrations(host);

            FillDatabaseWithValues(host);  

            host.Run();
        }

        public static void RunMigrations(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TaskManagerContext>();

                context.Database.Migrate();
            }
        }

        public static void FillDatabaseWithValues(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IWorkItemRepository>();

                if (repo.GetWorkItems().Result.Count() == 0)
                {
                    var wi1 = WorkItem.Create(new WorkItemSnapshot()
                    {
                        Title = "Система управления задачами",
                        Description = "Разработка системы управления задачами",
                        Executors = "Игорь Кокорин",
                        Status = WorkItemStatus.ASSIGNED,
                        PlannedExecutionTime = WorkItemExecutionTime.FromTimeSpan(TimeSpan.FromHours(5)).ExecutionTimeInSeconds,
                        ActualExecutionTime = WorkItemExecutionTime.FromTimeSpan(TimeSpan.FromHours(5)).ExecutionTimeInSeconds,
                        CreatedAt = DateTime.Now
                    });

                    var wi2 = WorkItem.Create(new WorkItemSnapshot()
                    {
                        Title = "Бизнес-логика",
                        Description = "Разработка бизнес-логики проекта управления задачами",
                        Executors = "Игорь Кокорин",
                        Status = WorkItemStatus.ASSIGNED,
                        PlannedExecutionTime = WorkItemExecutionTime.FromTimeSpan(TimeSpan.FromHours(5)).ExecutionTimeInSeconds,
                        ActualExecutionTime = WorkItemExecutionTime.FromTimeSpan(TimeSpan.FromHours(5)).ExecutionTimeInSeconds,
                        CreatedAt = DateTime.Now
                    });

                    wi2.SetParent(wi1);

                    var wi3 = WorkItem.Create(new WorkItemSnapshot()
                    {
                        Title = "Изменение статуса задач",
                        Description = "Реализовать возможность изменения статуса задач в соответсвии с требованиями",
                        Executors = "Игорь Кокорин",
                        Status = WorkItemStatus.ASSIGNED,
                        PlannedExecutionTime = WorkItemExecutionTime.FromTimeSpan(TimeSpan.FromHours(5)).ExecutionTimeInSeconds,
                        ActualExecutionTime = WorkItemExecutionTime.FromTimeSpan(TimeSpan.FromHours(5)).ExecutionTimeInSeconds,
                        CreatedAt = DateTime.Now
                    });

                    wi3.SetParent(wi2);

                    var wi4 = WorkItem.Create(new WorkItemSnapshot()
                    {
                        Title = "Рассчет времени выполнения здачи",
                        Description = "Реализовать возможность вычислять время выполнения задачи в соответсвии с требованиями",
                        Executors = "Игорь Кокорин",
                        Status = WorkItemStatus.ASSIGNED,
                        PlannedExecutionTime = WorkItemExecutionTime.FromTimeSpan(TimeSpan.FromHours(5)).ExecutionTimeInSeconds,
                        ActualExecutionTime = WorkItemExecutionTime.FromTimeSpan(TimeSpan.FromHours(5)).ExecutionTimeInSeconds,
                        CreatedAt = DateTime.Now
                    });

                    wi4.SetParent(wi2);

                    var wi5 = WorkItem.Create(new WorkItemSnapshot()
                    {
                        Title = "Управление деревом подзадач",
                        Description = "Корректно отрабатывать изменения в структуре дерева задач",
                        Executors = "Игорь Кокорин",
                        Status = WorkItemStatus.ASSIGNED,
                        PlannedExecutionTime = WorkItemExecutionTime.FromTimeSpan(TimeSpan.FromHours(5)).ExecutionTimeInSeconds,
                        ActualExecutionTime = WorkItemExecutionTime.FromTimeSpan(TimeSpan.FromHours(5)).ExecutionTimeInSeconds,
                        CreatedAt = DateTime.Now
                    });

                    wi5.SetParent(wi2);

                    var wi6 = WorkItem.Create(new WorkItemSnapshot()
                    {
                        Title = "Перевод задачи в статус Звершена",
                        Description = "Переводить все подзадачи данной задачи в статус Завершена",
                        Executors = "Игорь Кокорин",
                        Status = WorkItemStatus.ASSIGNED,
                        PlannedExecutionTime = WorkItemExecutionTime.FromTimeSpan(TimeSpan.FromHours(5)).ExecutionTimeInSeconds,
                        ActualExecutionTime = WorkItemExecutionTime.FromTimeSpan(TimeSpan.FromHours(5)).ExecutionTimeInSeconds,
                        CreatedAt = DateTime.Now
                    });

                    wi6.SetParent(wi2);

                    var wi7 = WorkItem.Create(new WorkItemSnapshot()
                    {
                        Title = "Хранение данных",
                        Description = "Возможность сохранять задачи в базе данных Microsoft SQL Server",
                        Executors = "Игорь Кокорин",
                        Status = WorkItemStatus.ASSIGNED,
                        PlannedExecutionTime = WorkItemExecutionTime.FromTimeSpan(TimeSpan.FromHours(5)).ExecutionTimeInSeconds,
                        ActualExecutionTime = WorkItemExecutionTime.FromTimeSpan(TimeSpan.FromHours(5)).ExecutionTimeInSeconds,
                        CreatedAt = DateTime.Now
                    });

                    wi7.SetParent(wi1);

                    var wi8 = WorkItem.Create(new WorkItemSnapshot()
                    {
                        Title = "Пользовательский интерфейс",
                        Description = "Разработка пользовательского интерфейса в соответствии с требованиями",
                        Executors = "Игорь Кокорин",
                        Status = WorkItemStatus.ASSIGNED,
                        PlannedExecutionTime = WorkItemExecutionTime.FromTimeSpan(TimeSpan.FromHours(5)).ExecutionTimeInSeconds,
                        ActualExecutionTime = WorkItemExecutionTime.FromTimeSpan(TimeSpan.FromHours(5)).ExecutionTimeInSeconds,
                        CreatedAt = DateTime.Now
                    });

                    wi8.SetParent(wi1);

                    repo.CreateWorkItem(wi1).Wait();
                }
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
