namespace LoanTracker.Domain.ValueObjects;

/// <summary>
/// Represents a single month's interest accrual for a loan
/// Immutable calculation result
/// </summary>
public sealed record InterestAccrual
{
    public DateTime AccrualDate { get; init; }
    public DateTime DueDate { get; init; }
    public Money PrincipalBalance { get; init; } = Money.Zero;
    public Money InterestAmount { get; init; } = Money.Zero;
    public InterestRate Rate { get; init; } = InterestRate.Zero;
    public string Period { get; init; } = string.Empty; // e.g., "Jan 2024"

    public static InterestAccrual Create(
        DateTime accrualDate,
        DateTime dueDate,
        Money principalBalance,
        InterestRate rate)
    {
        var interestAmount = rate.CalculateMonthlyInterest(principalBalance);
        var period = dueDate.ToString("MMM yyyy");

        return new InterestAccrual
        {
            AccrualDate = accrualDate,
            DueDate = dueDate,
            PrincipalBalance = principalBalance,
            InterestAmount = interestAmount,
            Rate = rate,
            Period = period
        };
    }
}
