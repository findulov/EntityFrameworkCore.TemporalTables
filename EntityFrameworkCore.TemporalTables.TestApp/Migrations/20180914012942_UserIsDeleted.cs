using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkCore.TemporalTables.TestApp.Migrations
{
    public partial class UserIsDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IsDeleted",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");
        }
    }
}
