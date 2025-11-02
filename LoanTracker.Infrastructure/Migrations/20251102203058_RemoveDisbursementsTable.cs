using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LoanTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDisbursementsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the Disbursements table entirely - disbursements are now event-sourced via Marten
            migrationBuilder.DropTable(
                name: "Disbursements");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disbursement_Projects_ProjectId",
                table: "Disbursement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Disbursement",
                table: "Disbursement");

            migrationBuilder.RenameTable(
                name: "Disbursement",
                newName: "Disbursements");

            migrationBuilder.RenameColumn(
                name: "AmountValue",
                table: "Disbursements",
                newName: "Amount");

            migrationBuilder.RenameIndex(
                name: "IX_Disbursement_ProjectId",
                table: "Disbursements",
                newName: "IX_Disbursements_ProjectId");

            migrationBuilder.AlterColumn<string>(
                name: "RecipientName",
                table: "Disbursements",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "RecipientDetails",
                table: "Disbursements",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DisbursementDate",
                table: "Disbursements",
                type: "timestamp with time zone",
                nullable: false,
                comment: "When funds were actually released (can be backdated)",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Disbursements",
                type: "timestamp with time zone",
                nullable: false,
                comment: "When record was created in system",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "AmountCurrency",
                table: "Disbursements",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "USD",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Disbursements",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Disbursements",
                table: "Disbursements",
                column: "DisbursementId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Disbursements_DisbursementDate",
                table: "Disbursements",
                column: "DisbursementDate");

            migrationBuilder.AddForeignKey(
                name: "FK_Disbursements_Projects_ProjectId",
                table: "Disbursements",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
