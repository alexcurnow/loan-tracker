namespace LoanTracker.Domain.ValueObjects;

/// <summary>
/// Value object representing an annual interest rate percentage
/// Provides type safety and calculation helpers
/// </summary>
public sealed record InterestRate
{
    public decimal AnnualPercentage { get; init; }

    private InterestRate(decimal annualPercentage)
    {
        if (annualPercentage < 0)
            throw new ArgumentException("Interest rate cannot be negative", nameof(annualPercentage));

        if (annualPercentage > 100)
            throw new ArgumentException("Interest rate cannot exceed 100%", nameof(annualPercentage));

        AnnualPercentage = annualPercentage;
    }

    public static InterestRate Zero => new(0);
    public static InterestRate FromPercentage(decimal percentage) => new(percentage);

    // Conversion helpers
    public decimal AsDecimal => AnnualPercentage / 100m;
    public decimal MonthlyPercentage => AnnualPercentage / 12m;
    public decimal MonthlyDecimal => MonthlyPercentage / 100m;

    // Calculation helpers
    public Money CalculateMonthlyInterest(Money principalBalance)
    {
        var monthlyInterest = principalBalance * MonthlyDecimal;
        return monthlyInterest.RoundToCents();
    }

    public Money CalculateAnnualInterest(Money principalBalance)
    {
        var annualInterest = principalBalance * AsDecimal;
        return annualInterest.RoundToCents();
    }

    // Comparison operators
    public static bool operator >(InterestRate left, InterestRate right)
        => left.AnnualPercentage > right.AnnualPercentage;

    public static bool operator <(InterestRate left, InterestRate right)
        => left.AnnualPercentage < right.AnnualPercentage;

    public static bool operator >=(InterestRate left, InterestRate right)
        => left.AnnualPercentage >= right.AnnualPercentage;

    public static bool operator <=(InterestRate left, InterestRate right)
        => left.AnnualPercentage <= right.AnnualPercentage;

    // Helper methods
    public bool IsZero => AnnualPercentage == 0;

    public override string ToString() => $"{AnnualPercentage:F2}%";
}
