using Fluxor;

namespace LoanTracker.Web.Store.ProjectState;

public class ProjectFeature : Feature<ProjectState>
{
    public override string GetName() => "Project";

    protected override ProjectState GetInitialState() => new ProjectState
    {
        IsLoading = false,
        Projects = Array.Empty<ProjectDto>(),
        Disbursements = Array.Empty<DisbursementDto>(),
        InterestAccruals = Array.Empty<InterestAccrualDto>()
    };
}
