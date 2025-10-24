using Fluxor;
using LoanTracker.Application.Commands;
using LoanTracker.Application.Common;
using LoanTracker.Application.DTOs;
using LoanTracker.Application.Interfaces;
using LoanTracker.Application.Queries;

namespace LoanTracker.Web.Store.LoanState;

public class LoanEffects
{
    private readonly IQueryHandler<GetAllLoansQuery, IEnumerable<LoanDto>> _getAllLoansHandler;
    private readonly IQueryHandler<GetLoanByIdQuery, LoanDto?> _getLoanByIdHandler;
    private readonly ICommandHandler<CreateLoanCommand, Result<LoanDto>> _createLoanHandler;
    private readonly ICommandHandler<TransitionLoanStatusCommand, Result<LoanDto>> _transitionStatusHandler;

    public LoanEffects(
        IQueryHandler<GetAllLoansQuery, IEnumerable<LoanDto>> getAllLoansHandler,
        IQueryHandler<GetLoanByIdQuery, LoanDto?> getLoanByIdHandler,
        ICommandHandler<CreateLoanCommand, Result<LoanDto>> createLoanHandler,
        ICommandHandler<TransitionLoanStatusCommand, Result<LoanDto>> transitionStatusHandler)
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

        var result = await _createLoanHandler.HandleAsync(command);

        if (result.IsSuccess)
        {
            dispatcher.Dispatch(new CreateLoanSuccessAction(result.Value!));
        }
        else
        {
            dispatcher.Dispatch(new ErrorState.ShowErrorAction(result.Error!));
        }
    }

    [EffectMethod]
    public async Task HandleTransitionLoanStatusAction(TransitionLoanStatusAction action, IDispatcher dispatcher)
    {
        var command = new TransitionLoanStatusCommand(
            action.LoanId,
            action.ToStatus,
            action.ReviewerNotes
        );

        var result = await _transitionStatusHandler.HandleAsync(command);

        if (result.IsSuccess)
        {
            dispatcher.Dispatch(new TransitionLoanStatusSuccessAction(result.Value!));
        }
        else
        {
            dispatcher.Dispatch(new ErrorState.ShowErrorAction(result.Error!));
        }
    }
}
