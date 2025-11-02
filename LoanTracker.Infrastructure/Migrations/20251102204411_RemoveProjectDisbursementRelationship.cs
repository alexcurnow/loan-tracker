using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProjectDisbursementRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // No action needed - Disbursements table was already dropped in RemoveDisbursementsTable migration
            // This migration just updates the EF Core model (removes Project.Disbursements navigation property)
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Disbursement",
                columns: table => new
                {
                    DisbursementId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    AmountCurrency = table.Column<string>(type: "text", nullable: false),
                    AmountValue = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DisbursementDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RecipientDetails = table.Column<string>(type: "text", nullable: false),
                    RecipientName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disbursement", x => x.DisbursementId);
                    table.ForeignKey(
                        name: "FK_Disbursement_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Disbursement_ProjectId",
                table: "Disbursement",
                column: "ProjectId");
        }
    }
}
