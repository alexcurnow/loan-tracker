using LoanTracker.Domain.Entities;
using LoanTracker.Domain.Enums;

namespace LoanTracker.Domain.Interfaces;

public interface ILoanRepository
{
    Task<Loan?> GetByIdAsync(Guid loanId);
    Task<IEnumerable<Loan>> GetAllAsync();
    Task<IEnumerable<Loan>> GetByStatusAsync(LoanStatus status);
    Task<Loan> CreateAsync(Loan loan);
    Task<Loan> UpdateAsync(Loan loan);
    Task DeleteAsync(Guid loanId);
}
