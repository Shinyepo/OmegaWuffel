using Microsoft.EntityFrameworkCore.Migrations;

namespace OWuffel.Migrations
{
    public partial class poprawka : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "logVoiceStateUpdate",
                table: "Settings",
                newName: "logVoiceStateUpdates");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "logVoiceStateUpdates",
                table: "Settings",
                newName: "logVoiceStateUpdate");
        }
    }
}
