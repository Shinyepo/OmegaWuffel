using Microsoft.EntityFrameworkCore.Migrations;

namespace OWuffel.Migrations
{
    public partial class NameConventionNormalizationPart2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyRankingConfig_GuildInformations_GuildId",
                table: "DailyRankingConfig");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DailyRankingConfig",
                table: "DailyRankingConfig");

            migrationBuilder.RenameTable(
                name: "DailyRankingConfig",
                newName: "DailyRankingConfigs");

            migrationBuilder.RenameIndex(
                name: "IX_DailyRankingConfig_GuildId",
                table: "DailyRankingConfigs",
                newName: "IX_DailyRankingConfigs_GuildId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DailyRankingConfigs",
                table: "DailyRankingConfigs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyRankingConfigs_GuildInformations_GuildId",
                table: "DailyRankingConfigs",
                column: "GuildId",
                principalTable: "GuildInformations",
                principalColumn: "GuildId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyRankingConfigs_GuildInformations_GuildId",
                table: "DailyRankingConfigs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DailyRankingConfigs",
                table: "DailyRankingConfigs");

            migrationBuilder.RenameTable(
                name: "DailyRankingConfigs",
                newName: "DailyRankingConfig");

            migrationBuilder.RenameIndex(
                name: "IX_DailyRankingConfigs_GuildId",
                table: "DailyRankingConfig",
                newName: "IX_DailyRankingConfig_GuildId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DailyRankingConfig",
                table: "DailyRankingConfig",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyRankingConfig_GuildInformations_GuildId",
                table: "DailyRankingConfig",
                column: "GuildId",
                principalTable: "GuildInformations",
                principalColumn: "GuildId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
