using Fluxor;

namespace LoanTracker.Web.Store.ErrorState;

public static class ErrorReducers
{
    [ReducerMethod]
    public static ErrorState ReduceShowErrorAction(ErrorState state, ShowErrorAction action)
    {
        return new ErrorState(action.Error);
    }

    [ReducerMethod]
    public static ErrorState ReduceClearErrorAction(ErrorState state, ClearErrorAction action)
    {
        return new ErrorState(null);
    }
}
