using Microsoft.EntityFrameworkCore.Migrations;

namespace OWuffel.Migrations
{
    public partial class logCleanupFinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "logVoiceStateUpdates",
                table: "Settings",
                newName: "logVoiceStateUpdated");

            migrationBuilder.RenameColumn(
                name: "logUserUpdates",
                table: "Settings",
                newName: "logUserUpdated");

            migrationBuilder.RenameColumn(
                name: "logUserJoins",
                table: "Settings",
                newName: "logUserUnbanned");

            migrationBuilder.RenameColumn(
                name: "logUserBans",
                table: "Settings",
                newName: "logUserLeft");

            migrationBuilder.RenameColumn(
                name: "logRoleEvents",
                table: "Settings",
                newName: "logUserJoined");

            migrationBuilder.RenameColumn(
                name: "logMessageEvents",
                table: "Settings",
                newName: "logUserBanned");

            migrationBuilder.RenameColumn(
                name: "logGuildEvents",
                table: "Settings",
                newName: "logRoleUpdated");

            migrationBuilder.AddColumn<ulong>(
                name: "logMessageDeleted",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "logMessageUpdated",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "logRoleCreated",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "logRoleDeleted",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "logMessageDeleted",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "logMessageUpdated",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "logRoleCreated",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "logRoleDeleted",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "logVoiceStateUpdated",
                table: "Settings",
                newName: "logVoiceStateUpdates");

            migrationBuilder.RenameColumn(
                name: "logUserUpdated",
                table: "Settings",
                newName: "logUserUpdates");

            migrationBuilder.RenameColumn(
                name: "logUserUnbanned",
                table: "Settings",
                newName: "logUserJoins");

            migrationBuilder.RenameColumn(
                name: "logUserLeft",
                table: "Settings",
                newName: "logUserBans");

            migrationBuilder.RenameColumn(
                name: "logUserJoined",
                table: "Settings",
                newName: "logRoleEvents");

            migrationBuilder.RenameColumn(
                name: "logUserBanned",
                table: "Settings",
                newName: "logMessageEvents");

            migrationBuilder.RenameColumn(
                name: "logRoleUpdated",
                table: "Settings",
                newName: "logGuildEvents");
        }
    }
}
