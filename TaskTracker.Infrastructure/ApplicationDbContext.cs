using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Infrastructure;
public class ApplicationDbContext : DbContext
{
    public DbSet<Counterparty> Counterparties => Set<Counterparty>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Domain.Entities.Task> Tasks => Set<Domain.Entities.Task>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Performer> Performers => Set<Performer>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TaskTracker;Trusted_Connection=True;");
        optionsBuilder.EnableSensitiveDataLogging();
    }
}
