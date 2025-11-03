using LoanTracker.Domain.Enums;
using LoanTracker.Domain.Types;

namespace LoanTracker.Application.DTOs;

public class LoanDto
{
    public Guid LoanId { get; set; }
    public string BorrowerName { get; set; } = string.Empty;
    public int BorrowerTypeId { get; set; }
    public string BorrowerTypeName { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal InterestRate { get; set; }
    public TermYears TermYears { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public DateTime ApplicationDate { get; set; }
    public LoanStatus Status { get; set; }
    public DateTime? DecisionDate { get; set; }
    public string? ReviewerNotes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
