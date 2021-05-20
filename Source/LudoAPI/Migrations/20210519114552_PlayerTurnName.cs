using Microsoft.EntityFrameworkCore.Migrations;

namespace LudoAPI.Migrations
{
    public partial class PlayerTurnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlayerTurnName",
                table: "Board",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerTurnName",
                table: "Board");
        }
    }
}
