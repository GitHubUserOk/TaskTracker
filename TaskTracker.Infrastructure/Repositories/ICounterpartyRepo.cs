
using TaskTracker.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskTracker.Infrastructure.Repositories;

public interface ICounterpartyRepo
{
    Task SaveChanges();
    Task<IEnumerable<Counterparty>> GetAll();
    Task<Counterparty> GetById(int id);
    Task Add(Counterparty counterparty);
}
