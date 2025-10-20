using Fluxor;

namespace LoanTracker.Web.Store.LoanState;

public static class LoanReducers
{
    [ReducerMethod]
    public static LoanState ReduceLoadLoansAction(LoanState state, LoadLoansAction action)
    {
        return new LoanState(true, state.Loans, state.SelectedLoan, null);
    }

    [ReducerMethod]
    public static LoanState ReduceLoadLoansSuccessAction(LoanState state, LoadLoansSuccessAction action)
    {
        return new LoanState(false, action.Loans, state.SelectedLoan, null);
    }

    [ReducerMethod]
    public static LoanState ReduceLoadLoansFailureAction(LoanState state, LoadLoansFailureAction action)
    {
        return new LoanState(false, state.Loans, state.SelectedLoan, action.ErrorMessage);
    }

    [ReducerMethod]
    public static LoanState ReduceLoadLoanByIdAction(LoanState state, LoadLoanByIdAction action)
    {
        return new LoanState(true, state.Loans, state.SelectedLoan, null);
    }

    [ReducerMethod]
    public static LoanState ReduceLoadLoanByIdSuccessAction(LoanState state, LoadLoanByIdSuccessAction action)
    {
        return new LoanState(false, state.Loans, action.Loan, null);
    }

    [ReducerMethod]
    public static LoanState ReduceLoadLoanByIdFailureAction(LoanState state, LoadLoanByIdFailureAction action)
    {
        return new LoanState(false, state.Loans, state.SelectedLoan, action.ErrorMessage);
    }

    [ReducerMethod]
    public static LoanState ReduceCreateLoanAction(LoanState state, CreateLoanAction action)
    {
        return new LoanState(true, state.Loans, state.SelectedLoan, null);
    }

    [ReducerMethod]
    public static LoanState ReduceCreateLoanSuccessAction(LoanState state, CreateLoanSuccessAction action)
    {
        var updatedLoans = state.Loans.Prepend(action.Loan);
        return new LoanState(false, updatedLoans, action.Loan, null);
    }

    [ReducerMethod]
    public static LoanState ReduceCreateLoanFailureAction(LoanState state, CreateLoanFailureAction action)
    {
        return new LoanState(false, state.Loans, state.SelectedLoan, action.ErrorMessage);
    }

    [ReducerMethod]
    public static LoanState ReduceTransitionLoanStatusAction(LoanState state, TransitionLoanStatusAction action)
    {
        return new LoanState(true, state.Loans, state.SelectedLoan, null);
    }

    [ReducerMethod]
    public static LoanState ReduceTransitionLoanStatusSuccessAction(LoanState state, TransitionLoanStatusSuccessAction action)
    {
        var updatedLoans = state.Loans.Select(l => l.LoanId == action.Loan.LoanId ? action.Loan : l);
        var updatedSelectedLoan = state.SelectedLoan?.LoanId == action.Loan.LoanId ? action.Loan : state.SelectedLoan;
        return new LoanState(false, updatedLoans, updatedSelectedLoan, null);
    }

    [ReducerMethod]
    public static LoanState ReduceTransitionLoanStatusFailureAction(LoanState state, TransitionLoanStatusFailureAction action)
    {
        return new LoanState(false, state.Loans, state.SelectedLoan, action.ErrorMessage);
    }

    [ReducerMethod]
    public static LoanState ReduceClearSelectedLoanAction(LoanState state, ClearSelectedLoanAction action)
    {
        return new LoanState(state.IsLoading, state.Loans, null, state.ErrorMessage);
    }

    [ReducerMethod]
    public static LoanState ReduceClearErrorAction(LoanState state, ClearErrorAction action)
    {
        return new LoanState(state.IsLoading, state.Loans, state.SelectedLoan, null);
    }
}
