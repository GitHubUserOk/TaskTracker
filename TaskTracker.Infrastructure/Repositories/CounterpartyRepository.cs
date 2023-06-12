using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskTracker.Infrastructure.Repositories;

public class CounterpartyRepository : ICounterpartyRepository
{
    private readonly ApplicationDbContext _context;

    public CounterpartyRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<Counterparty>> GetAll()
    {
        return await _context.Counterparties.AsNoTracking().ToListAsync();
    }
    public async Task<Counterparty> GetById(int id)
    {
        return (await _context.Counterparties.SingleOrDefaultAsync(_ => _.Id == id))!;
    }
    public async Task Add(Counterparty counterparty)
    {
        if (counterparty == null) throw new ArgumentNullException(nameof(counterparty));

        await _context.Counterparties.AddAsync(counterparty);
    }
}
