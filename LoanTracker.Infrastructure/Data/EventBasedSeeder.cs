using LoanTracker.Application.Commands;
using LoanTracker.Application.Interfaces;
using LoanTracker.Domain.Enums;
using LoanTracker.Domain.Interfaces;
using LoanTracker.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace LoanTracker.Infrastructure.Data;

/// <summary>
/// Seeds disbursement events for demonstration purposes
/// Uses the actual command handler to ensure business logic is followed
/// Idempotent - checks if disbursements already exist before seeding
/// </summary>
public class EventBasedSeeder
{
    private readonly ILoanRepository _loanRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IDisbursementQuery _disbursementQuery;
    private readonly ICommandHandler<IssueDisbursementCommand, Application.Common.Result<Application.Commands.DisbursementDto>> _disbursementHandler;
    private readonly ILogger<EventBasedSeeder> _logger;

    public EventBasedSeeder(
        ILoanRepository loanRepository,
        IProjectRepository projectRepository,
        IDisbursementQuery disbursementQuery,
        ICommandHandler<IssueDisbursementCommand, Application.Common.Result<Application.Commands.DisbursementDto>> disbursementHandler,
        ILogger<EventBasedSeeder> logger)
    {
        _loanRepository = loanRepository;
        _projectRepository = projectRepository;
        _disbursementQuery = disbursementQuery;
        _disbursementHandler = disbursementHandler;
        _logger = logger;
    }

    public async Task SeedDisbursementsAsync()
    {
        _logger.LogInformation("Starting event-based disbursement seeding...");

        // Seed disbursements for loan 11111111-1111-1111-1111-111111111111 (Construction - Road Repairs)
        await SeedRoadRepairsDisbursementsAsync();

        // Seed disbursements for loan 11111111-1111-1111-1111-111111111112 (Construction - School Wing)
        await SeedSchoolWingDisbursementsAsync();

        // Seed disbursements for loan 11111111-1111-1111-1111-111111111113 (Approved - Fire Station)
        // This will transition it to Construction
        await SeedFireStationDisbursementsAsync();

        _logger.LogInformation("Event-based disbursement seeding completed.");
    }

    private async Task<bool> HasExistingDisbursementsAsync(Guid loanId)
    {
        var existingDisbursements = await _disbursementQuery.GetByLoanIdAsync(loanId);
        return existingDisbursements.Any();
    }

    private async Task SeedRoadRepairsDisbursementsAsync()
    {
        var loanId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        // Check if disbursements already exist (idempotent)
        if (await HasExistingDisbursementsAsync(loanId))
        {
            _logger.LogInformation("Disbursements already exist for Road Repairs loan, skipping seed");
            return;
        }

        var loan = await _loanRepository.GetByIdAsync(loanId);

        if (loan == null)
        {
            _logger.LogWarning("Loan {LoanId} not found for seeding", loanId);
            return;
        }

        var projects = await _projectRepository.GetByLoanIdAsync(loanId);
        var roadRepairsProject = projects.FirstOrDefault(p => p.ProjectName == "Road Repairs");
        var bridgeProject = projects.FirstOrDefault(p => p.ProjectName == "Bridge Upgrades");

        if (roadRepairsProject == null || bridgeProject == null)
        {
            _logger.LogWarning("Projects not found for loan {LoanId}", loanId);
            return;
        }

        // Seed 3 disbursements for Road Repairs (backdated)
        await IssueDisbursementAsync(
            roadRepairsProject.ProjectId,
            200000m,
            new DateTime(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc),
            "Metro Road Construction LLC",
            "Initial payment for road repairs phase 1 - Main St and Broadway"
        );

        await IssueDisbursementAsync(
            roadRepairsProject.ProjectId,
            100000m,
            new DateTime(2024, 2, 5, 12, 0, 0, DateTimeKind.Utc),
            "Metro Road Construction LLC",
            "Second payment for road repairs phase 2 - West End Ave"
        );

        // Seed 1 disbursement for Bridge Upgrades (backdated)
        await IssueDisbursementAsync(
            bridgeProject.ProjectId,
            150000m,
            new DateTime(2024, 2, 10, 12, 0, 0, DateTimeKind.Utc),
            "Cumberland Bridge Engineering",
            "Initial payment for structural reinforcement of Jefferson St Bridge"
        );

        _logger.LogInformation("Seeded 3 disbursements for Road Repairs loan");
    }

    private async Task SeedSchoolWingDisbursementsAsync()
    {
        var loanId = Guid.Parse("11111111-1111-1111-1111-111111111112");

        // Check if disbursements already exist (idempotent)
        if (await HasExistingDisbursementsAsync(loanId))
        {
            _logger.LogInformation("Disbursements already exist for School Wing loan, skipping seed");
            return;
        }

        var loan = await _loanRepository.GetByIdAsync(loanId);

        if (loan == null)
        {
            _logger.LogWarning("Loan {LoanId} not found for seeding", loanId);
            return;
        }

        var projects = await _projectRepository.GetByLoanIdAsync(loanId);
        var schoolProject = projects.FirstOrDefault(p => p.ProjectName == "New Classroom Wing");

        if (schoolProject == null)
        {
            _logger.LogWarning("School project not found for loan {LoanId}", loanId);
            return;
        }

        // Seed 1 disbursement for School Wing (backdated)
        await IssueDisbursementAsync(
            schoolProject.ProjectId,
            250000m,
            new DateTime(2024, 2, 15, 12, 0, 0, DateTimeKind.Utc),
            "Shelby Construction Group",
            "Foundation and site preparation for new classroom wing"
        );

        _logger.LogInformation("Seeded 1 disbursement for School Wing loan");
    }

    private async Task SeedFireStationDisbursementsAsync()
    {
        var loanId = Guid.Parse("11111111-1111-1111-1111-111111111113");

        // Check if disbursements already exist (idempotent)
        if (await HasExistingDisbursementsAsync(loanId))
        {
            _logger.LogInformation("Disbursements already exist for Fire Station loan, skipping seed");
            return;
        }

        var loan = await _loanRepository.GetByIdAsync(loanId);

        if (loan == null)
        {
            _logger.LogWarning("Loan {LoanId} not found for seeding", loanId);
            return;
        }

        // Check if loan is in Approved status
        if (loan.Status != LoanStatus.Approved)
        {
            _logger.LogInformation("Fire Station loan is already in {Status} status, skipping disbursement seed", loan.Status);
            return;
        }

        // First, we need to create a project for this loan
        var existingProjects = await _projectRepository.GetByLoanIdAsync(loanId);
        if (!existingProjects.Any())
        {
            _logger.LogInformation("Creating project for Fire Station loan");
            var fireStationProject = new Domain.Entities.Project
            {
                ProjectId = Guid.NewGuid(),
                LoanId = loanId,
                ProjectName = "Fire Station Construction",
                BudgetAmount = 600000m,
                BudgetCurrency = "USD",
                Description = "New fire station building with emergency vehicle bays and equipment storage",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _projectRepository.AddAsync(fireStationProject);

            // Issue first disbursement - this will transition the loan to Construction
            await IssueDisbursementAsync(
                fireStationProject.ProjectId,
                300000m,
                DateTime.UtcNow,
                "Knox County Construction LLC",
                "Initial disbursement for site preparation and foundation work"
            );

            _logger.LogInformation("Seeded 1 disbursement for Fire Station loan (transitioned from Approved to Construction)");
        }
    }

    private async Task<bool> IssueDisbursementAsync(
        Guid projectId,
        decimal amount,
        DateTime disbursementDate,
        string recipientName,
        string recipientDetails)
    {
        var command = new IssueDisbursementCommand
        {
            ProjectId = projectId,
            Amount = amount,
            Currency = "USD",
            DisbursementDate = disbursementDate,
            RecipientName = recipientName,
            RecipientDetails = recipientDetails
        };

        var result = await _disbursementHandler.HandleAsync(command);

        if (result.IsSuccess)
        {
            _logger.LogDebug("Disbursement issued: {Amount} to {Recipient} on {Date}",
                amount, recipientName, disbursementDate);
            return true;
        }
        else
        {
            _logger.LogWarning("Failed to issue disbursement: {Error}", result.Error);
            return false;
        }
    }
}
