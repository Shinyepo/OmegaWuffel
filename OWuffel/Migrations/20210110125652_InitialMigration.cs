using Microsoft.EntityFrameworkCore.Migrations;

namespace OWuffel.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    guild_id = table.Column<ulong>(type: "INTEGER", nullable: false),
                    botActive = table.Column<int>(type: "INTEGER", nullable: false),
                    botPrefix = table.Column<string>(type: "TEXT", nullable: true),
                    botModRole = table.Column<ulong>(type: "INTEGER", nullable: false),
                    botAdminRole = table.Column<ulong>(type: "INTEGER", nullable: false),
                    botEmbedColor = table.Column<string>(type: "TEXT", nullable: true),
                    botFooter = table.Column<string>(type: "TEXT", nullable: true),
                    botSystemNotice = table.Column<bool>(type: "INTEGER", nullable: false),
                    botClearMessage = table.Column<bool>(type: "INTEGER", nullable: false),
                    botClearMessagesForBot = table.Column<bool>(type: "INTEGER", nullable: false),
                    botClearBotMessagesDelay = table.Column<int>(type: "INTEGER", nullable: false),
                    botMuteRole = table.Column<ulong>(type: "INTEGER", nullable: false),
                    botLanguage = table.Column<string>(type: "TEXT", nullable: true),
                    welcomeChannel = table.Column<ulong>(type: "INTEGER", nullable: false),
                    welcomeEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    welcomeMessageThumbnail = table.Column<string>(type: "TEXT", nullable: true),
                    welcomeMessageAutoDelete = table.Column<bool>(type: "INTEGER", nullable: false),
                    logChannelMessageModule = table.Column<ulong>(type: "INTEGER", nullable: false),
                    logChannelBanModule = table.Column<ulong>(type: "INTEGER", nullable: false),
                    logChannelMemberRemoveModule = table.Column<ulong>(type: "INTEGER", nullable: false),
                    logBlockedByUser = table.Column<ulong>(type: "INTEGER", nullable: false),
                    logChannelMessageIngored = table.Column<ulong>(type: "INTEGER", nullable: false),
                    logChannelReact = table.Column<ulong>(type: "INTEGER", nullable: false),
                    botDisabled = table.Column<string>(type: "TEXT", nullable: true),
                    reactServerChannel = table.Column<ulong>(type: "INTEGER", nullable: false),
                    reactServerMessage = table.Column<ulong>(type: "INTEGER", nullable: false),
                    reactServerParent = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
