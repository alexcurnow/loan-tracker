using LoanTracker.Application.Interfaces;
using LoanTracker.Domain.Entities;
using LoanTracker.Domain.Interfaces;
using LoanTracker.Domain.Services;
using LoanTracker.Domain.ValueObjects;

namespace LoanTracker.Application.Queries;

public class GetLoanInterestAccrualsQueryHandler
    : IQueryHandler<GetLoanInterestAccrualsQuery, IEnumerable<InterestAccrualDto>>
{
    private readonly ILoanRepository _loanRepository;
    private readonly IDisbursementQuery _disbursementQuery;
    private readonly InterestCalculationService _interestCalculationService;

    public GetLoanInterestAccrualsQueryHandler(
        ILoanRepository loanRepository,
        IDisbursementQuery disbursementQuery,
        InterestCalculationService interestCalculationService)
    {
        _loanRepository = loanRepository;
        _disbursementQuery = disbursementQuery;
        _interestCalculationService = interestCalculationService;
    }

    public async Task<IEnumerable<InterestAccrualDto>> HandleAsync(GetLoanInterestAccrualsQuery query)
    {
        // 1. Get loan
        var loan = await _loanRepository.GetByIdAsync(query.LoanId);
        if (loan == null)
        {
            return Enumerable.Empty<InterestAccrualDto>();
        }

        // 2. Get all disbursements from event-sourced projection
        var disbursementReadModels = await _disbursementQuery.GetByLoanIdAsync(query.LoanId);

        // Convert read models to domain entities for the calculation service
        var disbursements = disbursementReadModels.Select(d => new Disbursement
        {
            DisbursementId = d.DisbursementId,
            ProjectId = d.ProjectId,
            AmountValue = d.Amount,
            AmountCurrency = d.Currency,
            DisbursementDate = d.DisbursementDate,
            RecipientName = d.RecipientName,
            RecipientDetails = d.RecipientDetails,
            CreatedAt = d.RecordedAt
        });

        // 3. Calculate accruals using domain service
        var accruals = _interestCalculationService.CalculateAccruals(
            loan,
            disbursements,
            query.AsOfDate
        );

        // 4. Map to DTOs
        return accruals.Select(a => new InterestAccrualDto
        {
            AccrualDate = a.AccrualDate,
            DueDate = a.DueDate,
            PrincipalBalance = a.PrincipalBalance.Amount,
            InterestAmount = a.InterestAmount.Amount,
            InterestRate = a.Rate.AnnualPercentage,
            Period = a.Period,
            Currency = a.PrincipalBalance.Currency
        });
    }
}
