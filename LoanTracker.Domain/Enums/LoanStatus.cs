namespace LoanTracker.Domain.Enums;

public enum LoanStatus
{
    Open = 0,
    AwaitingReview = 1,
    ApprovalPending = 2,
    Approved = 3,
    Denied = 4,
    Construction = 5,  // First disbursement issued after approval
    Active = 6          // All disbursements complete, loan in repayment
}
