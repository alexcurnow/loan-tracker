using LoanTracker.Domain.Entities;

namespace LoanTracker.Domain.Interfaces;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid projectId);
    Task<IEnumerable<Project>> GetByLoanIdAsync(Guid loanId);
    Task<Project> AddAsync(Project project);
    Task UpdateAsync(Project project);
}
