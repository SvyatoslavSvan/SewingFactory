using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeHierarchyReworked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeTaskRepeats_ProcessBasedEmployees_WorkShopEmployeeId",
                table: "EmployeeTaskRepeats");

            migrationBuilder.DropForeignKey(
                name: "FK_RateBasedEmployees_ProcessBasedEmployees_Id",
                table: "RateBasedEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_Technologists_ProcessBasedEmployees_Id",
                table: "Technologists");

            migrationBuilder.DropTable(
                name: "ProcessBasedEmployeeWorkshopDocument");

            migrationBuilder.AddColumn<decimal>(
                name: "Premium",
                table: "RateBasedEmployees",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "EmployeeWorkshopDocument",
                columns: table => new
                {
                    DocumentsId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeWorkshopDocument", x => new { x.DocumentsId, x.EmployeesId });
                    table.ForeignKey(
                        name: "FK_EmployeeWorkshopDocument_Employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeWorkshopDocument_WorkshopDocuments_DocumentsId",
                        column: x => x.DocumentsId,
                        principalTable: "WorkshopDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkshopDocument_EmployeesId",
                table: "EmployeeWorkshopDocument",
                column: "EmployeesId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeTaskRepeats_Employees_WorkShopEmployeeId",
                table: "EmployeeTaskRepeats",
                column: "WorkShopEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RateBasedEmployees_Employees_Id",
                table: "RateBasedEmployees",
                column: "Id",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Technologists_Employees_Id",
                table: "Technologists",
                column: "Id",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeTaskRepeats_Employees_WorkShopEmployeeId",
                table: "EmployeeTaskRepeats");

            migrationBuilder.DropForeignKey(
                name: "FK_RateBasedEmployees_Employees_Id",
                table: "RateBasedEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_Technologists_Employees_Id",
                table: "Technologists");

            migrationBuilder.DropTable(
                name: "EmployeeWorkshopDocument");

            migrationBuilder.DropColumn(
                name: "Premium",
                table: "RateBasedEmployees");

            migrationBuilder.CreateTable(
                name: "ProcessBasedEmployeeWorkshopDocument",
                columns: table => new
                {
                    DocumentsId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessBasedEmployeeWorkshopDocument", x => new { x.DocumentsId, x.EmployeesId });
                    table.ForeignKey(
                        name: "FK_ProcessBasedEmployeeWorkshopDocument_ProcessBasedEmployees_~",
                        column: x => x.EmployeesId,
                        principalTable: "ProcessBasedEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessBasedEmployeeWorkshopDocument_WorkshopDocuments_Docu~",
                        column: x => x.DocumentsId,
                        principalTable: "WorkshopDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessBasedEmployeeWorkshopDocument_EmployeesId",
                table: "ProcessBasedEmployeeWorkshopDocument",
                column: "EmployeesId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeTaskRepeats_ProcessBasedEmployees_WorkShopEmployeeId",
                table: "EmployeeTaskRepeats",
                column: "WorkShopEmployeeId",
                principalTable: "ProcessBasedEmployees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RateBasedEmployees_ProcessBasedEmployees_Id",
                table: "RateBasedEmployees",
                column: "Id",
                principalTable: "ProcessBasedEmployees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Technologists_ProcessBasedEmployees_Id",
                table: "Technologists",
                column: "Id",
                principalTable: "ProcessBasedEmployees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
