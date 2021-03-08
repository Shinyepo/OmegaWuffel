using Microsoft.EntityFrameworkCore.Migrations;

namespace OWuffel.Migrations
{
    public partial class cleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "botClearBotMessagesDelay",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "logBlockedByUser",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "logChannelBanModule",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "logChannelMemberRemoveModule",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "logChannelMessageIngored",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "logChannelMessageModule",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "logChannelReact",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "welcomeEnabled",
                table: "Settings",
                newName: "logMessageEvents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "logMessageEvents",
                table: "Settings",
                newName: "welcomeEnabled");

            migrationBuilder.AddColumn<int>(
                name: "botClearBotMessagesDelay",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<ulong>(
                name: "logBlockedByUser",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "logChannelBanModule",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "logChannelMemberRemoveModule",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "logChannelMessageIngored",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "logChannelMessageModule",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "logChannelReact",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);
        }
    }
}
