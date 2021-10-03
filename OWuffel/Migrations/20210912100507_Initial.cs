using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace OWuffel.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GuildInformations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    GuildName = table.Column<string>(type: "text", nullable: true),
                    GuildAvatar = table.Column<string>(type: "text", nullable: true),
                    GuildOwnerId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    GuildOwnerName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildInformations", x => x.Id);
                    table.UniqueConstraint("AK_GuildInformations_GuildId", x => x.GuildId);
                });

            migrationBuilder.CreateTable(
                name: "BotSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    BotActive = table.Column<bool>(type: "boolean", nullable: false),
                    BotPrefix = table.Column<string>(type: "text", nullable: true),
                    BotModRole = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    BotAdminRole = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    BotDisabledCommands = table.Column<string>(type: "text", nullable: true),
                    BotSystemNotice = table.Column<bool>(type: "boolean", nullable: false),
                    BotClearMessage = table.Column<bool>(type: "boolean", nullable: false),
                    BotClearMessagesForBot = table.Column<bool>(type: "boolean", nullable: false),
                    BotMuteRole = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    BotLanguage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BotSettings_GuildInformations_GuildId",
                        column: x => x.GuildId,
                        principalTable: "GuildInformations",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyRankingConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    IgnoreId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyRankingConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyRankingConfig_GuildInformations_GuildId",
                        column: x => x.GuildId,
                        principalTable: "GuildInformations",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    LogVoicePresencePowerSwitch = table.Column<bool>(type: "boolean", nullable: false),
                    LogVoiceStateUpdated = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    LogIgnoreVoiceStateUpdated = table.Column<string>(type: "text", nullable: true),
                    LogUserMovementPowerSwitch = table.Column<bool>(type: "boolean", nullable: false),
                    LogUserMovement = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    LogUserUpdatedPowerSwitch = table.Column<bool>(type: "boolean", nullable: false),
                    LogUserUpdated = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    LogRolePowerSwtich = table.Column<bool>(type: "boolean", nullable: false),
                    LogRoleEvents = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    LogMessageEventsPowerSwitch = table.Column<bool>(type: "boolean", nullable: false),
                    LogMessageEvents = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    LogIgnoreMessageEvents = table.Column<string>(type: "text", nullable: true),
                    LogEmotePowerSwitch = table.Column<bool>(type: "boolean", nullable: false),
                    LogEmoteEvents = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    LogGuildUpdated = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    LogChannelPowerSwitch = table.Column<bool>(type: "boolean", nullable: false),
                    LogChannelEvents = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_GuildInformations_GuildId",
                        column: x => x.GuildId,
                        principalTable: "GuildInformations",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReactionsConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ReactServerChannel = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ReactServerMessage = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ReactServerParent = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionsConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReactionsConfigs_GuildInformations_GuildId",
                        column: x => x.GuildId,
                        principalTable: "GuildInformations",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Suggestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Author = table.Column<string>(type: "text", nullable: true),
                    AuthorId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    MessageId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    VoteLike = table.Column<int>(type: "integer", nullable: false),
                    VoteDislike = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Moderator = table.Column<string>(type: "text", nullable: true),
                    ModeratorId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Timestamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suggestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suggestions_GuildInformations_GuildId",
                        column: x => x.GuildId,
                        principalTable: "GuildInformations",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuggestionsConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    SuggestionChannel = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    SuggestionLogChannel = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuggestionsConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuggestionsConfigs_GuildInformations_GuildId",
                        column: x => x.GuildId,
                        principalTable: "GuildInformations",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupportConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ParentId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ActiveTicketsId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    TicketInfoId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    MessageId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Notify = table.Column<int>(type: "integer", nullable: false),
                    TicketMessage = table.Column<string>(type: "text", nullable: true),
                    PremiumStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportConfig_GuildInformations_GuildId",
                        column: x => x.GuildId,
                        principalTable: "GuildInformations",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ModeratorId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    WhoClosedTicket = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Timestamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_GuildInformations_GuildId",
                        column: x => x.GuildId,
                        principalTable: "GuildInformations",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WelcomeMessageConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    WelcomeChannel = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    WelcomeMessageAutoDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WelcomeMessageConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WelcomeMessageConfigs_GuildInformations_GuildId",
                        column: x => x.GuildId,
                        principalTable: "GuildInformations",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BotSettings_GuildId",
                table: "BotSettings",
                column: "GuildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyRankingConfig_GuildId",
                table: "DailyRankingConfig",
                column: "GuildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_GuildId",
                table: "Logs",
                column: "GuildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReactionsConfigs_GuildId",
                table: "ReactionsConfigs",
                column: "GuildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suggestions_GuildId",
                table: "Suggestions",
                column: "GuildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuggestionsConfigs_GuildId",
                table: "SuggestionsConfigs",
                column: "GuildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupportConfig_GuildId",
                table: "SupportConfig",
                column: "GuildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_GuildId",
                table: "Tickets",
                column: "GuildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WelcomeMessageConfigs_GuildId",
                table: "WelcomeMessageConfigs",
                column: "GuildId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BotSettings");

            migrationBuilder.DropTable(
                name: "DailyRankingConfig");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "ReactionsConfigs");

            migrationBuilder.DropTable(
                name: "Suggestions");

            migrationBuilder.DropTable(
                name: "SuggestionsConfigs");

            migrationBuilder.DropTable(
                name: "SupportConfig");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "WelcomeMessageConfigs");

            migrationBuilder.DropTable(
                name: "GuildInformations");
        }
    }
}
