using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Events.Data.Migrations
{
    public partial class FoodMenuRefToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FoodMenuRef",
                schema: "thamco.events",
                table: "Events",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FoodMenuRef",
                schema: "thamco.events",
                table: "Events",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
