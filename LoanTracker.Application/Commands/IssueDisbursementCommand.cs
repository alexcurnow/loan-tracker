using LoanTracker.Application.Common;

namespace LoanTracker.Application.Commands;

/// <summary>
/// Command to issue a disbursement to a project
/// This triggers:
/// - Creation of immutable Disbursement record
/// - DisbursementIssued event
/// - Automatic loan status transition (Approved → Construction on first disbursement)
/// - LoanStatusChanged event if status changes
/// </summary>
public record IssueDisbursementCommand
{
    public Guid ProjectId { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; } = "USD";
    public DateTime DisbursementDate { get; init; }
    public string RecipientName { get; init; } = string.Empty;
    public string RecipientDetails { get; init; } = string.Empty;
    public string IssuedBy { get; init; } = "System";
}

/// <summary>
/// DTO returned when disbursement is successfully issued
/// </summary>
public record DisbursementDto
{
    public Guid DisbursementId { get; init; }
    public Guid ProjectId { get; init; }
    public Guid LoanId { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public DateTime DisbursementDate { get; init; }
    public string RecipientName { get; init; } = string.Empty;
    public string RecipientDetails { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public bool IsBackdated { get; init; }
}
