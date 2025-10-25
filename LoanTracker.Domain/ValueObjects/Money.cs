namespace LoanTracker.Domain.ValueObjects;

/// <summary>
/// Value object representing a monetary amount with currency
/// Provides type safety and prevents primitive obsession
/// </summary>
public sealed record Money
{
    public decimal Amount { get; init; }
    public string Currency { get; init; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Zero => new(0, "USD");
    public static Money FromDecimal(decimal amount, string currency = "USD") => new(amount, currency);
    public static Money Dollars(decimal amount) => new(amount, "USD");

    // Arithmetic operators
    public static Money operator +(Money left, Money right)
    {
        EnsureSameCurrency(left, right);
        return new Money(left.Amount + right.Amount, left.Currency);
    }

    public static Money operator -(Money left, Money right)
    {
        EnsureSameCurrency(left, right);
        return new Money(left.Amount - right.Amount, left.Currency);
    }

    public static Money operator *(Money money, decimal multiplier)
    {
        return new Money(money.Amount * multiplier, money.Currency);
    }

    public static Money operator /(Money money, decimal divisor)
    {
        if (divisor == 0)
            throw new DivideByZeroException("Cannot divide money by zero");

        return new Money(money.Amount / divisor, money.Currency);
    }

    // Comparison operators
    public static bool operator >(Money left, Money right)
    {
        EnsureSameCurrency(left, right);
        return left.Amount > right.Amount;
    }

    public static bool operator <(Money left, Money right)
    {
        EnsureSameCurrency(left, right);
        return left.Amount < right.Amount;
    }

    public static bool operator >=(Money left, Money right)
    {
        EnsureSameCurrency(left, right);
        return left.Amount >= right.Amount;
    }

    public static bool operator <=(Money left, Money right)
    {
        EnsureSameCurrency(left, right);
        return left.Amount <= right.Amount;
    }

    // Helper methods
    public bool IsZero => Amount == 0;
    public bool IsPositive => Amount > 0;
    public bool IsNegative => Amount < 0;

    public Money Negate() => new(-Amount, Currency);
    public Money Abs() => new(Math.Abs(Amount), Currency);
    public Money RoundToCents() => new(Math.Round(Amount, 2), Currency);

    private static void EnsureSameCurrency(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException($"Cannot perform operation on different currencies: {left.Currency} and {right.Currency}");
    }

    public override string ToString() => $"${Amount:N2}";
    public string ToString(string format) => $"${Amount.ToString(format)}";
}
