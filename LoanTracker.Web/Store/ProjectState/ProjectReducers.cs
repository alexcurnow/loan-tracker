using Fluxor;

namespace LoanTracker.Web.Store.ProjectState;

public static class ProjectReducers
{
    [ReducerMethod]
    public static ProjectState ReduceLoadProjectsForLoanAction(ProjectState state, LoadProjectsForLoanAction action)
    {
        return state with { IsLoading = true, ErrorMessage = null };
    }

    [ReducerMethod]
    public static ProjectState ReduceLoadProjectsForLoanSuccessAction(ProjectState state, LoadProjectsForLoanSuccessAction action)
    {
        return state with { IsLoading = false, Projects = action.Projects };
    }

    [ReducerMethod]
    public static ProjectState ReduceLoadProjectsForLoanFailureAction(ProjectState state, LoadProjectsForLoanFailureAction action)
    {
        return state with { IsLoading = false, ErrorMessage = action.ErrorMessage };
    }

    [ReducerMethod]
    public static ProjectState ReduceLoadDisbursementsForLoanAction(ProjectState state, LoadDisbursementsForLoanAction action)
    {
        return state with { IsLoading = true, ErrorMessage = null };
    }

    [ReducerMethod]
    public static ProjectState ReduceLoadDisbursementsForLoanSuccessAction(ProjectState state, LoadDisbursementsForLoanSuccessAction action)
    {
        return state with { IsLoading = false, Disbursements = action.Disbursements };
    }

    [ReducerMethod]
    public static ProjectState ReduceLoadDisbursementsForLoanFailureAction(ProjectState state, LoadDisbursementsForLoanFailureAction action)
    {
        return state with { IsLoading = false, ErrorMessage = action.ErrorMessage };
    }

    [ReducerMethod]
    public static ProjectState ReduceLoadInterestAccrualsForLoanAction(ProjectState state, LoadInterestAccrualsForLoanAction action)
    {
        return state with { IsLoading = true, ErrorMessage = null };
    }

    [ReducerMethod]
    public static ProjectState ReduceLoadInterestAccrualsForLoanSuccessAction(ProjectState state, LoadInterestAccrualsForLoanSuccessAction action)
    {
        return state with { IsLoading = false, InterestAccruals = action.Accruals };
    }

    [ReducerMethod]
    public static ProjectState ReduceLoadInterestAccrualsForLoanFailureAction(ProjectState state, LoadInterestAccrualsForLoanFailureAction action)
    {
        return state with { IsLoading = false, ErrorMessage = action.ErrorMessage };
    }

    [ReducerMethod]
    public static ProjectState ReduceIssueDisbursementAction(ProjectState state, IssueDisbursementAction action)
    {
        return state with { IsLoading = true, ErrorMessage = null };
    }

    [ReducerMethod]
    public static ProjectState ReduceIssueDisbursementSuccessAction(ProjectState state, IssueDisbursementSuccessAction action)
    {
        var updatedDisbursements = state.Disbursements.Append(action.Disbursement).OrderByDescending(d => d.DisbursementDate);
        return state with { IsLoading = false, Disbursements = updatedDisbursements };
    }

    [ReducerMethod]
    public static ProjectState ReduceIssueDisbursementFailureAction(ProjectState state, IssueDisbursementFailureAction action)
    {
        return state with { IsLoading = false, ErrorMessage = action.ErrorMessage };
    }
}
