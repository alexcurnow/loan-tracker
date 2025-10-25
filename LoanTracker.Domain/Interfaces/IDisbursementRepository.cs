using LoanTracker.Domain.Entities;

namespace LoanTracker.Domain.Interfaces;

public interface IDisbursementRepository
{
    Task<Disbursement> AddAsync(Disbursement disbursement);
    Task<IEnumerable<Disbursement>> GetByLoanIdAsync(Guid loanId);
    Task<bool> HasDisbursementsAsync(Guid loanId);
}
