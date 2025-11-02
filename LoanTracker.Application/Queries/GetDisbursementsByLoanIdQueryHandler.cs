using LoanTracker.Application.Interfaces;
using LoanTracker.Domain.Interfaces;

namespace LoanTracker.Application.Queries;

public class GetDisbursementsByLoanIdQueryHandler : IQueryHandler<GetDisbursementsByLoanIdQuery, IEnumerable<DisbursementDto>>
{
    private readonly IDisbursementQuery _disbursementQuery;
    private readonly IProjectRepository _projectRepository;

    public GetDisbursementsByLoanIdQueryHandler(
        IDisbursementQuery disbursementQuery,
        IProjectRepository projectRepository)
    {
        _disbursementQuery = disbursementQuery;
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<DisbursementDto>> HandleAsync(GetDisbursementsByLoanIdQuery query)
    {
        // Query the event-sourced projection!
        var disbursements = await _disbursementQuery.GetByLoanIdAsync(query.LoanId);

        if (!disbursements.Any())
        {
            return Enumerable.Empty<DisbursementDto>();
        }

        // Get project names for display (could be optimized with a projection later)
        var projectIds = disbursements.Select(d => d.ProjectId).Distinct();
        var projects = new Dictionary<Guid, string>();
        foreach (var projectId in projectIds)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project != null)
            {
                projects[projectId] = project.ProjectName;
            }
        }

        return disbursements.Select(d => new DisbursementDto
        {
            DisbursementId = d.DisbursementId,
            ProjectId = d.ProjectId,
            LoanId = query.LoanId,
            ProjectName = projects.GetValueOrDefault(d.ProjectId, "Unknown Project"),
            Amount = d.Amount,
            Currency = d.Currency,
            DisbursementDate = d.DisbursementDate,
            RecipientName = d.RecipientName,
            RecipientDetails = d.RecipientDetails,
            CreatedAt = d.RecordedAt,
            IsBackdated = d.IsBackdated
        });
    }
}
