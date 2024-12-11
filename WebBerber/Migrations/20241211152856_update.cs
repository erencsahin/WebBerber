using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBerber.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Employees",
                table: "Shops");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Employees",
                table: "Shops",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
