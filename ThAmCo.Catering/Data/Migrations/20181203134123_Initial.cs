using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Catering.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "thamco.catering");

            migrationBuilder.CreateTable(
                name: "FoodMenus",
                schema: "thamco.catering",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MenuName = table.Column<string>(nullable: false),
                    MenuDescription = table.Column<string>(nullable: false),
                    People = table.Column<int>(nullable: false),
                    MenuCost = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodMenus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                schema: "thamco.catering",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    EventDate = table.Column<DateTime>(nullable: false),
                    WhenMade = table.Column<DateTime>(nullable: false),
                    FoodMenuId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.FoodMenuId);
                    table.UniqueConstraint("AK_Bookings_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_FoodMenus_FoodMenuId",
                        column: x => x.FoodMenuId,
                        principalSchema: "thamco.catering",
                        principalTable: "FoodMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "thamco.catering",
                table: "FoodMenus",
                columns: new[] { "Id", "MenuCost", "MenuDescription", "MenuName", "People" },
                values: new object[,]
                {
                    { 1, 24.99, "Honey-roast ham with wholegrain mustard, Chicken Caesar salad sandwiches", "Sandwiches Buffet", 10 },
                    { 2, 24.99, "Roast tomato and herb quiche, Smoked haddock & spring onion fishcake, Chicken fillet skewers with sweet chilli dipping sauce", "Finger Buffet", 10 },
                    { 3, 24.99, "Mini strawberry and cream pavlova, Fresh fruit skewer with chocolate dipping sauce", "Mini Desserts", 10 },
                    { 4, 24.99, "Slider burger platter with pickles and sauces, Fish finger sandwich with tartare sauce", "Late-night Snacks", 10 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings",
                schema: "thamco.catering");

            migrationBuilder.DropTable(
                name: "FoodMenus",
                schema: "thamco.catering");
        }
    }
}
