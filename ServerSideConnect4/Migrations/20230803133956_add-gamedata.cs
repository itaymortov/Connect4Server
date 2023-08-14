using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerSideConnect4.Migrations
{
    public partial class addgamedata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isPlayerWin",
                table: "GameDatas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPlayerWin",
                table: "GameDatas");
        }
    }
}
