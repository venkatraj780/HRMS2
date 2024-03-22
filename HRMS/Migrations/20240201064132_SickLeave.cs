using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Migrations
{
    /// <inheritdoc />
    public partial class SickLeave : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SickLeaveReplacedEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaveId = table.Column<int>(type: "int", nullable: false),
                    EmpIdOnLeave = table.Column<int>(type: "int", nullable: false),
                    EmpReplcedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SickLeaveReplacedEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SickLeaveReplacedEmployees_Employees_EmpIdOnLeave",
                        column: x => x.EmpIdOnLeave,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SickLeaveReplacedEmployees_Employees_EmpReplcedId",
                        column: x => x.EmpReplcedId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SickLeaveReplacedEmployees_Leaves_LeaveId",
                        column: x => x.LeaveId,
                        principalTable: "Leaves",
                        principalColumn: "LeaveId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SickLeaveReplacedEmployees_EmpIdOnLeave",
                table: "SickLeaveReplacedEmployees",
                column: "EmpIdOnLeave",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SickLeaveReplacedEmployees_EmpReplcedId",
                table: "SickLeaveReplacedEmployees",
                column: "EmpReplcedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SickLeaveReplacedEmployees_LeaveId",
                table: "SickLeaveReplacedEmployees",
                column: "LeaveId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SickLeaveReplacedEmployees");
        }
    }
}
