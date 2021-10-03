using Microsoft.EntityFrameworkCore.Migrations;

namespace OWuffel.Migrations
{
    public partial class NameConventionNormalization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_GuildInformations_GuildId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportConfig_GuildInformations_GuildId",
                table: "SupportConfig");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupportConfig",
                table: "SupportConfig");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Logs",
                table: "Logs");

            migrationBuilder.RenameTable(
                name: "SupportConfig",
                newName: "SupportConfigs");

            migrationBuilder.RenameTable(
                name: "Logs",
                newName: "LogsConfigs");

            migrationBuilder.RenameIndex(
                name: "IX_SupportConfig_GuildId",
                table: "SupportConfigs",
                newName: "IX_SupportConfigs_GuildId");

            migrationBuilder.RenameIndex(
                name: "IX_Logs_GuildId",
                table: "LogsConfigs",
                newName: "IX_LogsConfigs_GuildId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupportConfigs",
                table: "SupportConfigs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogsConfigs",
                table: "LogsConfigs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LogsConfigs_GuildInformations_GuildId",
                table: "LogsConfigs",
                column: "GuildId",
                principalTable: "GuildInformations",
                principalColumn: "GuildId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportConfigs_GuildInformations_GuildId",
                table: "SupportConfigs",
                column: "GuildId",
                principalTable: "GuildInformations",
                principalColumn: "GuildId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogsConfigs_GuildInformations_GuildId",
                table: "LogsConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportConfigs_GuildInformations_GuildId",
                table: "SupportConfigs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupportConfigs",
                table: "SupportConfigs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LogsConfigs",
                table: "LogsConfigs");

            migrationBuilder.RenameTable(
                name: "SupportConfigs",
                newName: "SupportConfig");

            migrationBuilder.RenameTable(
                name: "LogsConfigs",
                newName: "Logs");

            migrationBuilder.RenameIndex(
                name: "IX_SupportConfigs_GuildId",
                table: "SupportConfig",
                newName: "IX_SupportConfig_GuildId");

            migrationBuilder.RenameIndex(
                name: "IX_LogsConfigs_GuildId",
                table: "Logs",
                newName: "IX_Logs_GuildId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupportConfig",
                table: "SupportConfig",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logs",
                table: "Logs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_GuildInformations_GuildId",
                table: "Logs",
                column: "GuildId",
                principalTable: "GuildInformations",
                principalColumn: "GuildId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportConfig_GuildInformations_GuildId",
                table: "SupportConfig",
                column: "GuildId",
                principalTable: "GuildInformations",
                principalColumn: "GuildId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
