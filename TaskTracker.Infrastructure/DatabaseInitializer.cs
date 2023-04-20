using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Domain.Entities;
using Task = TaskTracker.Domain.Entities.Task;

namespace TaskTracker.Infrastructure;

public static class DatabaseInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ApplicationDbContext>>()))
        {
            if (context.Counterparty.Any())
            {
               return;
            }

            var counterparty1 = new Counterparty()
            {
                Name = "Unit A"
            };

            context.Counterparty.Add(counterparty1);

            var project1 = new Project()
            {
                Counterparty= counterparty1,
                Name = "Поточна підтримка"
            };

            context.Project.Add(project1);

            var project2 = new Project()
            {
                Counterparty = counterparty1,
                Name = "Впровадження WMS"
            };

            context.Project.Add(project2);

            var user1 = new User()
            {
                Name = "Швидкий Олександр"
            };

            context.User.Add(user1);

            var task1 = new Task()
            {
                Author = user1,
                Counterparty = counterparty1,
                CreatedDate = DateTime.Now,
                Description = "Провести навчання співробітників складу з виконання кластерного відбору",
                Project = project2,
                Title = "Навчання співробітників"
            };

            context.Task.Add(task1);

            context.SaveChanges();
        }
    }
}
