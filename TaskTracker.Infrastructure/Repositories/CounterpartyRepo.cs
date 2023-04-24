using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskTracker.Infrastructure.Repositories;

public class CounterpartyRepo : ICounterpartyRepo
{
    private readonly ApplicationDbContext _context;

    public CounterpartyRepo(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Add(Counterparty counterparty)
    {
        if (counterparty == null) throw new ArgumentNullException(nameof(counterparty));

        await _context.Counterparty.AddAsync(counterparty);
    }

    public async Task<IEnumerable<Counterparty>> GetAll()
    {
        return await _context.Counterparty.ToListAsync();
    }

    public async Task<Counterparty> GetById(int id)
    {
        return (await _context.Counterparty.SingleOrDefaultAsync(_ => _.Id == id))!;
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}
