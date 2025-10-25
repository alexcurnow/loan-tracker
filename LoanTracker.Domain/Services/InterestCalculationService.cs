using LoanTracker.Domain.Entities;
using LoanTracker.Domain.ValueObjects;

namespace LoanTracker.Domain.Services;

/// <summary>
/// Domain service for calculating interest accruals on loans
/// Handles:
/// - Monthly interest calculations based on principal balance
/// - Temporal ordering (disbursements affect balance on their disbursement date)
/// - Accrual generation timing (15 days before due day)
/// </summary>
public class InterestCalculationService
{
    /// <summary>
    /// Calculate all interest accruals for a loan from approval date to present
    /// </summary>
    /// <param name="loan">The loan to calculate accruals for</param>
    /// <param name="disbursements">All disbursements for this loan (across all projects)</param>
    /// <param name="asOfDate">Calculate accruals up to this date (default: today)</param>
    public List<InterestAccrual> CalculateAccruals(
        Loan loan,
        IEnumerable<Disbursement> disbursements,
        DateTime? asOfDate = null)
    {
        var calculationDate = asOfDate ?? DateTime.UtcNow;
        var accruals = new List<InterestAccrual>();

        if (!loan.DecisionDate.HasValue)
        {
            // No approval date = no accruals
            return accruals;
        }

        // Sort disbursements by disbursement date (temporal ordering!)
        var sortedDisbursements = disbursements
            .OrderBy(d => d.DisbursementDate)
            .ToList();

        // Start from the first month after approval
        var currentDate = loan.DecisionDate.Value.Date;
        var firstAccrualDate = GetNextAccrualDate(currentDate, loan.DueDay);

        // Track principal balance as we move forward in time
        Money currentBalance = Money.Zero;
        int disbursementIndex = 0;

        // Generate monthly accruals
        var accrualDate = firstAccrualDate;
        while (accrualDate <= calculationDate)
        {
            // Apply any disbursements that occurred before this accrual date
            while (disbursementIndex < sortedDisbursements.Count &&
                   sortedDisbursements[disbursementIndex].DisbursementDate < accrualDate)
            {
                currentBalance = currentBalance + sortedDisbursements[disbursementIndex].Amount;
                disbursementIndex++;
            }

            // Only generate accrual if we have a balance
            if (currentBalance.IsPositive)
            {
                var dueDate = GetDueDate(accrualDate, loan.DueDay);
                var accrual = InterestAccrual.Create(
                    accrualDate: accrualDate,
                    dueDate: dueDate,
                    principalBalance: currentBalance,
                    rate: loan.Rate
                );

                accruals.Add(accrual);
            }

            // Move to next month
            accrualDate = GetNextAccrualDate(accrualDate, loan.DueDay);
        }

        return accruals;
    }

    /// <summary>
    /// Get the next accrual generation date
    /// Accruals generate 15 days before the due day
    /// </summary>
    private DateTime GetNextAccrualDate(DateTime fromDate, int dueDay)
    {
        var accrualDay = dueDay - 15;
        if (accrualDay <= 0)
        {
            accrualDay += 30; // Wrap to previous month
        }

        // Start from the next month
        var nextMonth = fromDate.AddMonths(1);
        var year = nextMonth.Year;
        var month = nextMonth.Month;

        // Handle edge case where accrual day exceeds days in month
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var actualDay = Math.Min(accrualDay, daysInMonth);

        return new DateTime(year, month, actualDay, 0, 0, 0, DateTimeKind.Utc);
    }

    /// <summary>
    /// Get the due date for an accrual (same month as accrual, on the due day)
    /// </summary>
    private DateTime GetDueDate(DateTime accrualDate, int dueDay)
    {
        var year = accrualDate.Year;
        var month = accrualDate.Month;

        // If accrual is after due day, due date is next month
        if (accrualDate.Day > dueDay)
        {
            var nextMonth = accrualDate.AddMonths(1);
            year = nextMonth.Year;
            month = nextMonth.Month;
        }

        var daysInMonth = DateTime.DaysInMonth(year, month);
        var actualDay = Math.Min(dueDay, daysInMonth);

        return new DateTime(year, month, actualDay, 0, 0, 0, DateTimeKind.Utc);
    }

    /// <summary>
    /// Calculate total interest accrued to date
    /// </summary>
    public Money CalculateTotalInterestAccrued(
        Loan loan,
        IEnumerable<Disbursement> disbursements,
        DateTime? asOfDate = null)
    {
        var accruals = CalculateAccruals(loan, disbursements, asOfDate);
        return accruals.Aggregate(Money.Zero, (total, accrual) => total + accrual.InterestAmount);
    }
}
