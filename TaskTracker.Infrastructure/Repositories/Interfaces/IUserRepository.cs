using TaskTracker.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskTracker.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task SaveChanges();
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task Add(User user);
    }
}
