using Microsoft.EntityFrameworkCore.Migrations;

namespace Timesheet.DataAccess.Npgsql.Migrations
{
    public partial class edittimelogremoveemployeefk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeLogs_Employees_EmployeeId",
                table: "TimeLogs");

            migrationBuilder.DropIndex(
                name: "IX_TimeLogs_EmployeeId",
                table: "TimeLogs");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "TimeLogs");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeLogin",
                table: "TimeLogs",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeLogin",
                table: "TimeLogs");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "TimeLogs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeLogs_EmployeeId",
                table: "TimeLogs",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLogs_Employees_EmployeeId",
                table: "TimeLogs",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
