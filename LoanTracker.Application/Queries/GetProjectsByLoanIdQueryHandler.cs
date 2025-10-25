using LoanTracker.Application.Interfaces;
using LoanTracker.Domain.Interfaces;

namespace LoanTracker.Application.Queries;

public class GetProjectsByLoanIdQueryHandler : IQueryHandler<GetProjectsByLoanIdQuery, IEnumerable<ProjectDto>>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectsByLoanIdQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<ProjectDto>> HandleAsync(GetProjectsByLoanIdQuery query)
    {
        var projects = await _projectRepository.GetByLoanIdAsync(query.LoanId);

        return projects.Select(p => new ProjectDto
        {
            ProjectId = p.ProjectId,
            LoanId = p.LoanId,
            ProjectName = p.ProjectName,
            Budget = p.BudgetAmount,
            Currency = p.BudgetCurrency,
            Description = p.Description,
            CreatedAt = p.CreatedAt
        });
    }
}
