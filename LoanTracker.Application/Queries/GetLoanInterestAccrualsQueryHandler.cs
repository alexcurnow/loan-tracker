using LoanTracker.Application.Interfaces;
using LoanTracker.Domain.Interfaces;
using LoanTracker.Domain.Services;

namespace LoanTracker.Application.Queries;

public class GetLoanInterestAccrualsQueryHandler
    : IQueryHandler<GetLoanInterestAccrualsQuery, IEnumerable<InterestAccrualDto>>
{
    private readonly ILoanRepository _loanRepository;
    private readonly IDisbursementRepository _disbursementRepository;
    private readonly InterestCalculationService _interestCalculationService;

    public GetLoanInterestAccrualsQueryHandler(
        ILoanRepository loanRepository,
        IDisbursementRepository disbursementRepository,
        InterestCalculationService interestCalculationService)
    {
        _loanRepository = loanRepository;
        _disbursementRepository = disbursementRepository;
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

        // 2. Get all disbursements for this loan
        var disbursements = await _disbursementRepository.GetByLoanIdAsync(query.LoanId);

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
