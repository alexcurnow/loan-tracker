using LoanTracker.Domain.Enums;

namespace LoanTracker.Domain.Services;

public class WorkflowStateMachine
{
    private static readonly Dictionary<LoanStatus, List<LoanStatus>> ValidTransitions = new()
    {
        { LoanStatus.Open, new List<LoanStatus> { LoanStatus.AwaitingReview } },
        { LoanStatus.AwaitingReview, new List<LoanStatus> { LoanStatus.Open, LoanStatus.ApprovalPending } },
        { LoanStatus.ApprovalPending, new List<LoanStatus> { LoanStatus.AwaitingReview, LoanStatus.Approved, LoanStatus.Denied } },
        { LoanStatus.Approved, new List<LoanStatus>() },  // Terminal state
        { LoanStatus.Denied, new List<LoanStatus>() }      // Terminal state
    };

    public bool IsValidTransition(LoanStatus fromStatus, LoanStatus toStatus)
    {
        if (!ValidTransitions.ContainsKey(fromStatus))
        {
            return false;
        }

        return ValidTransitions[fromStatus].Contains(toStatus);
    }

    public List<LoanStatus> GetValidTransitions(LoanStatus currentStatus)
    {
        return ValidTransitions.TryGetValue(currentStatus, out var transitions)
            ? transitions
            : new List<LoanStatus>();
    }

    public string GetTransitionErrorMessage(LoanStatus fromStatus, LoanStatus toStatus)
    {
        if (fromStatus == LoanStatus.Approved || fromStatus == LoanStatus.Denied)
        {
            return $"Cannot transition from {fromStatus} status. This is a terminal state.";
        }

        var validTransitions = GetValidTransitions(fromStatus);
        if (validTransitions.Count == 0)
        {
            return $"No valid transitions available from {fromStatus} status.";
        }

        var validStates = string.Join(", ", validTransitions);
        return $"Cannot transition from {fromStatus} to {toStatus}. Valid transitions are: {validStates}";
    }
}
