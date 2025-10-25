namespace LoanTracker.Application.Queries;

public record GetLoanInterestAccrualsQuery
{
    public Guid LoanId { get; init; }
    public DateTime? AsOfDate { get; init; }
}

public record InterestAccrualDto
{
    public DateTime AccrualDate { get; init; }
    public DateTime DueDate { get; init; }
    public decimal PrincipalBalance { get; init; }
    public decimal InterestAmount { get; init; }
    public decimal InterestRate { get; init; }
    public string Period { get; init; } = string.Empty;
    public string Currency { get; init; } = "USD";
}
