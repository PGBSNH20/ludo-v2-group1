using Microsoft.EntityFrameworkCore.Migrations;

namespace Ludo.API.Migrations
{
    public partial class Correction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Board",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoardName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PlayerIDLastMadeMove = table.Column<int>(type: "int", nullable: false),
                    PlayerTurnName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Board", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoardId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Player_Board_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Board",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Square",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Index = table.Column<int>(type: "int", nullable: false),
                    BoardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Square", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Square_Board_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Board",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Steps = table.Column<int>(type: "int", nullable: false),
                    SquareId = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Token_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Route",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Index = table.Column<int>(type: "int", nullable: false),
                    TokenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Route", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Route_Token_TokenId",
                        column: x => x.TokenId,
                        principalTable: "Token",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_Player_BoardId",
                table: "Player",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Route_TokenId",
                table: "Route",
                column: "TokenId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Token_PlayerId",
                table: "Token",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Route");

            migrationBuilder.DropTable(
                name: "SquareOccupant");

            migrationBuilder.DropTable(
                name: "Square");

            migrationBuilder.DropTable(
                name: "Token");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Board");
        }
    }
}
