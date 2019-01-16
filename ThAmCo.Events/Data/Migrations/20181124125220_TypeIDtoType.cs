using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Events.Data.Migrations
{
    public partial class TypeIDtoType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeId",
                schema: "thamco.events",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                schema: "thamco.events",
                table: "Events",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "thamco.events",
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "Type",
                value: "Party");

            migrationBuilder.UpdateData(
                schema: "thamco.events",
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                column: "Type",
                value: "Wedding");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "thamco.events",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "TypeId",
                schema: "thamco.events",
                table: "Events",
                fixedLength: true,
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "thamco.events",
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "TypeId",
                value: "PTY");

            migrationBuilder.UpdateData(
                schema: "thamco.events",
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                column: "TypeId",
                value: "WED");
        }
    }
}
