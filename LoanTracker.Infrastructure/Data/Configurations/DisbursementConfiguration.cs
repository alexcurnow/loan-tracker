using LoanTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanTracker.Infrastructure.Data.Configurations;

public class DisbursementConfiguration : IEntityTypeConfiguration<Disbursement>
{
    public void Configure(EntityTypeBuilder<Disbursement> builder)
    {
        builder.ToTable("Disbursements");

        builder.HasKey(d => d.DisbursementId);

        builder.Property(d => d.AmountValue)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasColumnName("Amount");

        builder.Property(d => d.AmountCurrency)
            .IsRequired()
            .HasMaxLength(3)
            .HasDefaultValue("USD");

        builder.Property(d => d.DisbursementDate)
            .IsRequired()
            .HasComment("When funds were actually released (can be backdated)");

        builder.Property(d => d.RecipientName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.RecipientDetails)
            .HasMaxLength(1000);

        builder.Property(d => d.CreatedAt)
            .IsRequired()
            .HasComment("When record was created in system");

        // Ignore the computed Amount property (it's a domain wrapper)
        builder.Ignore(d => d.Amount);

        // Relationships
        builder.HasOne(d => d.Project)
            .WithMany(p => p.Disbursements)
            .HasForeignKey(d => d.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);  // Don't cascade delete (immutable audit trail)

        // Indexes for querying
        builder.HasIndex(d => d.DisbursementDate)
            .HasDatabaseName("IX_Disbursements_DisbursementDate");

        builder.HasIndex(d => d.ProjectId)
            .HasDatabaseName("IX_Disbursements_ProjectId");

        // Seed sample disbursement data
        // Note: These are IMMUTABLE records - demonstrates backdating (DisbursementDate vs CreatedAt)
        var sampleDisbursements = new List<Disbursement>
        {
            // Road Repairs - First disbursement (backdated)
            new Disbursement
            {
                DisbursementId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddd01"),
                ProjectId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                AmountValue = 200000m,
                AmountCurrency = "USD",
                DisbursementDate = new DateTime(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc),
                RecipientName = "Metro Road Construction LLC",
                RecipientDetails = "Initial payment for road repairs phase 1 - Main St and Broadway",
                CreatedAt = new DateTime(2024, 1, 20, 12, 0, 0, DateTimeKind.Utc)  // Recorded 5 days later
            },
            // Road Repairs - Second disbursement (backdated)
            new Disbursement
            {
                DisbursementId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddd02"),
                ProjectId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                AmountValue = 100000m,
                AmountCurrency = "USD",
                DisbursementDate = new DateTime(2024, 2, 5, 12, 0, 0, DateTimeKind.Utc),
                RecipientName = "Metro Road Construction LLC",
                RecipientDetails = "Second payment for road repairs phase 2 - West End Ave",
                CreatedAt = new DateTime(2024, 2, 8, 12, 0, 0, DateTimeKind.Utc)  // Recorded 3 days later
            },
            // Bridge Upgrades - First disbursement (backdated)
            new Disbursement
            {
                DisbursementId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddd03"),
                ProjectId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaabbb"),
                AmountValue = 150000m,
                AmountCurrency = "USD",
                DisbursementDate = new DateTime(2024, 2, 10, 12, 0, 0, DateTimeKind.Utc),
                RecipientName = "Cumberland Bridge Engineering",
                RecipientDetails = "Initial payment for structural reinforcement of Jefferson St Bridge",
                CreatedAt = new DateTime(2024, 2, 10, 12, 0, 0, DateTimeKind.Utc)  // Same day
            },
            // New Classroom Wing - First disbursement
            new Disbursement
            {
                DisbursementId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddd04"),
                ProjectId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                AmountValue = 250000m,
                AmountCurrency = "USD",
                DisbursementDate = new DateTime(2024, 2, 15, 12, 0, 0, DateTimeKind.Utc),
                RecipientName = "Shelby Construction Group",
                RecipientDetails = "Foundation and site preparation for new classroom wing",
                CreatedAt = new DateTime(2024, 2, 15, 12, 0, 0, DateTimeKind.Utc)
            }
        };

        builder.HasData(sampleDisbursements);
    }
}
