using LoanTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanTracker.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<BorrowerType> BorrowerTypes => Set<BorrowerType>();
    public DbSet<Project> Projects => Set<Project>();
    // Disbursements are now event-sourced via Marten, not stored in EF Core

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ignore value objects and Marten projection types
        modelBuilder.Ignore<LoanTracker.Domain.ValueObjects.Money>();
        modelBuilder.Ignore<LoanTracker.Domain.ValueObjects.InterestRate>();
        modelBuilder.Ignore<LoanTracker.Infrastructure.Projections.DisbursementHistoryProjection>();
        modelBuilder.Ignore<LoanTracker.Infrastructure.Projections.DisbursementRecord>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
