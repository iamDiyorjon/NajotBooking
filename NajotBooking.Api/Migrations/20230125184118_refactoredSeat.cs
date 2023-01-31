using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NajotBooking.Api.Migrations
{
    /// <inheritdoc />
    public partial class refactoredSeat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBooked",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Seats");

            migrationBuilder.RenameColumn(
                name: "SeatNumber",
                table: "Seats",
                newName: "Number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Seats",
                newName: "SeatNumber");

            migrationBuilder.AddColumn<bool>(
                name: "IsBooked",
                table: "Seats",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Seats",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
