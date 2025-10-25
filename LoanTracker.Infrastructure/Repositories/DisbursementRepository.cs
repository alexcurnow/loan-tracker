using LoanTracker.Domain.Entities;
using LoanTracker.Domain.Interfaces;
using LoanTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LoanTracker.Infrastructure.Repositories;

public class DisbursementRepository : IDisbursementRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public DisbursementRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Disbursement> AddAsync(Disbursement disbursement)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.Disbursements.Add(disbursement);
        await context.SaveChangesAsync();
        return disbursement;
    }

    public async Task<IEnumerable<Disbursement>> GetByLoanIdAsync(Guid loanId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Disbursements
            .Include(d => d.Project)
            .Where(d => d.Project!.LoanId == loanId)
            .OrderBy(d => d.DisbursementDate)
            .ToListAsync();
    }

    public async Task<bool> HasDisbursementsAsync(Guid loanId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Disbursements
            .AnyAsync(d => d.Project!.LoanId == loanId);
    }
}
