using LoanTracker.Domain.Entities;
using LoanTracker.Domain.Interfaces;
using LoanTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LoanTracker.Infrastructure.Repositories;

public class BorrowerTypeRepository : IBorrowerTypeRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public BorrowerTypeRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<IEnumerable<BorrowerType>> GetAllAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.BorrowerTypes
            .OrderBy(bt => bt.TypeName)
            .ToListAsync();
    }
}
