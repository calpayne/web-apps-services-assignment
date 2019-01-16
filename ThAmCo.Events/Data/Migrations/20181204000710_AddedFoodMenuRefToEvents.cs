using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Events.Data.Migrations
{
    public partial class AddedFoodMenuRefToEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FoodMenuRef",
                schema: "thamco.events",
                table: "Events",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FoodMenuGetDTO",
                schema: "thamco.events",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MenuName = table.Column<string>(nullable: true),
                    MenuDescription = table.Column<string>(nullable: true),
                    People = table.Column<int>(nullable: false),
                    MenuCost = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodMenuGetDTO", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodMenuGetDTO",
                schema: "thamco.events");

            migrationBuilder.DropColumn(
                name: "FoodMenuRef",
                schema: "thamco.events",
                table: "Events");
        }
    }
}
