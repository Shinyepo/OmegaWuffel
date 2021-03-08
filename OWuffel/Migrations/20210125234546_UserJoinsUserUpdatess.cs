using Microsoft.EntityFrameworkCore.Migrations;

namespace OWuffel.Migrations
{
    public partial class UserJoinsUserUpdatess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "logUserEvents",
                table: "Settings",
                newName: "logUserUpdates");

            migrationBuilder.AddColumn<ulong>(
                name: "logUserJoins",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "logUserJoins",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "logUserUpdates",
                table: "Settings",
                newName: "logUserEvents");
        }
    }
}
