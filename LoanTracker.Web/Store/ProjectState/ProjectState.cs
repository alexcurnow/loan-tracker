namespace LoanTracker.Web.Store.ProjectState;

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

public record ProjectState
{
    public bool IsLoading { get; init; }
    public IEnumerable<ProjectDto> Projects { get; init; } = Array.Empty<ProjectDto>();
    public IEnumerable<DisbursementDto> Disbursements { get; init; } = Array.Empty<DisbursementDto>();
    public IEnumerable<InterestAccrualDto> InterestAccruals { get; init; } = Array.Empty<InterestAccrualDto>();
    public string? ErrorMessage { get; init; }
}
