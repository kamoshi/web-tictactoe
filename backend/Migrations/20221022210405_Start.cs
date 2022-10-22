using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tictactoe.Migrations
{
    public partial class Start : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    State = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerOId = table.Column<long>(type: "INTEGER", nullable: true),
                    PlayerXId = table.Column<long>(type: "INTEGER", nullable: true),
                    Top = table.Column<byte>(type: "INTEGER", nullable: false),
                    Mid = table.Column<byte>(type: "INTEGER", nullable: false),
                    Bot = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Game_Player_PlayerOId",
                        column: x => x.PlayerOId,
                        principalTable: "Player",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Game_Player_PlayerXId",
                        column: x => x.PlayerXId,
                        principalTable: "Player",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Game_PlayerOId",
                table: "Game",
                column: "PlayerOId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_PlayerXId",
                table: "Game",
                column: "PlayerXId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "Player");
        }
    }
}
