using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Infrastructure;
public static class DatabaseInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ApplicationDbContext>>()))
        {
            if (context.Counterparties.Any())
            {
               return;
            }

            var counterparty1 = new Counterparty()
            {
                Name = "Unit A"
            };

            context.Counterparties.Add(counterparty1);

            var project1 = new Project()
            {
                Counterparty = counterparty1,
                Name = "Поточна підтримка"
            };

            context.Projects.Add(project1);

            var project2 = new Project()
            {
                Counterparty = counterparty1,
                Name = "Впровадження WMS"
            };

            context.Projects.Add(project2);

            var user1 = new User()
            {
                Name = "Швидкий Олександр"
            };

            context.Users.Add(user1);

            context.SaveChanges();

            var task1 = new Domain.Entities.Task()
            {
                AuthorId = user1.Id,
                CounterpartyId = counterparty1.Id,
                CreatedDate = DateTime.UtcNow,
                Description = "Провести навчання співробітників складу з виконання кластерного відбору",
                ProjectId = project2.Id,
                Title = "Навчання співробітників"
            };

            context.Tasks.Add(task1);

            context.SaveChanges();
        }
    }
}
