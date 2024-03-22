using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInEmpSkill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmpSkills_Employees_EmployeeId",
                table: "EmpSkills");

            migrationBuilder.DropIndex(
                name: "IX_EmpSkills_EmployeeId",
                table: "EmpSkills");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "EmpSkills");

            migrationBuilder.CreateIndex(
                name: "IX_EmpSkills_EmpId",
                table: "EmpSkills",
                column: "EmpId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmpSkills_Employees_EmpId",
                table: "EmpSkills",
                column: "EmpId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmpSkills_Employees_EmpId",
                table: "EmpSkills");

            migrationBuilder.DropIndex(
                name: "IX_EmpSkills_EmpId",
                table: "EmpSkills");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "EmpSkills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EmpSkills_EmployeeId",
                table: "EmpSkills",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmpSkills_Employees_EmployeeId",
                table: "EmpSkills",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
