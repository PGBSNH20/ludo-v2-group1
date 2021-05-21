using Microsoft.EntityFrameworkCore.Migrations;

namespace LudoAPI.Migrations
{
    public partial class SeedingDataUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Player",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "playerRed");

            migrationBuilder.UpdateData(
                table: "Player",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "playerBlue");

            migrationBuilder.UpdateData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 2,
                column: "SquareID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 4,
                column: "SquareID",
                value: 30);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Player",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "pl1");

            migrationBuilder.UpdateData(
                table: "Player",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "pl2");

            migrationBuilder.UpdateData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 2,
                column: "SquareID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Token",
                keyColumn: "Id",
                keyValue: 4,
                column: "SquareID",
                value: 0);
        }
    }
}
