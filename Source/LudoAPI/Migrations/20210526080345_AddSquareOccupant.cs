using Microsoft.EntityFrameworkCore.Migrations;

namespace LudoAPI.Migrations
{
    public partial class AddSquareOccupant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Token_Square_SquareId",
                table: "Token");

            migrationBuilder.DropIndex(
                name: "IX_Token_SquareId",
                table: "Token");

            migrationBuilder.CreateTable(
                name: "SquareOccupant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SquareId = table.Column<int>(type: "int", nullable: false),
                    OccupantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SquareOccupant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SquareOccupant_Square_SquareId",
                        column: x => x.SquareId,
                        principalTable: "Square",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SquareOccupant_Token_OccupantId",
                        column: x => x.OccupantId,
                        principalTable: "Token",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Square_BoardId",
                table: "Square",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_SquareOccupant_OccupantId",
                table: "SquareOccupant",
                column: "OccupantId");

            migrationBuilder.CreateIndex(
                name: "IX_SquareOccupant_SquareId",
                table: "SquareOccupant",
                column: "SquareId");

            migrationBuilder.AddForeignKey(
                name: "FK_Square_Board_BoardId",
                table: "Square",
                column: "BoardId",
                principalTable: "Board",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Square_Board_BoardId",
                table: "Square");

            migrationBuilder.DropTable(
                name: "SquareOccupant");

            migrationBuilder.DropIndex(
                name: "IX_Square_BoardId",
                table: "Square");

            migrationBuilder.CreateIndex(
                name: "IX_Token_SquareId",
                table: "Token",
                column: "SquareId");

            migrationBuilder.AddForeignKey(
                name: "FK_Token_Square_SquareId",
                table: "Token",
                column: "SquareId",
                principalTable: "Square",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
