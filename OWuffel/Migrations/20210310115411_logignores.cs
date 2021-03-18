using Microsoft.EntityFrameworkCore.Migrations;

namespace OWuffel.Migrations
{
    public partial class logignores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "logIgnoreMessageDeleted",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "logIgnoreMessageUpdated",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "logIgnoreVoiceStateUpdated",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.CreateIndex(
                name: "IX_Settings_guild_id",
                table: "Settings",
                column: "guild_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Settings_guild_id",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "logIgnoreMessageDeleted",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "logIgnoreMessageUpdated",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "logIgnoreVoiceStateUpdated",
                table: "Settings");
        }
    }
}
