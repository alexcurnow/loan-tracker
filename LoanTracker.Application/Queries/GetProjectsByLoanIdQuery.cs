namespace LoanTracker.Application.Queries;

public record GetProjectsByLoanIdQuery
{
    public Guid LoanId { get; init; }
}

public record ProjectDto
{
    public Guid ProjectId { get; init; }
    public Guid LoanId { get; init; }
    public string ProjectName { get; init; } = string.Empty;
    public decimal Budget { get; init; }
    public string Currency { get; init; } = "USD";
    public string Description { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
