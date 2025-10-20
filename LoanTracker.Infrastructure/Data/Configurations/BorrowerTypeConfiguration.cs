using LoanTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanTracker.Infrastructure.Data.Configurations;

public class BorrowerTypeConfiguration : IEntityTypeConfiguration<BorrowerType>
{
    public void Configure(EntityTypeBuilder<BorrowerType> builder)
    {
        builder.ToTable("BorrowerTypes");

        builder.HasKey(bt => bt.BorrowerTypeId);

        builder.Property(bt => bt.TypeName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(bt => bt.Description)
            .HasMaxLength(500);

        // Seed data
        builder.HasData(
            new BorrowerType { BorrowerTypeId = 1, TypeName = "Municipality", Description = "City or town government entity" },
            new BorrowerType { BorrowerTypeId = 2, TypeName = "County", Description = "County government entity" },
            new BorrowerType { BorrowerTypeId = 3, TypeName = "School District", Description = "Public school district" },
            new BorrowerType { BorrowerTypeId = 4, TypeName = "University", Description = "Public university or college" },
            new BorrowerType { BorrowerTypeId = 5, TypeName = "Small Business", Description = "Small business entity" },
            new BorrowerType { BorrowerTypeId = 6, TypeName = "State Agency", Description = "State government agency" }
        );
    }
}
