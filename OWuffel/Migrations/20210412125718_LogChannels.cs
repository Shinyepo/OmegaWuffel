using Microsoft.EntityFrameworkCore.Migrations;

namespace OWuffel.Migrations
{
    public partial class LogChannels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "logChannelCreated",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "logChannelDeleted",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "logChannelUpdated",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "logChannelCreated",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "logChannelDeleted",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "logChannelUpdated",
                table: "Settings");
        }
    }
}
