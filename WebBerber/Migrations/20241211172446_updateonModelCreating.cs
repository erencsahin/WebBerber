using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBerber.Migrations
{
    /// <inheritdoc />
    public partial class updateonModelCreating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_Employees_EmployeeId",
                table: "Operations");

            migrationBuilder.DropIndex(
                name: "IX_Operations_EmployeeId",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Operations");

            migrationBuilder.CreateTable(
                name: "EmployeeOperations",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    OperationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeOperations", x => new { x.EmployeeId, x.OperationId });
                    table.ForeignKey(
                        name: "FK_EmployeeOperations_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeOperations_Operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeOperations_OperationId",
                table: "EmployeeOperations",
                column: "OperationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeOperations");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Operations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operations_EmployeeId",
                table: "Operations",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_Employees_EmployeeId",
                table: "Operations",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
