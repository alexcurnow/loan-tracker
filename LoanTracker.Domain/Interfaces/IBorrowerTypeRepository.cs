using LoanTracker.Domain.Entities;

namespace LoanTracker.Domain.Interfaces;

public interface IBorrowerTypeRepository
{
    Task<IEnumerable<BorrowerType>> GetAllAsync();
}
