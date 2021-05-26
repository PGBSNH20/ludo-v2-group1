using Microsoft.EntityFrameworkCore.Migrations;

namespace LudoAPI.Migrations
{
    public partial class AddSquare : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SquareID",
                table: "Token",
                newName: "SquareId");

            migrationBuilder.CreateTable(
                name: "Square",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Square", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Token_Square_SquareId",
                table: "Token");

            migrationBuilder.DropTable(
                name: "Square");

            migrationBuilder.DropIndex(
                name: "IX_Token_SquareId",
                table: "Token");

            migrationBuilder.RenameColumn(
                name: "SquareId",
                table: "Token",
                newName: "SquareID");
        }
    }
}
