using Microsoft.EntityFrameworkCore.Migrations;

namespace LudoAPI.Migrations
{
    public partial class SeedingDataUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Board",
                keyColumn: "Id",
                keyValue: 1,
                column: "PlayerTurnName",
                value: "playerRed");

            migrationBuilder.InsertData(
                table: "Player",
                columns: new[] { "Id", "BoardId", "Name" },
                values: new object[,]
                {
                    { 3, 1, "playerGreen" },
                    { 4, 1, "playerYellow" }
                });

            migrationBuilder.UpdateData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 1,
                column: "SquareID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "SquareID", "Steps" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Color", "IsActive", "PlayerId", "SquareID", "Steps" },
                values: new object[] { 2, true, 1, 17, 17 });

            migrationBuilder.UpdateData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Color", "PlayerId", "SquareID", "Steps" },
                values: new object[] { 2, 1, 18, 18 });

            migrationBuilder.InsertData(
                table: "Token",
                columns: new[] { "Id", "Color", "IsActive", "PlayerId", "SquareID", "Steps" },
                values: new object[,]
                {
                    { 5, 0, true, 2, 50, 11 },
                    { 6, 0, true, 2, 12, 25 },
                    { 7, 0, false, 2, 0, 0 },
                    { 8, 0, false, 2, 0, 0 }
                });

            migrationBuilder.InsertData(
                table: "Token",
                columns: new[] { "Id", "Color", "IsActive", "PlayerId", "SquareID", "Steps" },
                values: new object[,]
                {
                    { 9, 3, true, 3, 13, 0 },
                    { 10, 3, true, 3, 14, 1 },
                    { 11, 3, true, 3, 15, 2 },
                    { 12, 3, true, 3, 16, 3 },
                    { 13, 1, true, 4, 301, 51 },
                    { 14, 1, true, 4, 302, 52 },
                    { 15, 1, true, 4, 303, 53 },
                    { 16, 1, true, 4, 304, 54 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Player",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Player",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Board",
                keyColumn: "Id",
                keyValue: 1,
                column: "PlayerTurnName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 1,
                column: "SquareID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "SquareID", "Steps" },
                values: new object[] { 2, 2 });

            migrationBuilder.UpdateData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Color", "IsActive", "PlayerId", "SquareID", "Steps" },
                values: new object[] { 0, false, 2, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Color", "PlayerId", "SquareID", "Steps" },
                values: new object[] { 0, 2, 30, 1 });
        }
    }
}
