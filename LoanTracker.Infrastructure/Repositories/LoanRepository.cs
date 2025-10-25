using LoanTracker.Domain.Entities;
using LoanTracker.Domain.Enums;
using LoanTracker.Domain.Interfaces;
using LoanTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LoanTracker.Infrastructure.Repositories;

public class LoanRepository : ILoanRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public LoanRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Loan?> GetByIdAsync(Guid loanId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Loans
            .Include(l => l.BorrowerType)
            .FirstOrDefaultAsync(l => l.LoanId == loanId);
    }

    public async Task<IEnumerable<Loan>> GetAllAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Loans
            .Include(l => l.BorrowerType)
            .OrderByDescending(l => l.ApplicationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetByStatusAsync(LoanStatus status)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Loans
            .Include(l => l.BorrowerType)
            .Where(l => l.Status == status)
            .OrderByDescending(l => l.ApplicationDate)
            .ToListAsync();
    }

    public async Task<Loan> CreateAsync(Loan loan)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        loan.CreatedAt = DateTime.UtcNow;
        loan.UpdatedAt = DateTime.UtcNow;

        context.Loans.Add(loan);
        await context.SaveChangesAsync();

        return await GetByIdAsync(loan.LoanId) ?? loan;
    }

    public async Task<Loan> UpdateAsync(Loan loan)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        loan.UpdatedAt = DateTime.UtcNow;

        context.Loans.Update(loan);
        await context.SaveChangesAsync();

        return await GetByIdAsync(loan.LoanId) ?? loan;
    }

    public async Task DeleteAsync(Guid loanId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var loan = await context.Loans.FindAsync(loanId);
        if (loan != null)
        {
            context.Loans.Remove(loan);
            await context.SaveChangesAsync();
        }
    }
}
