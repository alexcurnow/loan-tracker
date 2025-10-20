using LoanTracker.Domain.Entities;
using LoanTracker.Domain.Interfaces;
using LoanTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LoanTracker.Infrastructure.Repositories;

public class BorrowerTypeRepository : IBorrowerTypeRepository
{
    private readonly ApplicationDbContext _context;

    public BorrowerTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BorrowerType>> GetAllAsync()
    {
        return await _context.BorrowerTypes
            .OrderBy(bt => bt.TypeName)
            .ToListAsync();
    }
}
