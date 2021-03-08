using Microsoft.EntityFrameworkCore.Migrations;

namespace OWuffel.Migrations
{
    public partial class logEmotesCUD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "logEmoteCreated",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "logEmoteDeleted",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "logEmoteUpdated",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "logEmoteCreated",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "logEmoteDeleted",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "logEmoteUpdated",
                table: "Settings");
        }
    }
}
