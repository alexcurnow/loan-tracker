namespace LoanTracker.Web.Store.ProjectState;

// Load Projects for a Loan
public record LoadProjectsForLoanAction(Guid LoanId);
public record LoadProjectsForLoanSuccessAction(IEnumerable<ProjectDto> Projects);
public record LoadProjectsForLoanFailureAction(string ErrorMessage);

// Load Disbursements for a Loan
public record LoadDisbursementsForLoanAction(Guid LoanId);
public record LoadDisbursementsForLoanSuccessAction(IEnumerable<DisbursementDto> Disbursements);
public record LoadDisbursementsForLoanFailureAction(string ErrorMessage);

// Load Interest Accruals for a Loan
public record LoadInterestAccrualsForLoanAction(Guid LoanId);
public record LoadInterestAccrualsForLoanSuccessAction(IEnumerable<InterestAccrualDto> Accruals);
public record LoadInterestAccrualsForLoanFailureAction(string ErrorMessage);

// Issue Disbursement
public record IssueDisbursementAction(
    Guid ProjectId,
    decimal Amount,
    string Currency,
    DateTime DisbursementDate,
    string RecipientName,
    string RecipientDetails
);
public record IssueDisbursementSuccessAction(DisbursementDto Disbursement);
public record IssueDisbursementFailureAction(string ErrorMessage);
