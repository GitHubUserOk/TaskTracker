﻿using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Infrastructure;
public class ApplicationDbContext : DbContext
{
    public DbSet<Counterparty> Counterparty => Set<Counterparty>();
    public DbSet<Project> Project => Set<Project>();
    public DbSet<Domain.Entities.Task> Task => base.Set<Domain.Entities.Task>();
    public DbSet<User> User => Set<User>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TaskTracker;Trusted_Connection=True;");
    }
}
