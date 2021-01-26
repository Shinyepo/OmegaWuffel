using Microsoft.EntityFrameworkCore.Migrations;

namespace OWuffel.Migrations
{
    public partial class RebuildPart1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "botDisabled",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "botEmbedColor",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "botFooter",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "welcomeMessageThumbnail",
                table: "Settings",
                newName: "botDisabledCommands");

            migrationBuilder.AddColumn<ulong>(
                name: "logGuildEvents",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "logUserEvents",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "logVoiceStateUpdate",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "logGuildEvents",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "logUserEvents",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "logVoiceStateUpdate",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "botDisabledCommands",
                table: "Settings",
                newName: "welcomeMessageThumbnail");

            migrationBuilder.AddColumn<string>(
                name: "botDisabled",
                table: "Settings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "botEmbedColor",
                table: "Settings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "botFooter",
                table: "Settings",
                type: "TEXT",
                nullable: true);
        }
    }
}
