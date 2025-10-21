using Fluxor;
using LoanTracker.Application.DTOs;

namespace LoanTracker.Web.Store.LoanState;

[FeatureState]
public class LoanState
{
    public bool IsLoading { get; init; }
    public IEnumerable<LoanDto> Loans { get; init; }
    public LoanDto? SelectedLoan { get; init; }

    public LoanState()
    {
        IsLoading = false;
        Loans = Array.Empty<LoanDto>();
        SelectedLoan = null;
    }

    public LoanState(bool isLoading, IEnumerable<LoanDto> loans, LoanDto? selectedLoan)
    {
        IsLoading = isLoading;
        Loans = loans;
        SelectedLoan = selectedLoan;
    }
}
