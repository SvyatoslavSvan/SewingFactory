using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TimesheetEmployeeManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timesheets_RateBasedEmployees_RateBasedEmployeeId",
                table: "Timesheets");

            migrationBuilder.DropIndex(
                name: "IX_Timesheets_RateBasedEmployeeId",
                table: "Timesheets");

            migrationBuilder.DropColumn(
                name: "RateBasedEmployeeId",
                table: "Timesheets");

            migrationBuilder.CreateTable(
                name: "RateBasedEmployeeTimesheet",
                columns: table => new
                {
                    EmployeesId = table.Column<Guid>(type: "uuid", nullable: false),
                    TimesheetsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateBasedEmployeeTimesheet", x => new { x.EmployeesId, x.TimesheetsId });
                    table.ForeignKey(
                        name: "FK_RateBasedEmployeeTimesheet_RateBasedEmployees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "RateBasedEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RateBasedEmployeeTimesheet_Timesheets_TimesheetsId",
                        column: x => x.TimesheetsId,
                        principalTable: "Timesheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RateBasedEmployeeTimesheet_TimesheetsId",
                table: "RateBasedEmployeeTimesheet",
                column: "TimesheetsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RateBasedEmployeeTimesheet");

            migrationBuilder.AddColumn<Guid>(
                name: "RateBasedEmployeeId",
                table: "Timesheets",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Timesheets_RateBasedEmployeeId",
                table: "Timesheets",
                column: "RateBasedEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timesheets_RateBasedEmployees_RateBasedEmployeeId",
                table: "Timesheets",
                column: "RateBasedEmployeeId",
                principalTable: "RateBasedEmployees",
                principalColumn: "Id");
        }
    }
}
