using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Events.Data.Migrations
{
    public partial class AddedStaffAndStaffBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Staff",
                schema: "thamco.events",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Surname = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    FirstAider = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaffBooking",
                schema: "thamco.events",
                columns: table => new
                {
                    StaffId = table.Column<int>(nullable: false),
                    EventId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffBooking", x => new { x.StaffId, x.EventId });
                    table.UniqueConstraint("AK_StaffBooking_EventId_StaffId", x => new { x.EventId, x.StaffId });
                    table.ForeignKey(
                        name: "FK_StaffBooking_Events_EventId",
                        column: x => x.EventId,
                        principalSchema: "thamco.events",
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffBooking_Staff_StaffId",
                        column: x => x.StaffId,
                        principalSchema: "thamco.events",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "thamco.events",
                table: "Staff",
                columns: new[] { "Id", "FirstAider", "FirstName", "Surname" },
                values: new object[] { 1, true, "Callum", "Payne" });

            migrationBuilder.InsertData(
                schema: "thamco.events",
                table: "Staff",
                columns: new[] { "Id", "FirstAider", "FirstName", "Surname" },
                values: new object[] { 2, false, "Ethan", "Payne" });

            migrationBuilder.InsertData(
                schema: "thamco.events",
                table: "StaffBooking",
                columns: new[] { "StaffId", "EventId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                schema: "thamco.events",
                table: "StaffBooking",
                columns: new[] { "StaffId", "EventId" },
                values: new object[] { 1, 2 });

            migrationBuilder.InsertData(
                schema: "thamco.events",
                table: "StaffBooking",
                columns: new[] { "StaffId", "EventId" },
                values: new object[] { 2, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StaffBooking",
                schema: "thamco.events");

            migrationBuilder.DropTable(
                name: "Staff",
                schema: "thamco.events");
        }
    }
}
