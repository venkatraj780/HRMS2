using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Migrations
{
    /// <inheritdoc />
    public partial class SickLeaveReplacedEmployeeChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SickLeaveReplacedEmployees_EmpReplcedId",
                table: "SickLeaveReplacedEmployees");

            migrationBuilder.CreateIndex(
                name: "IX_SickLeaveReplacedEmployees_EmpReplcedId",
                table: "SickLeaveReplacedEmployees",
                column: "EmpReplcedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SickLeaveReplacedEmployees_EmpReplcedId",
                table: "SickLeaveReplacedEmployees");

            migrationBuilder.CreateIndex(
                name: "IX_SickLeaveReplacedEmployees_EmpReplcedId",
                table: "SickLeaveReplacedEmployees",
                column: "EmpReplcedId",
                unique: true);
        }
    }
}
