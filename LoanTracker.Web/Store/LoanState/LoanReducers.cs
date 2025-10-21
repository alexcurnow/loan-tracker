using Fluxor;

namespace LoanTracker.Web.Store.LoanState;

public static class LoanReducers
{
    [ReducerMethod]
    public static LoanState ReduceLoadLoansAction(LoanState state, LoadLoansAction action)
    {
        return new LoanState(true, state.Loans, state.SelectedLoan);
    }

    [ReducerMethod]
    public static LoanState ReduceLoadLoansSuccessAction(LoanState state, LoadLoansSuccessAction action)
    {
        return new LoanState(false, action.Loans, state.SelectedLoan);
    }

    [ReducerMethod]
    public static LoanState ReduceLoadLoanByIdAction(LoanState state, LoadLoanByIdAction action)
    {
        return new LoanState(true, state.Loans, state.SelectedLoan);
    }

    [ReducerMethod]
    public static LoanState ReduceLoadLoanByIdSuccessAction(LoanState state, LoadLoanByIdSuccessAction action)
    {
        return new LoanState(false, state.Loans, action.Loan);
    }

    [ReducerMethod]
    public static LoanState ReduceCreateLoanAction(LoanState state, CreateLoanAction action)
    {
        return new LoanState(true, state.Loans, state.SelectedLoan);
    }

    [ReducerMethod]
    public static LoanState ReduceCreateLoanSuccessAction(LoanState state, CreateLoanSuccessAction action)
    {
        var updatedLoans = state.Loans.Prepend(action.Loan);
        return new LoanState(false, updatedLoans, action.Loan);
    }

    [ReducerMethod]
    public static LoanState ReduceTransitionLoanStatusAction(LoanState state, TransitionLoanStatusAction action)
    {
        return new LoanState(true, state.Loans, state.SelectedLoan);
    }

    [ReducerMethod]
    public static LoanState ReduceTransitionLoanStatusSuccessAction(LoanState state, TransitionLoanStatusSuccessAction action)
    {
        var updatedLoans = state.Loans.Select(l => l.LoanId == action.Loan.LoanId ? action.Loan : l);
        var updatedSelectedLoan = state.SelectedLoan?.LoanId == action.Loan.LoanId ? action.Loan : state.SelectedLoan;
        return new LoanState(false, updatedLoans, updatedSelectedLoan);
    }

    [ReducerMethod]
    public static LoanState ReduceClearSelectedLoanAction(LoanState state, ClearSelectedLoanAction action)
    {
        return new LoanState(state.IsLoading, state.Loans, null);
    }
}
