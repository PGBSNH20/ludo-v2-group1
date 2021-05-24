using Microsoft.EntityFrameworkCore.Migrations;

namespace LudoAPI.Migrations
{
    public partial class OldSquareId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OldSquareId",
                table: "Token",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldSquareId",
                table: "Token");
        }
    }
}
