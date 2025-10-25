using Fluxor;
using LoanTracker.Application.Commands;
using LoanTracker.Application.Interfaces;
using LoanTracker.Application.Queries;
using MudBlazor;

namespace LoanTracker.Web.Store.ProjectState;

public class ProjectEffects
{
    private readonly IQueryHandler<GetProjectsByLoanIdQuery, IEnumerable<LoanTracker.Application.Queries.ProjectDto>> _getProjectsHandler;
    private readonly IQueryHandler<GetDisbursementsByLoanIdQuery, IEnumerable<LoanTracker.Application.Queries.DisbursementDto>> _getDisbursementsHandler;
    private readonly IQueryHandler<GetLoanInterestAccrualsQuery, IEnumerable<LoanTracker.Application.Queries.InterestAccrualDto>> _getInterestAccrualsHandler;
    private readonly ICommandHandler<IssueDisbursementCommand, LoanTracker.Application.Common.Result<LoanTracker.Application.Commands.DisbursementDto>> _issueDisbursementHandler;
    private readonly ISnackbar _snackbar;

    public ProjectEffects(
        IQueryHandler<GetProjectsByLoanIdQuery, IEnumerable<LoanTracker.Application.Queries.ProjectDto>> getProjectsHandler,
        IQueryHandler<GetDisbursementsByLoanIdQuery, IEnumerable<LoanTracker.Application.Queries.DisbursementDto>> getDisbursementsHandler,
        IQueryHandler<GetLoanInterestAccrualsQuery, IEnumerable<LoanTracker.Application.Queries.InterestAccrualDto>> getInterestAccrualsHandler,
        ICommandHandler<IssueDisbursementCommand, LoanTracker.Application.Common.Result<LoanTracker.Application.Commands.DisbursementDto>> issueDisbursementHandler,
        ISnackbar snackbar)
    {
        _getProjectsHandler = getProjectsHandler;
        _getDisbursementsHandler = getDisbursementsHandler;
        _getInterestAccrualsHandler = getInterestAccrualsHandler;
        _issueDisbursementHandler = issueDisbursementHandler;
        _snackbar = snackbar;
    }

    [EffectMethod]
    public async Task HandleLoadProjectsForLoanAction(LoadProjectsForLoanAction action, IDispatcher dispatcher)
    {
        try
        {
            var query = new GetProjectsByLoanIdQuery { LoanId = action.LoanId };
            var projects = await _getProjectsHandler.HandleAsync(query);

            // Map from Application.Queries.ProjectDto to Web.Store.ProjectState.ProjectDto
            var webProjects = projects.Select(p => new ProjectDto
            {
                ProjectId = p.ProjectId,
                LoanId = p.LoanId,
                ProjectName = p.ProjectName,
                Budget = p.Budget,
                Currency = p.Currency,
                Description = p.Description,
                CreatedAt = p.CreatedAt
            });

            dispatcher.Dispatch(new LoadProjectsForLoanSuccessAction(webProjects));
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new LoadProjectsForLoanFailureAction(ex.Message));
        }
    }

    [EffectMethod]
    public async Task HandleLoadDisbursementsForLoanAction(LoadDisbursementsForLoanAction action, IDispatcher dispatcher)
    {
        try
        {
            var query = new GetDisbursementsByLoanIdQuery { LoanId = action.LoanId };
            var disbursements = await _getDisbursementsHandler.HandleAsync(query);

            // Map from Application.Queries.DisbursementDto to Web.Store.ProjectState.DisbursementDto
            var webDisbursements = disbursements.Select(d => new DisbursementDto
            {
                DisbursementId = d.DisbursementId,
                ProjectId = d.ProjectId,
                LoanId = d.LoanId,
                ProjectName = d.ProjectName,
                Amount = d.Amount,
                Currency = d.Currency,
                DisbursementDate = d.DisbursementDate,
                RecipientName = d.RecipientName,
                RecipientDetails = d.RecipientDetails,
                CreatedAt = d.CreatedAt,
                IsBackdated = d.IsBackdated
            });

            dispatcher.Dispatch(new LoadDisbursementsForLoanSuccessAction(webDisbursements));
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new LoadDisbursementsForLoanFailureAction(ex.Message));
        }
    }

    [EffectMethod]
    public async Task HandleLoadInterestAccrualsForLoanAction(LoadInterestAccrualsForLoanAction action, IDispatcher dispatcher)
    {
        try
        {
            var query = new GetLoanInterestAccrualsQuery { LoanId = action.LoanId };
            var accruals = await _getInterestAccrualsHandler.HandleAsync(query);

            // Map from Application.Queries.InterestAccrualDto to Web.Store.ProjectState.InterestAccrualDto
            var webAccruals = accruals.Select(a => new InterestAccrualDto
            {
                AccrualDate = a.AccrualDate,
                DueDate = a.DueDate,
                PrincipalBalance = a.PrincipalBalance,
                InterestAmount = a.InterestAmount,
                InterestRate = a.InterestRate,
                Period = a.Period,
                Currency = a.Currency
            });

            dispatcher.Dispatch(new LoadInterestAccrualsForLoanSuccessAction(webAccruals));
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new LoadInterestAccrualsForLoanFailureAction(ex.Message));
        }
    }

    [EffectMethod]
    public async Task HandleIssueDisbursementAction(IssueDisbursementAction action, IDispatcher dispatcher)
    {
        try
        {
            var command = new IssueDisbursementCommand
            {
                ProjectId = action.ProjectId,
                Amount = action.Amount,
                Currency = action.Currency,
                DisbursementDate = action.DisbursementDate,
                RecipientName = action.RecipientName,
                RecipientDetails = action.RecipientDetails,
                IssuedBy = "User" // TODO: Get from auth context
            };

            var result = await _issueDisbursementHandler.HandleAsync(command);

            if (result.IsSuccess)
            {
                var disbursementDto = new DisbursementDto
                {
                    DisbursementId = result.Value!.DisbursementId,
                    ProjectId = result.Value.ProjectId,
                    LoanId = result.Value.LoanId,
                    ProjectName = "", // Will be updated on reload
                    Amount = result.Value.Amount,
                    Currency = result.Value.Currency,
                    DisbursementDate = result.Value.DisbursementDate,
                    RecipientName = result.Value.RecipientName,
                    RecipientDetails = result.Value.RecipientDetails,
                    CreatedAt = result.Value.CreatedAt,
                    IsBackdated = result.Value.IsBackdated
                };

                dispatcher.Dispatch(new IssueDisbursementSuccessAction(disbursementDto));
                _snackbar.Add($"Disbursement of ${action.Amount:N2} recorded successfully", Severity.Success);

                // Reload data to refresh everything (including interest calculations and loan status)
                dispatcher.Dispatch(new LoadDisbursementsForLoanAction(result.Value.LoanId));
                dispatcher.Dispatch(new LoadInterestAccrualsForLoanAction(result.Value.LoanId));
            }
            else
            {
                dispatcher.Dispatch(new IssueDisbursementFailureAction(result.Error!.Message));
                _snackbar.Add($"Failed to record disbursement: {result.Error.Message}", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new IssueDisbursementFailureAction(ex.Message));
            _snackbar.Add($"Error recording disbursement: {ex.Message}", Severity.Error);
        }
    }
}
