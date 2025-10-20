namespace LoanTracker.Domain.Entities;

public class BorrowerType
{
    public int BorrowerTypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation property
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
