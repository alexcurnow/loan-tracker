namespace LoanTracker.Application.Queries;

public record GetDisbursementsByLoanIdQuery
{
    public Guid LoanId { get; init; }
}

public record DisbursementDto
{
    public Guid DisbursementId { get; init; }
    public Guid ProjectId { get; init; }
    public Guid LoanId { get; init; }
    public string ProjectName { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string Currency { get; init; } = "USD";
    public DateTime DisbursementDate { get; init; }
    public string RecipientName { get; init; } = string.Empty;
    public string RecipientDetails { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public bool IsBackdated { get; init; }
}
