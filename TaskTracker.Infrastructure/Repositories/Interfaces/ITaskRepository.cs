using Task = System.Threading.Tasks.Task;

namespace TaskTracker.Infrastructure.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task SaveChanges();
        Task<IEnumerable<TaskTracker.Domain.Entities.Task>> GetAll();
        Task<TaskTracker.Domain.Entities.Task> GetById(int id);
        Task Add(TaskTracker.Domain.Entities.Task task);
    }
}
