using Microsoft.EntityFrameworkCore.Migrations;

namespace LudoAPI.Migrations
{
    public partial class SeedingData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Board",
                columns: new[] { "Id", "BoardName", "PlayerIDLastMadeMove" },
                values: new object[] { 1, "game1", 0 });

            migrationBuilder.InsertData(
                table: "Player",
                columns: new[] { "Id", "BoardId", "Name" },
                values: new object[] { 1, 1, "pl1" });

            migrationBuilder.InsertData(
                table: "Player",
                columns: new[] { "Id", "BoardId", "Name" },
                values: new object[] { 2, 1, "pl2" });

            migrationBuilder.InsertData(
                table: "Token",
                columns: new[] { "Id", "Color", "IsActive", "PlayerId", "Steps" },
                values: new object[,]
                {
                    { 1, 2, true, 1, 1 },
                    { 2, 2, true, 1, 2 },
                    { 3, 0, false, 2, 0 },
                    { 4, 0, true, 2, 1 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Player",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Player",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Board",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
