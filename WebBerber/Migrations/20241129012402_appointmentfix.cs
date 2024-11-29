using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBerber.Migrations
{
    /// <inheritdoc />
    public partial class appointmentfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Appointments",
                newName: "StartTime");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Appointments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Appointments",
                newName: "Date");
        }
    }
}
