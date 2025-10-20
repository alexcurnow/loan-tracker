using LoanTracker.Application.Interfaces;
using LoanTracker.Domain.Entities;
using LoanTracker.Domain.Interfaces;

namespace LoanTracker.Application.Queries;

public record GetAllBorrowerTypesQuery();

public class GetAllBorrowerTypesQueryHandler : IQueryHandler<GetAllBorrowerTypesQuery, IEnumerable<BorrowerType>>
{
    private readonly IBorrowerTypeRepository _borrowerTypeRepository;

    public GetAllBorrowerTypesQueryHandler(IBorrowerTypeRepository borrowerTypeRepository)
    {
        _borrowerTypeRepository = borrowerTypeRepository;
    }

    public async Task<IEnumerable<BorrowerType>> HandleAsync(GetAllBorrowerTypesQuery query)
    {
        return await _borrowerTypeRepository.GetAllAsync();
    }
}
