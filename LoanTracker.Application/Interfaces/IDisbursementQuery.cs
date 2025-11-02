namespace LoanTracker.Application.Interfaces;

/// <summary>
/// Query interface for reading disbursement projections
/// Implemented in Infrastructure using Marten
/// </summary>
public interface IDisbursementQuery
{
    Task<IEnumerable<DisbursementReadModel>> GetByLoanIdAsync(Guid loanId);
}

public record DisbursementReadModel(
    Guid DisbursementId,
    Guid ProjectId,
    decimal Amount,
    string Currency,
    DateTime DisbursementDate,
    string RecipientName,
    string RecipientDetails,
    DateTime RecordedAt,
    bool IsBackdated
);
