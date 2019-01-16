using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Events.Data.Migrations
{
    public partial class VenueCodetoVenue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VenueCode",
                schema: "thamco.events",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "Venue",
                schema: "thamco.events",
                table: "Events",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Venue",
                schema: "thamco.events",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "VenueCode",
                schema: "thamco.events",
                table: "Events",
                maxLength: 5,
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "thamco.events",
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "VenueCode",
                value: "CRKHL");

            migrationBuilder.UpdateData(
                schema: "thamco.events",
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                column: "VenueCode",
                value: "TNDMR");
        }
    }
}
