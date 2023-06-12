using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskTracker.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Project project)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));

            await _context.Projects.AddAsync(project);
        }

        public async Task<IEnumerable<Project>> GetAll()
        {
            //return await _context.Projects.AsNoTracking().Include(_ => _.Counterparty).ToListAsync();
            return await _context.Projects.AsNoTracking().ToListAsync();
        }

        public async Task<Project> GetById(int id)
        {
            //return (await _context.Projects.Include(_ => _.Counterparty).SingleOrDefaultAsync(_ => _.Id == id))!;
            return (await _context.Projects.SingleOrDefaultAsync(_ => _.Id == id))!;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
