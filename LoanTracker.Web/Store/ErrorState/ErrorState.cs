using Fluxor;
using LoanTracker.Application.Common;

namespace LoanTracker.Web.Store.ErrorState;

[FeatureState]
public class ErrorState
{
    public Error? CurrentError { get; init; }

    public ErrorState()
    {
        CurrentError = null;
    }

    public ErrorState(Error? currentError)
    {
        CurrentError = currentError;
    }
}
