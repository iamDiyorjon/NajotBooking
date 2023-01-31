using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NajotBooking.Api.Migrations
{
    /// <inheritdoc />
    public partial class BranchAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Branch",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Branch",
                table: "Seats");
        }
    }
}
