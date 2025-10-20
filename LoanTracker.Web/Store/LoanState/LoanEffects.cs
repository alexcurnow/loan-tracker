using Fluxor;
using LoanTracker.Application.Commands;
using LoanTracker.Application.DTOs;
using LoanTracker.Application.Interfaces;
using LoanTracker.Application.Queries;

namespace LoanTracker.Web.Store.LoanState;

public class LoanEffects
{
    private readonly IQueryHandler<GetAllLoansQuery, IEnumerable<LoanDto>> _getAllLoansHandler;
    private readonly IQueryHandler<GetLoanByIdQuery, LoanDto?> _getLoanByIdHandler;
    private readonly ICommandHandler<CreateLoanCommand, LoanDto> _createLoanHandler;
    private readonly ICommandHandler<TransitionLoanStatusCommand, LoanDto> _transitionStatusHandler;

    public LoanEffects(
        IQueryHandler<GetAllLoansQuery, IEnumerable<LoanDto>> getAllLoansHandler,
        IQueryHandler<GetLoanByIdQuery, LoanDto?> getLoanByIdHandler,
        ICommandHandler<CreateLoanCommand, LoanDto> createLoanHandler,
        ICommandHandler<TransitionLoanStatusCommand, LoanDto> transitionStatusHandler)
    {
        _getAllLoansHandler = getAllLoansHandler;
        _getLoanByIdHandler = getLoanByIdHandler;
        _createLoanHandler = createLoanHandler;
        _transitionStatusHandler = transitionStatusHandler;
    }

    [EffectMethod]
    public async Task HandleLoadLoansAction(LoadLoansAction action, IDispatcher dispatcher)
    {
        try
        {
            var loans = await _getAllLoansHandler.HandleAsync(new GetAllLoansQuery());
            dispatcher.Dispatch(new LoadLoansSuccessAction(loans));
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new LoadLoansFailureAction(ex.Message));
        }
    }

    [EffectMethod]
    public async Task HandleLoadLoanByIdAction(LoadLoanByIdAction action, IDispatcher dispatcher)
    {
        try
        {
            var loan = await _getLoanByIdHandler.HandleAsync(new GetLoanByIdQuery(action.LoanId));
            if (loan != null)
            {
                dispatcher.Dispatch(new LoadLoanByIdSuccessAction(loan));
            }
            else
            {
                dispatcher.Dispatch(new LoadLoanByIdFailureAction("Loan not found"));
            }
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new LoadLoanByIdFailureAction(ex.Message));
        }
    }

    [EffectMethod]
    public async Task HandleCreateLoanAction(CreateLoanAction action, IDispatcher dispatcher)
    {
        try
        {
            var command = new CreateLoanCommand(
                action.BorrowerName,
                action.BorrowerTypeId,
                action.ContactPerson,
                action.ContactEmail,
                action.Amount,
                action.InterestRate,
                action.TermYears,
                action.Purpose
            );

            var loan = await _createLoanHandler.HandleAsync(command);
            dispatcher.Dispatch(new CreateLoanSuccessAction(loan));
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new CreateLoanFailureAction(ex.Message));
        }
    }

    [EffectMethod]
    public async Task HandleTransitionLoanStatusAction(TransitionLoanStatusAction action, IDispatcher dispatcher)
    {
        try
        {
            var command = new TransitionLoanStatusCommand(
                action.LoanId,
                action.ToStatus,
                action.ReviewerNotes
            );

            var loan = await _transitionStatusHandler.HandleAsync(command);
            dispatcher.Dispatch(new TransitionLoanStatusSuccessAction(loan));
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new TransitionLoanStatusFailureAction(ex.Message));
        }
    }
}
