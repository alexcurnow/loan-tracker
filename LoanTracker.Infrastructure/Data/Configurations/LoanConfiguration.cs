using Dots.Standard.StrongTypes;
using LoanTracker.Domain.Entities;
using LoanTracker.Domain.Enums;
using LoanTracker.Domain.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanTracker.Infrastructure.Data.Configurations;

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.ToTable("Loans");

        builder.HasKey(l => l.LoanId);

        builder.Property(l => l.BorrowerName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.ContactPerson)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.ContactEmail)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(l => l.InterestRate)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(l => l.TermYears)
            .IsRequired();

        builder.Property(l => l.Purpose)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(l => l.ApplicationDate)
            .IsRequired();

        builder.Property(l => l.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(l => l.ReviewerNotes)
            .HasMaxLength(2000);

        builder.Property(l => l.CreatedAt)
            .IsRequired();

        builder.Property(l => l.UpdatedAt)
            .IsRequired();

        builder.HasOne(l => l.BorrowerType)
            .WithMany(bt => bt.Loans)
            .HasForeignKey(l => l.BorrowerTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed sample loan data with hardcoded dates

        var sampleLoans = new List<Loan>
        {
            // Construction loans (2) - These have projects and disbursements
            new Loan
            {
                LoanId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                BorrowerName = "Metro Nashville Infrastructure",
                BorrowerTypeId = 1,
                ContactPerson = "John Smith",
                ContactEmail = "jsmith@nashville.gov",
                Amount = 700000m,
                InterestRate = 3.5m,
                TermYears = 15.As<TermYears>(),
                Purpose = "Road repairs and bridge upgrades for city infrastructure",
                ApplicationDate = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.Construction,
                DueDay = 20,
                DecisionDate = new DateTime(2024, 1, 5, 12, 0, 0, DateTimeKind.Utc),
                ReviewerNotes = "Critical infrastructure needs. Approved for phased construction.",
                CreatedAt = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 2, 10, 12, 0, 0, DateTimeKind.Utc)
            },
            new Loan
            {
                LoanId = Guid.Parse("11111111-1111-1111-1111-111111111112"),
                BorrowerName = "Shelby County Schools",
                BorrowerTypeId = 3,
                ContactPerson = "Mary Johnson",
                ContactEmail = "mjohnson@shelbycounty.gov",
                Amount = 500000m,
                InterestRate = 2.5m,
                TermYears = 20.As<TermYears>(),
                Purpose = "New classroom wing construction",
                ApplicationDate = new DateTime(2024, 2, 1, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.Construction,
                DueDay = 20,
                DecisionDate = new DateTime(2024, 2, 5, 12, 0, 0, DateTimeKind.Utc),
                ReviewerNotes = "Addresses critical overcrowding. Strong enrollment projections.",
                CreatedAt = new DateTime(2024, 2, 1, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 2, 15, 12, 0, 0, DateTimeKind.Utc)
            },
            new Loan
            {
                LoanId = Guid.Parse("11111111-1111-1111-1111-111111111113"),
                BorrowerName = "Knox County Fire Department",
                BorrowerTypeId = 2,
                ContactPerson = "Robert Williams",
                ContactEmail = "rwilliams@knoxcounty.gov",
                Amount = 600000m,
                InterestRate = 3.0m,
                TermYears = 15.As<TermYears>(),
                Purpose = "New fire station and equipment",
                ApplicationDate = new DateTime(2024, 3, 1, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.Approved,
                DueDay = 20,
                DecisionDate = new DateTime(2024, 3, 10, 12, 0, 0, DateTimeKind.Utc),
                ReviewerNotes = "Approved. Waiting for contractor selection before first disbursement.",
                CreatedAt = new DateTime(2024, 3, 1, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 3, 10, 12, 0, 0, DateTimeKind.Utc)
            },
            new Loan
            {
                LoanId = Guid.Parse("11111111-1111-1111-1111-111111111114"),
                BorrowerName = "Tennessee Tech Small Business",
                BorrowerTypeId = 5,
                ContactPerson = "Sarah Davis",
                ContactEmail = "sdavis@tntech.com",
                Amount = 500000m,
                InterestRate = 4.5m,
                TermYears = 10.As<TermYears>(),
                Purpose = "Equipment purchase and facility expansion",
                ApplicationDate = new DateTime(2025, 10, 16, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.Open,
                CreatedAt = new DateTime(2025, 10, 16, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 10, 16, 12, 0, 0, DateTimeKind.Utc)
            },
            new Loan
            {
                LoanId = Guid.Parse("11111111-1111-1111-1111-111111111115"),
                BorrowerName = "City of Memphis",
                BorrowerTypeId = 1,
                ContactPerson = "James Brown",
                ContactEmail = "jbrown@memphis.gov",
                Amount = 4000000m,
                InterestRate = 3.25m,
                TermYears = 30.As<TermYears>(),
                Purpose = "Public transportation infrastructure upgrades",
                ApplicationDate = new DateTime(2025, 10, 18, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.Open,
                CreatedAt = new DateTime(2025, 10, 18, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 10, 18, 12, 0, 0, DateTimeKind.Utc)
            },

            // AwaitingReview loans (4)
            new Loan
            {
                LoanId = Guid.Parse("22222222-2222-2222-2222-222222222221"),
                BorrowerName = "University of Tennessee",
                BorrowerTypeId = 4,
                ContactPerson = "Patricia Garcia",
                ContactEmail = "pgarcia@utk.edu",
                Amount = 5000000m,
                InterestRate = 2.75m,
                TermYears = 30.As<TermYears>(),
                Purpose = "Research facility construction and equipment",
                ApplicationDate = new DateTime(2025, 9, 24, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.AwaitingReview,
                CreatedAt = new DateTime(2025, 9, 24, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 9, 29, 12, 0, 0, DateTimeKind.Utc)
            },
            new Loan
            {
                LoanId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                BorrowerName = "Hamilton County",
                BorrowerTypeId = 2,
                ContactPerson = "Michael Martinez",
                ContactEmail = "mmartinez@hamiltoncounty.gov",
                Amount = 1200000m,
                InterestRate = 3.5m,
                TermYears = 15.As<TermYears>(),
                Purpose = "Emergency services facility renovation",
                ApplicationDate = new DateTime(2025, 9, 27, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.AwaitingReview,
                CreatedAt = new DateTime(2025, 9, 27, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 10, 1, 12, 0, 0, DateTimeKind.Utc)
            },
            new Loan
            {
                LoanId = Guid.Parse("22222222-2222-2222-2222-222222222223"),
                BorrowerName = "Tennessee Department of Transportation",
                BorrowerTypeId = 6,
                ContactPerson = "Linda Rodriguez",
                ContactEmail = "lrodriguez@tn.gov",
                Amount = 8000000m,
                InterestRate = 3.0m,
                TermYears = 25.As<TermYears>(),
                Purpose = "Highway expansion and bridge repairs",
                ApplicationDate = new DateTime(2025, 9, 30, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.AwaitingReview,
                CreatedAt = new DateTime(2025, 9, 30, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 10, 4, 12, 0, 0, DateTimeKind.Utc)
            },
            new Loan
            {
                LoanId = Guid.Parse("22222222-2222-2222-2222-222222222224"),
                BorrowerName = "Metro Nashville School District",
                BorrowerTypeId = 3,
                ContactPerson = "David Wilson",
                ContactEmail = "dwilson@mnps.org",
                Amount = 2500000m,
                InterestRate = 2.25m,
                TermYears = 20.As<TermYears>(),
                Purpose = "Technology infrastructure upgrade across all schools",
                ApplicationDate = new DateTime(2025, 10, 3, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.AwaitingReview,
                CreatedAt = new DateTime(2025, 10, 3, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 10, 7, 12, 0, 0, DateTimeKind.Utc)
            },

            // ApprovalPending loans (3)
            new Loan
            {
                LoanId = Guid.Parse("33333333-3333-3333-3333-333333333331"),
                BorrowerName = "City of Knoxville",
                BorrowerTypeId = 1,
                ContactPerson = "Barbara Anderson",
                ContactEmail = "banderson@knoxville.gov",
                Amount = 1800000m,
                InterestRate = 3.25m,
                TermYears = 18.As<TermYears>(),
                Purpose = "Public park development and recreation facilities",
                ApplicationDate = new DateTime(2025, 9, 9, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.ApprovalPending,
                CreatedAt = new DateTime(2025, 9, 9, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 9, 19, 12, 0, 0, DateTimeKind.Utc)
            },
            new Loan
            {
                LoanId = Guid.Parse("33333333-3333-3333-3333-333333333332"),
                BorrowerName = "Middle Tennessee State University",
                BorrowerTypeId = 4,
                ContactPerson = "Christopher Thomas",
                ContactEmail = "cthomas@mtsu.edu",
                Amount = 3500000m,
                InterestRate = 2.5m,
                TermYears = 25.As<TermYears>(),
                Purpose = "Student housing complex construction",
                ApplicationDate = new DateTime(2025, 9, 11, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.ApprovalPending,
                CreatedAt = new DateTime(2025, 9, 11, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 9, 21, 12, 0, 0, DateTimeKind.Utc)
            },
            new Loan
            {
                LoanId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                BorrowerName = "Davidson County Library",
                BorrowerTypeId = 2,
                ContactPerson = "Jessica Taylor",
                ContactEmail = "jtaylor@nashville.gov",
                Amount = 400000m,
                InterestRate = 3.75m,
                TermYears = 12.As<TermYears>(),
                Purpose = "Library system modernization and technology upgrades",
                ApplicationDate = new DateTime(2024, 4, 1, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.AwaitingReview,
                DueDay = 20,
                CreatedAt = new DateTime(2024, 4, 1, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 4, 5, 12, 0, 0, DateTimeKind.Utc)
            },

            // Approved loans (5)
            new Loan
            {
                LoanId = Guid.Parse("44444444-4444-4444-4444-444444444441"),
                BorrowerName = "City of Chattanooga",
                BorrowerTypeId = 1,
                ContactPerson = "Daniel White",
                ContactEmail = "dwhite@chattanooga.gov",
                Amount = 2200000m,
                InterestRate = 3.0m,
                TermYears = 20.As<TermYears>(),
                Purpose = "Downtown revitalization project",
                ApplicationDate = new DateTime(2025, 8, 20, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.Approved,
                DecisionDate = new DateTime(2025, 9, 4, 12, 0, 0, DateTimeKind.Utc),
                ReviewerNotes = "Excellent project proposal with strong community support. Approved for full amount requested.",
                CreatedAt = new DateTime(2025, 8, 20, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 9, 4, 12, 0, 0, DateTimeKind.Utc)
            },
            new Loan
            {
                LoanId = Guid.Parse("44444444-4444-4444-4444-444444444442"),
                BorrowerName = "Williamson County School District",
                BorrowerTypeId = 3,
                ContactPerson = "Karen Harris",
                ContactEmail = "kharris@wcsdistrict.com",
                Amount = 4500000m,
                InterestRate = 2.25m,
                TermYears = 30.As<TermYears>(),
                Purpose = "New high school construction",
                ApplicationDate = new DateTime(2025, 8, 25, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.Approved,
                DecisionDate = new DateTime(2025, 9, 9, 12, 0, 0, DateTimeKind.Utc),
                ReviewerNotes = "Critical need demonstrated. Project aligns with state education goals.",
                CreatedAt = new DateTime(2025, 8, 25, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 9, 9, 12, 0, 0, DateTimeKind.Utc)
            },
            new Loan
            {
                LoanId = Guid.Parse("44444444-4444-4444-4444-444444444443"),
                BorrowerName = "Tennessee Department of Environment",
                BorrowerTypeId = 6,
                ContactPerson = "Steven Martin",
                ContactEmail = "smartin@tn.gov",
                Amount = 1000000m,
                InterestRate = 2.75m,
                TermYears = 15.As<TermYears>(),
                Purpose = "Water treatment facility upgrades",
                ApplicationDate = new DateTime(2025, 8, 30, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.Approved,
                DecisionDate = new DateTime(2025, 9, 14, 12, 0, 0, DateTimeKind.Utc),
                ReviewerNotes = "Essential environmental infrastructure project. Approved.",
                CreatedAt = new DateTime(2025, 8, 30, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 9, 14, 12, 0, 0, DateTimeKind.Utc)
            },
            new Loan
            {
                LoanId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                BorrowerName = "Austin Peay State University",
                BorrowerTypeId = 4,
                ContactPerson = "Nancy Jackson",
                ContactEmail = "njackson@apsu.edu",
                Amount = 2800000m,
                InterestRate = 2.5m,
                TermYears = 25.As<TermYears>(),
                Purpose = "Science building renovation and laboratory upgrades",
                ApplicationDate = new DateTime(2025, 9, 1, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.Approved,
                DecisionDate = new DateTime(2025, 9, 16, 12, 0, 0, DateTimeKind.Utc),
                ReviewerNotes = "Strong academic justification. Supports STEM education initiatives.",
                CreatedAt = new DateTime(2025, 9, 1, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 9, 16, 12, 0, 0, DateTimeKind.Utc)
            },
            new Loan
            {
                LoanId = Guid.Parse("44444444-4444-4444-4444-444444444445"),
                BorrowerName = "Rutherford County",
                BorrowerTypeId = 2,
                ContactPerson = "Brian Thompson",
                ContactEmail = "bthompson@rutherfordcounty.gov",
                Amount = 1600000m,
                InterestRate = 3.5m,
                TermYears = 18.As<TermYears>(),
                Purpose = "Public safety communication system upgrade",
                ApplicationDate = new DateTime(2025, 9, 4, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.Approved,
                DecisionDate = new DateTime(2025, 9, 19, 12, 0, 0, DateTimeKind.Utc),
                ReviewerNotes = "Critical public safety need. Approved for full funding.",
                CreatedAt = new DateTime(2025, 9, 4, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 9, 19, 12, 0, 0, DateTimeKind.Utc)
            },

            // Denied loans (3)
            new Loan
            {
                LoanId = Guid.Parse("55555555-5555-5555-5555-555555555551"),
                BorrowerName = "Small Town Manufacturing LLC",
                BorrowerTypeId = 5,
                ContactPerson = "Kevin Lee",
                ContactEmail = "klee@stmfg.com",
                Amount = 750000m,
                InterestRate = 5.5m,
                TermYears = 10.As<TermYears>(),
                Purpose = "Factory expansion and new equipment",
                ApplicationDate = new DateTime(2025, 8, 28, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.Denied,
                DecisionDate = new DateTime(2025, 9, 12, 12, 0, 0, DateTimeKind.Utc),
                ReviewerNotes = "Insufficient collateral and questionable financial projections. Recommend reapplication with revised business plan.",
                CreatedAt = new DateTime(2025, 8, 28, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 9, 12, 12, 0, 0, DateTimeKind.Utc)
            },
            new Loan
            {
                LoanId = Guid.Parse("55555555-5555-5555-5555-555555555552"),
                BorrowerName = "City of Springfield",
                BorrowerTypeId = 1,
                ContactPerson = "Michelle Clark",
                ContactEmail = "mclark@springfield.gov",
                Amount = 5500000m,
                InterestRate = 4.0m,
                TermYears = 25.As<TermYears>(),
                Purpose = "Sports complex and convention center",
                ApplicationDate = new DateTime(2025, 8, 31, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.Denied,
                DecisionDate = new DateTime(2025, 9, 15, 12, 0, 0, DateTimeKind.Utc),
                ReviewerNotes = "Project scope exceeds municipality's debt capacity. Insufficient revenue projections to support debt service.",
                CreatedAt = new DateTime(2025, 8, 31, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 9, 15, 12, 0, 0, DateTimeKind.Utc)
            },
            new Loan
            {
                LoanId = Guid.Parse("55555555-5555-5555-5555-555555555553"),
                BorrowerName = "Regional Small Business Co-op",
                BorrowerTypeId = 5,
                ContactPerson = "Angela Moore",
                ContactEmail = "amoore@regionalbiz.com",
                Amount = 350000m,
                InterestRate = 6.0m,
                TermYears = 8.As<TermYears>(),
                Purpose = "Shared warehouse facility",
                ApplicationDate = new DateTime(2025, 9, 3, 12, 0, 0, DateTimeKind.Utc),
                Status = LoanStatus.Denied,
                DecisionDate = new DateTime(2025, 9, 18, 12, 0, 0, DateTimeKind.Utc),
                ReviewerNotes = "Cooperative structure creates unclear liability. Recommend individual entity applications.",
                CreatedAt = new DateTime(2025, 9, 3, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 9, 18, 12, 0, 0, DateTimeKind.Utc)
            }
        };

        builder.HasData(sampleLoans);
    }
}
