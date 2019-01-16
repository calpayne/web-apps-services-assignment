using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Catering.Data.Migrations
{
    public partial class StringPKToBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                schema: "thamco.catering",
                table: "Bookings");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Bookings_Id",
                schema: "thamco.catering",
                table: "Bookings");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "thamco.catering",
                table: "Bookings",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                schema: "thamco.catering",
                table: "Bookings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_FoodMenuId",
                schema: "thamco.catering",
                table: "Bookings",
                column: "FoodMenuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                schema: "thamco.catering",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_FoodMenuId",
                schema: "thamco.catering",
                table: "Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "thamco.catering",
                table: "Bookings",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                schema: "thamco.catering",
                table: "Bookings",
                column: "FoodMenuId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Bookings_Id",
                schema: "thamco.catering",
                table: "Bookings",
                column: "Id");
        }
    }
}
