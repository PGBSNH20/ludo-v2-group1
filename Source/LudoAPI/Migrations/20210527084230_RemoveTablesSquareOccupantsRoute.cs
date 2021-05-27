using Microsoft.EntityFrameworkCore.Migrations;

namespace Ludo.API.Migrations
{
    public partial class RemoveTablesSquareOccupantsRoute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Route");

            migrationBuilder.DropTable(
                name: "SquareOccupant");

            migrationBuilder.DropTable(
                name: "Square");

            migrationBuilder.DropColumn(
                name: "PlayerIDLastMadeMove",
                table: "Board");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayerIDLastMadeMove",
                table: "Board",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                name: "Square",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoardId = table.Column<int>(type: "int", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false)
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
                name: "SquareOccupant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OccupantId = table.Column<int>(type: "int", nullable: true),
                    SquareId = table.Column<int>(type: "int", nullable: false)
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
        }
    }
}
