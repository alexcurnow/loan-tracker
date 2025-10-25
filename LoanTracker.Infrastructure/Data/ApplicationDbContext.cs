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
    public DbSet<Disbursement> Disbursements => Set<Disbursement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
