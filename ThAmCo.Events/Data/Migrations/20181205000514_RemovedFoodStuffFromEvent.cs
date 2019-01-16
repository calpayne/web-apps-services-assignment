using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Events.Data.Migrations
{
    public partial class RemovedFoodStuffFromEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodMenu",
                schema: "thamco.events",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "FoodMenuRef",
                schema: "thamco.events",
                table: "Events");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FoodMenu",
                schema: "thamco.events",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FoodMenuRef",
                schema: "thamco.events",
                table: "Events",
                nullable: true);
        }
    }
}
