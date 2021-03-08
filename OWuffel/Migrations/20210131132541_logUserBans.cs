using Microsoft.EntityFrameworkCore.Migrations;

namespace OWuffel.Migrations
{
    public partial class logUserBans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "logUserBans",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "logUserBans",
                table: "Settings");
        }
    }
}
