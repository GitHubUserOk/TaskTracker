using TaskTracker.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskTracker.Infrastructure.Repositories.Interfaces;

public interface IProjectRepository
{
    Task SaveChanges();
    Task<IEnumerable<Project>> GetAll();
    Task<Project> GetById(int id);
    Task Add(Project project);
}
