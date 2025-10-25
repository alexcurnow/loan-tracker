using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LoanTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectDisbursementSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "Amount", "ApplicationDate", "BorrowerName", "CreatedAt", "DecisionDate", "Purpose", "ReviewerNotes", "Status", "UpdatedAt" },
                values: new object[] { 700000m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Metro Nashville Infrastructure", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 5, 12, 0, 0, 0, DateTimeKind.Utc), "Road repairs and bridge upgrades for city infrastructure", "Critical infrastructure needs. Approved for phased construction.", "Construction", new DateTime(2024, 2, 10, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
                columns: new[] { "Amount", "ApplicationDate", "BorrowerName", "BorrowerTypeId", "CreatedAt", "DecisionDate", "InterestRate", "Purpose", "ReviewerNotes", "Status", "UpdatedAt" },
                values: new object[] { 500000m, new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Shelby County Schools", 3, new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 5, 12, 0, 0, 0, DateTimeKind.Utc), 2.5m, "New classroom wing construction", "Addresses critical overcrowding. Strong enrollment projections.", "Construction", new DateTime(2024, 2, 15, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111113"),
                columns: new[] { "Amount", "ApplicationDate", "BorrowerName", "BorrowerTypeId", "ContactEmail", "CreatedAt", "DecisionDate", "InterestRate", "Purpose", "ReviewerNotes", "Status", "TermYears", "UpdatedAt" },
                values: new object[] { 600000m, new DateTime(2024, 3, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Knox County Fire Department", 2, "rwilliams@knoxcounty.gov", new DateTime(2024, 3, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 10, 12, 0, 0, 0, DateTimeKind.Utc), 3.0m, "New fire station and equipment", "Approved. Waiting for contractor selection before first disbursement.", "Approved", 15, new DateTime(2024, 3, 10, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "Amount", "ApplicationDate", "BorrowerName", "CreatedAt", "Purpose", "Status", "UpdatedAt" },
                values: new object[] { 400000m, new DateTime(2024, 4, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Davidson County Library", new DateTime(2024, 4, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Library system modernization and technology upgrades", "AwaitingReview", new DateTime(2024, 4, 5, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "ProjectId", "Budget", "BudgetCurrency", "CreatedAt", "Description", "LoanId", "ProjectName", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), 400000m, "USD", new DateTime(2024, 1, 10, 12, 0, 0, 0, DateTimeKind.Utc), "Repair and repaving of major downtown thoroughfares", new Guid("11111111-1111-1111-1111-111111111111"), "Road Repairs", new DateTime(2024, 1, 10, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaabbb"), 300000m, "USD", new DateTime(2024, 1, 10, 12, 0, 0, 0, DateTimeKind.Utc), "Structural reinforcement and safety improvements for city bridges", new Guid("11111111-1111-1111-1111-111111111111"), "Bridge Upgrades", new DateTime(2024, 1, 10, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 500000m, "USD", new DateTime(2024, 2, 5, 12, 0, 0, 0, DateTimeKind.Utc), "Construction of new classroom wing to address overcrowding", new Guid("11111111-1111-1111-1111-111111111112"), "New Classroom Wing", new DateTime(2024, 2, 5, 12, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Disbursements",
                columns: new[] { "DisbursementId", "AmountCurrency", "Amount", "CreatedAt", "DisbursementDate", "ProjectId", "RecipientDetails", "RecipientName" },
                values: new object[,]
                {
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddd01"), "USD", 200000m, new DateTime(2024, 1, 20, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 15, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Initial payment for road repairs phase 1 - Main St and Broadway", "Metro Road Construction LLC" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddd02"), "USD", 100000m, new DateTime(2024, 2, 8, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 5, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Second payment for road repairs phase 2 - West End Ave", "Metro Road Construction LLC" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddd03"), "USD", 150000m, new DateTime(2024, 2, 10, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 10, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaabbb"), "Initial payment for structural reinforcement of Jefferson St Bridge", "Cumberland Bridge Engineering" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddd04"), "USD", 250000m, new DateTime(2024, 2, 15, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 15, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Foundation and site preparation for new classroom wing", "Shelby Construction Group" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Disbursements",
                keyColumn: "DisbursementId",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddd01"));

            migrationBuilder.DeleteData(
                table: "Disbursements",
                keyColumn: "DisbursementId",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddd02"));

            migrationBuilder.DeleteData(
                table: "Disbursements",
                keyColumn: "DisbursementId",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddd03"));

            migrationBuilder.DeleteData(
                table: "Disbursements",
                keyColumn: "DisbursementId",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddd04"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "ProjectId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "ProjectId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaabbb"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "ProjectId",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "Amount", "ApplicationDate", "BorrowerName", "CreatedAt", "DecisionDate", "Purpose", "ReviewerNotes", "Status", "UpdatedAt" },
                values: new object[] { 1500000m, new DateTime(2025, 10, 9, 12, 0, 0, 0, DateTimeKind.Utc), "City of Nashville", new DateTime(2025, 10, 9, 12, 0, 0, 0, DateTimeKind.Utc), null, "Infrastructure improvements for downtown area", null, "Open", new DateTime(2025, 10, 9, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
                columns: new[] { "Amount", "ApplicationDate", "BorrowerName", "BorrowerTypeId", "CreatedAt", "DecisionDate", "InterestRate", "Purpose", "ReviewerNotes", "Status", "UpdatedAt" },
                values: new object[] { 2000000m, new DateTime(2025, 10, 11, 12, 0, 0, 0, DateTimeKind.Utc), "Shelby County", 2, new DateTime(2025, 10, 11, 12, 0, 0, 0, DateTimeKind.Utc), null, 3.75m, "New county courthouse construction", null, "Open", new DateTime(2025, 10, 11, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111113"),
                columns: new[] { "Amount", "ApplicationDate", "BorrowerName", "BorrowerTypeId", "ContactEmail", "CreatedAt", "DecisionDate", "InterestRate", "Purpose", "ReviewerNotes", "Status", "TermYears", "UpdatedAt" },
                values: new object[] { 3000000m, new DateTime(2025, 10, 14, 12, 0, 0, 0, DateTimeKind.Utc), "Knox County School District", 3, "rwilliams@knoxschools.org", new DateTime(2025, 10, 14, 12, 0, 0, 0, DateTimeKind.Utc), null, 2.5m, "New elementary school construction", null, "Open", 25, new DateTime(2025, 10, 14, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "Amount", "ApplicationDate", "BorrowerName", "CreatedAt", "Purpose", "Status", "UpdatedAt" },
                values: new object[] { 900000m, new DateTime(2025, 9, 14, 12, 0, 0, 0, DateTimeKind.Utc), "Davidson County", new DateTime(2025, 9, 14, 12, 0, 0, 0, DateTimeKind.Utc), "County library system modernization", "ApprovalPending", new DateTime(2025, 9, 24, 12, 0, 0, 0, DateTimeKind.Utc) });
        }
    }
}
