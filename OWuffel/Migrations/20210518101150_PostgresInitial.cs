﻿using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace OWuffel.Migrations
{
    public partial class PostgresInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelChecks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    AuthorId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    MessageId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Target = table.Column<string>(type: "text", nullable: true),
                    Channels = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Found = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelChecks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyRankings",
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
                    table.PrimaryKey("PK_DailyRankings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guild_id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    botActive = table.Column<int>(type: "integer", nullable: false),
                    botPrefix = table.Column<string>(type: "text", nullable: true),
                    botModRole = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    botAdminRole = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    botDisabledCommands = table.Column<string>(type: "text", nullable: true),
                    botSystemNotice = table.Column<bool>(type: "boolean", nullable: false),
                    botClearMessage = table.Column<bool>(type: "boolean", nullable: false),
                    botClearMessagesForBot = table.Column<bool>(type: "boolean", nullable: false),
                    botMuteRole = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    botLanguage = table.Column<string>(type: "text", nullable: true),
                    welcomeChannel = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    welcomeMessageAutoDelete = table.Column<bool>(type: "boolean", nullable: false),
                    logVoiceStateUpdated = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    logIgnoreVoiceStateUpdated = table.Column<string>(type: "text", nullable: true),
                    logUserJoined = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    logUserLeft = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    logUserBanned = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    logUserUnbanned = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    logUserUpdated = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    logMessageDeleted = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    logMessageUpdated = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    logIgnoreMessageUpdated = table.Column<string>(type: "text", nullable: true),
                    logIgnoreMessageDeleted = table.Column<string>(type: "text", nullable: true),
                    logEmoteCreated = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    logEmoteUpdated = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    logEmoteDeleted = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    logRoleCreated = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    logRoleUpdated = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    logRoleDeleted = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    logGuildUpdated = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    logChannelUpdated = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    logChannelCreated = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    logChannelDeleted = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    suggestionChannel = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    suggestionLogChannel = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    reactServerChannel = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    reactServerMessage = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    reactServerParent = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.id);
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
                });

            migrationBuilder.CreateTable(
                name: "SupportConfiguration",
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
                    table.PrimaryKey("PK_SupportConfiguration", x => x.Id);
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_Settings_guild_id",
                table: "Settings",
                column: "guild_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelChecks");

            migrationBuilder.DropTable(
                name: "DailyRankings");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Suggestions");

            migrationBuilder.DropTable(
                name: "SupportConfiguration");

            migrationBuilder.DropTable(
                name: "Tickets");
        }
    }
}
