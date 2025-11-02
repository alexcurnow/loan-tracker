using LoanTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanTracker.Infrastructure.Data.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(p => p.ProjectId);

        builder.Property(p => p.ProjectName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.BudgetAmount)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasColumnName("Budget");

        builder.Property(p => p.BudgetCurrency)
            .IsRequired()
            .HasMaxLength(3)
            .HasDefaultValue("USD");

        builder.Property(p => p.Description)
            .HasMaxLength(2000);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        // Ignore the computed Budget property (it's a domain wrapper)
        builder.Ignore(p => p.Budget);

        // Relationships
        builder.HasOne(p => p.Loan)
            .WithMany(l => l.Projects)
            .HasForeignKey(p => p.LoanId)
            .OnDelete(DeleteBehavior.Cascade);

        // Disbursements are now event-sourced via Marten, no EF Core relationship

        // Seed sample project data
        var sampleProjects = new List<Project>
        {
            // Metro Nashville Infrastructure - Road Repairs Project
            new Project
            {
                ProjectId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                LoanId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ProjectName = "Road Repairs",
                BudgetAmount = 400000m,
                BudgetCurrency = "USD",
                Description = "Repair and repaving of major downtown thoroughfares",
                CreatedAt = new DateTime(2024, 1, 10, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 10, 12, 0, 0, DateTimeKind.Utc)
            },
            // Metro Nashville Infrastructure - Bridge Upgrades Project
            new Project
            {
                ProjectId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaabbb"),
                LoanId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ProjectName = "Bridge Upgrades",
                BudgetAmount = 300000m,
                BudgetCurrency = "USD",
                Description = "Structural reinforcement and safety improvements for city bridges",
                CreatedAt = new DateTime(2024, 1, 10, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 10, 12, 0, 0, DateTimeKind.Utc)
            },
            // Shelby County Schools - New Classroom Wing Project
            new Project
            {
                ProjectId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                LoanId = Guid.Parse("11111111-1111-1111-1111-111111111112"),
                ProjectName = "New Classroom Wing",
                BudgetAmount = 500000m,
                BudgetCurrency = "USD",
                Description = "Construction of new classroom wing to address overcrowding",
                CreatedAt = new DateTime(2024, 2, 5, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 2, 5, 12, 0, 0, DateTimeKind.Utc)
            }
        };

        builder.HasData(sampleProjects);
    }
}
