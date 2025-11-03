using LoanTracker.Application.DTOs;
using LoanTracker.Domain.Enums;
using LoanTracker.Domain.Types;

namespace LoanTracker.Web.Store.LoanState;

// Load all loans
public class LoadLoansAction { }
public class LoadLoansSuccessAction
{
    public IEnumerable<LoanDto> Loans { get; }
    public LoadLoansSuccessAction(IEnumerable<LoanDto> loans)
    {
        Loans = loans;
    }
}
public class LoadLoansFailureAction
{
    public string ErrorMessage { get; }
    public LoadLoansFailureAction(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}

// Load loan by ID
public class LoadLoanByIdAction
{
    public Guid LoanId { get; }
    public LoadLoanByIdAction(Guid loanId)
    {
        LoanId = loanId;
    }
}
public class LoadLoanByIdSuccessAction
{
    public LoanDto Loan { get; }
    public LoadLoanByIdSuccessAction(LoanDto loan)
    {
        Loan = loan;
    }
}
public class LoadLoanByIdFailureAction
{
    public string ErrorMessage { get; }
    public LoadLoanByIdFailureAction(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}

// Create loan
public sealed record CreateLoanAction(
    string BorrowerName,
    int BorrowerTypeId,
    string ContactPerson,
    string ContactEmail,
    decimal Amount,
    decimal InterestRate,
    TermYears TermYears,
    string Purpose);

public class CreateLoanSuccessAction
{
    public LoanDto Loan { get; }
    public CreateLoanSuccessAction(LoanDto loan)
    {
        Loan = loan;
    }
}
public class CreateLoanFailureAction
{
    public string ErrorMessage { get; }
    public CreateLoanFailureAction(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}

// Transition loan status
public class TransitionLoanStatusAction
{
    public Guid LoanId { get; }
    public LoanStatus ToStatus { get; }
    public string? ReviewerNotes { get; }

    public TransitionLoanStatusAction(Guid loanId, LoanStatus toStatus, string? reviewerNotes = null)
    {
        LoanId = loanId;
        ToStatus = toStatus;
        ReviewerNotes = reviewerNotes;
    }
}
public class TransitionLoanStatusSuccessAction
{
    public LoanDto Loan { get; }
    public TransitionLoanStatusSuccessAction(LoanDto loan)
    {
        Loan = loan;
    }
}
public class TransitionLoanStatusFailureAction
{
    public string ErrorMessage { get; }
    public TransitionLoanStatusFailureAction(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}

// Clear selected loan
public class ClearSelectedLoanAction { }

// Clear error
public class ClearErrorAction { }
