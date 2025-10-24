using Fluxor;
using LoanTracker.Application.Common;
using MudBlazor;

namespace LoanTracker.Web.Store.ErrorState;

public class ErrorEffects
{
    private readonly ISnackbar _snackbar;

    public ErrorEffects(ISnackbar snackbar)
    {
        _snackbar = snackbar;
    }

    [EffectMethod]
    public Task HandleShowErrorAction(ShowErrorAction action, IDispatcher dispatcher)
    {
        var severity = GetSeverityFromErrorType(action.Error.Type);
        _snackbar.Add(action.Error.Message, severity);

        // Auto-clear the error after showing it
        dispatcher.Dispatch(new ClearErrorAction());

        return Task.CompletedTask;
    }

    private static Severity GetSeverityFromErrorType(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Validation => Severity.Warning,
            ErrorType.NotFound => Severity.Info,
            ErrorType.Conflict => Severity.Warning,
            ErrorType.ServerError => Severity.Error,
            _ => Severity.Error
        };
    }
}
