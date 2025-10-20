using LoanTracker.Domain.Entities;
using LoanTracker.Domain.Enums;
using LoanTracker.Domain.Interfaces;
using LoanTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LoanTracker.Infrastructure.Repositories;

public class LoanRepository : ILoanRepository
{
    private readonly ApplicationDbContext _context;

    public LoanRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Loan?> GetByIdAsync(Guid loanId)
    {
        return await _context.Loans
            .Include(l => l.BorrowerType)
            .FirstOrDefaultAsync(l => l.LoanId == loanId);
    }

    public async Task<IEnumerable<Loan>> GetAllAsync()
    {
        return await _context.Loans
            .Include(l => l.BorrowerType)
            .OrderByDescending(l => l.ApplicationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetByStatusAsync(LoanStatus status)
    {
        return await _context.Loans
            .Include(l => l.BorrowerType)
            .Where(l => l.Status == status)
            .OrderByDescending(l => l.ApplicationDate)
            .ToListAsync();
    }

    public async Task<Loan> CreateAsync(Loan loan)
    {
        loan.CreatedAt = DateTime.UtcNow;
        loan.UpdatedAt = DateTime.UtcNow;

        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(loan.LoanId) ?? loan;
    }

    public async Task<Loan> UpdateAsync(Loan loan)
    {
        loan.UpdatedAt = DateTime.UtcNow;

        _context.Loans.Update(loan);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(loan.LoanId) ?? loan;
    }

    public async Task DeleteAsync(Guid loanId)
    {
        var loan = await _context.Loans.FindAsync(loanId);
        if (loan != null)
        {
            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
        }
    }
}
