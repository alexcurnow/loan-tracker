using LoanTracker.Application.Common;

namespace LoanTracker.Web.Store.ErrorState;

/// <summary>
/// Action to show an error globally
/// </summary>
public class ShowErrorAction
{
    public Error Error { get; }

    public ShowErrorAction(Error error)
    {
        Error = error;
    }
}

/// <summary>
/// Action to clear the current error
/// </summary>
public class ClearErrorAction { }
