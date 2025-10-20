using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LoanTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BorrowerTypes",
                columns: table => new
                {
                    BorrowerTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowerTypes", x => x.BorrowerTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    LoanId = table.Column<Guid>(type: "uuid", nullable: false),
                    BorrowerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    BorrowerTypeId = table.Column<int>(type: "integer", nullable: false),
                    ContactPerson = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ContactEmail = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    InterestRate = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    TermYears = table.Column<int>(type: "integer", nullable: false),
                    Purpose = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    DecisionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReviewerNotes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.LoanId);
                    table.ForeignKey(
                        name: "FK_Loans_BorrowerTypes_BorrowerTypeId",
                        column: x => x.BorrowerTypeId,
                        principalTable: "BorrowerTypes",
                        principalColumn: "BorrowerTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "BorrowerTypes",
                columns: new[] { "BorrowerTypeId", "Description", "TypeName" },
                values: new object[,]
                {
                    { 1, "City or town government entity", "Municipality" },
                    { 2, "County government entity", "County" },
                    { 3, "Public school district", "School District" },
                    { 4, "Public university or college", "University" },
                    { 5, "Small business entity", "Small Business" },
                    { 6, "State government agency", "State Agency" }
                });

            migrationBuilder.InsertData(
                table: "Loans",
                columns: new[] { "LoanId", "Amount", "ApplicationDate", "BorrowerName", "BorrowerTypeId", "ContactEmail", "ContactPerson", "CreatedAt", "DecisionDate", "InterestRate", "Purpose", "ReviewerNotes", "Status", "TermYears", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 1500000m, new DateTime(2025, 10, 9, 12, 0, 0, 0, DateTimeKind.Utc), "City of Nashville", 1, "jsmith@nashville.gov", "John Smith", new DateTime(2025, 10, 9, 12, 0, 0, 0, DateTimeKind.Utc), null, 3.5m, "Infrastructure improvements for downtown area", null, "Open", 15, new DateTime(2025, 10, 9, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11111111-1111-1111-1111-111111111112"), 2000000m, new DateTime(2025, 10, 11, 12, 0, 0, 0, DateTimeKind.Utc), "Shelby County", 2, "mjohnson@shelbycounty.gov", "Mary Johnson", new DateTime(2025, 10, 11, 12, 0, 0, 0, DateTimeKind.Utc), null, 3.75m, "New county courthouse construction", null, "Open", 20, new DateTime(2025, 10, 11, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11111111-1111-1111-1111-111111111113"), 3000000m, new DateTime(2025, 10, 14, 12, 0, 0, 0, DateTimeKind.Utc), "Knox County School District", 3, "rwilliams@knoxschools.org", "Robert Williams", new DateTime(2025, 10, 14, 12, 0, 0, 0, DateTimeKind.Utc), null, 2.5m, "New elementary school construction", null, "Open", 25, new DateTime(2025, 10, 14, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11111111-1111-1111-1111-111111111114"), 500000m, new DateTime(2025, 10, 16, 12, 0, 0, 0, DateTimeKind.Utc), "Tennessee Tech Small Business", 5, "sdavis@tntech.com", "Sarah Davis", new DateTime(2025, 10, 16, 12, 0, 0, 0, DateTimeKind.Utc), null, 4.5m, "Equipment purchase and facility expansion", null, "Open", 10, new DateTime(2025, 10, 16, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11111111-1111-1111-1111-111111111115"), 4000000m, new DateTime(2025, 10, 18, 12, 0, 0, 0, DateTimeKind.Utc), "City of Memphis", 1, "jbrown@memphis.gov", "James Brown", new DateTime(2025, 10, 18, 12, 0, 0, 0, DateTimeKind.Utc), null, 3.25m, "Public transportation infrastructure upgrades", null, "Open", 30, new DateTime(2025, 10, 18, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222221"), 5000000m, new DateTime(2025, 9, 24, 12, 0, 0, 0, DateTimeKind.Utc), "University of Tennessee", 4, "pgarcia@utk.edu", "Patricia Garcia", new DateTime(2025, 9, 24, 12, 0, 0, 0, DateTimeKind.Utc), null, 2.75m, "Research facility construction and equipment", null, "AwaitingReview", 30, new DateTime(2025, 9, 29, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 1200000m, new DateTime(2025, 9, 27, 12, 0, 0, 0, DateTimeKind.Utc), "Hamilton County", 2, "mmartinez@hamiltoncounty.gov", "Michael Martinez", new DateTime(2025, 9, 27, 12, 0, 0, 0, DateTimeKind.Utc), null, 3.5m, "Emergency services facility renovation", null, "AwaitingReview", 15, new DateTime(2025, 10, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222223"), 8000000m, new DateTime(2025, 9, 30, 12, 0, 0, 0, DateTimeKind.Utc), "Tennessee Department of Transportation", 6, "lrodriguez@tn.gov", "Linda Rodriguez", new DateTime(2025, 9, 30, 12, 0, 0, 0, DateTimeKind.Utc), null, 3.0m, "Highway expansion and bridge repairs", null, "AwaitingReview", 25, new DateTime(2025, 10, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222224"), 2500000m, new DateTime(2025, 10, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Metro Nashville School District", 3, "dwilson@mnps.org", "David Wilson", new DateTime(2025, 10, 3, 12, 0, 0, 0, DateTimeKind.Utc), null, 2.25m, "Technology infrastructure upgrade across all schools", null, "AwaitingReview", 20, new DateTime(2025, 10, 7, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333331"), 1800000m, new DateTime(2025, 9, 9, 12, 0, 0, 0, DateTimeKind.Utc), "City of Knoxville", 1, "banderson@knoxville.gov", "Barbara Anderson", new DateTime(2025, 9, 9, 12, 0, 0, 0, DateTimeKind.Utc), null, 3.25m, "Public park development and recreation facilities", null, "ApprovalPending", 18, new DateTime(2025, 9, 19, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333332"), 3500000m, new DateTime(2025, 9, 11, 12, 0, 0, 0, DateTimeKind.Utc), "Middle Tennessee State University", 4, "cthomas@mtsu.edu", "Christopher Thomas", new DateTime(2025, 9, 11, 12, 0, 0, 0, DateTimeKind.Utc), null, 2.5m, "Student housing complex construction", null, "ApprovalPending", 25, new DateTime(2025, 9, 21, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 900000m, new DateTime(2025, 9, 14, 12, 0, 0, 0, DateTimeKind.Utc), "Davidson County", 2, "jtaylor@nashville.gov", "Jessica Taylor", new DateTime(2025, 9, 14, 12, 0, 0, 0, DateTimeKind.Utc), null, 3.75m, "County library system modernization", null, "ApprovalPending", 12, new DateTime(2025, 9, 24, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("44444444-4444-4444-4444-444444444441"), 2200000m, new DateTime(2025, 8, 20, 12, 0, 0, 0, DateTimeKind.Utc), "City of Chattanooga", 1, "dwhite@chattanooga.gov", "Daniel White", new DateTime(2025, 8, 20, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 4, 12, 0, 0, 0, DateTimeKind.Utc), 3.0m, "Downtown revitalization project", "Excellent project proposal with strong community support. Approved for full amount requested.", "Approved", 20, new DateTime(2025, 9, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("44444444-4444-4444-4444-444444444442"), 4500000m, new DateTime(2025, 8, 25, 12, 0, 0, 0, DateTimeKind.Utc), "Williamson County School District", 3, "kharris@wcsdistrict.com", "Karen Harris", new DateTime(2025, 8, 25, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 9, 12, 0, 0, 0, DateTimeKind.Utc), 2.25m, "New high school construction", "Critical need demonstrated. Project aligns with state education goals.", "Approved", 30, new DateTime(2025, 9, 9, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("44444444-4444-4444-4444-444444444443"), 1000000m, new DateTime(2025, 8, 30, 12, 0, 0, 0, DateTimeKind.Utc), "Tennessee Department of Environment", 6, "smartin@tn.gov", "Steven Martin", new DateTime(2025, 8, 30, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 14, 12, 0, 0, 0, DateTimeKind.Utc), 2.75m, "Water treatment facility upgrades", "Essential environmental infrastructure project. Approved.", "Approved", 15, new DateTime(2025, 9, 14, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 2800000m, new DateTime(2025, 9, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Austin Peay State University", 4, "njackson@apsu.edu", "Nancy Jackson", new DateTime(2025, 9, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 16, 12, 0, 0, 0, DateTimeKind.Utc), 2.5m, "Science building renovation and laboratory upgrades", "Strong academic justification. Supports STEM education initiatives.", "Approved", 25, new DateTime(2025, 9, 16, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("44444444-4444-4444-4444-444444444445"), 1600000m, new DateTime(2025, 9, 4, 12, 0, 0, 0, DateTimeKind.Utc), "Rutherford County", 2, "bthompson@rutherfordcounty.gov", "Brian Thompson", new DateTime(2025, 9, 4, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 19, 12, 0, 0, 0, DateTimeKind.Utc), 3.5m, "Public safety communication system upgrade", "Critical public safety need. Approved for full funding.", "Approved", 18, new DateTime(2025, 9, 19, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-5555-5555-5555-555555555551"), 750000m, new DateTime(2025, 8, 28, 12, 0, 0, 0, DateTimeKind.Utc), "Small Town Manufacturing LLC", 5, "klee@stmfg.com", "Kevin Lee", new DateTime(2025, 8, 28, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 12, 12, 0, 0, 0, DateTimeKind.Utc), 5.5m, "Factory expansion and new equipment", "Insufficient collateral and questionable financial projections. Recommend reapplication with revised business plan.", "Denied", 10, new DateTime(2025, 9, 12, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-5555-5555-5555-555555555552"), 5500000m, new DateTime(2025, 8, 31, 12, 0, 0, 0, DateTimeKind.Utc), "City of Springfield", 1, "mclark@springfield.gov", "Michelle Clark", new DateTime(2025, 8, 31, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 15, 12, 0, 0, 0, DateTimeKind.Utc), 4.0m, "Sports complex and convention center", "Project scope exceeds municipality's debt capacity. Insufficient revenue projections to support debt service.", "Denied", 25, new DateTime(2025, 9, 15, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-5555-5555-5555-555555555553"), 350000m, new DateTime(2025, 9, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Regional Small Business Co-op", 5, "amoore@regionalbiz.com", "Angela Moore", new DateTime(2025, 9, 3, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 18, 12, 0, 0, 0, DateTimeKind.Utc), 6.0m, "Shared warehouse facility", "Cooperative structure creates unclear liability. Recommend individual entity applications.", "Denied", 8, new DateTime(2025, 9, 18, 12, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loans_BorrowerTypeId",
                table: "Loans",
                column: "BorrowerTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "BorrowerTypes");
        }
    }
}
