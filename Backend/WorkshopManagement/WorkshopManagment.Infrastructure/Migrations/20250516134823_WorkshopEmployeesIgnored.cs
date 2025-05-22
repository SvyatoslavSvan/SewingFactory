using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WorkshopEmployeesIgnored : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessBasedEmployees_WorkshopTasks_WorkshopTaskId",
                table: "ProcessBasedEmployees");

            migrationBuilder.DropIndex(
                name: "IX_ProcessBasedEmployees_WorkshopTaskId",
                table: "ProcessBasedEmployees");

            migrationBuilder.DropColumn(
                name: "WorkshopTaskId",
                table: "ProcessBasedEmployees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkshopTaskId",
                table: "ProcessBasedEmployees",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessBasedEmployees_WorkshopTaskId",
                table: "ProcessBasedEmployees",
                column: "WorkshopTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessBasedEmployees_WorkshopTasks_WorkshopTaskId",
                table: "ProcessBasedEmployees",
                column: "WorkshopTaskId",
                principalTable: "WorkshopTasks",
                principalColumn: "Id");
        }
    }
}
