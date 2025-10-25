using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectsAndDisbursements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DueDay",
                table: "Loans",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Budget = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    BudgetCurrency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false, defaultValue: "USD"),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_Projects_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "LoanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Disbursements",
                columns: table => new
                {
                    DisbursementId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AmountCurrency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false, defaultValue: "USD"),
                    DisbursementDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "When funds were actually released (can be backdated)"),
                    RecipientName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RecipientDetails = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "When record was created in system")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disbursements", x => x.DisbursementId);
                    table.ForeignKey(
                        name: "FK_Disbursements_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "DueDay",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
                column: "DueDay",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111113"),
                column: "DueDay",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111114"),
                column: "DueDay",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111115"),
                column: "DueDay",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "DueDay",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "DueDay",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "DueDay",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222224"),
                column: "DueDay",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333331"),
                column: "DueDay",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333332"),
                column: "DueDay",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "DueDay",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444441"),
                column: "DueDay",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444442"),
                column: "DueDay",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444443"),
                column: "DueDay",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "DueDay",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444445"),
                column: "DueDay",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("55555555-5555-5555-5555-555555555551"),
                column: "DueDay",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("55555555-5555-5555-5555-555555555552"),
                column: "DueDay",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanId",
                keyValue: new Guid("55555555-5555-5555-5555-555555555553"),
                column: "DueDay",
                value: 20);

            migrationBuilder.CreateIndex(
                name: "IX_Disbursements_DisbursementDate",
                table: "Disbursements",
                column: "DisbursementDate");

            migrationBuilder.CreateIndex(
                name: "IX_Disbursements_ProjectId",
                table: "Disbursements",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_LoanId",
                table: "Projects",
                column: "LoanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Disbursements");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropColumn(
                name: "DueDay",
                table: "Loans");
        }
    }
}
