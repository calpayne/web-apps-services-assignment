using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Catering.Data.Migrations
{
    public partial class EventIdToBookings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventId",
                schema: "thamco.catering",
                table: "Bookings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventId",
                schema: "thamco.catering",
                table: "Bookings");
        }
    }
}
