using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DragonAcc.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameInfoId",
                table: "Valorant_Games");

            migrationBuilder.DropColumn(
                name: "GameInfoId",
                table: "TocChien_Games");

            migrationBuilder.DropColumn(
                name: "GameInfoId",
                table: "Pubg_Games");

            migrationBuilder.DropColumn(
                name: "GameInfoId",
                table: "NgocRong_Games");

            migrationBuilder.CreateTable(
                name: "PurchasedAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountPasswordChange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedAccounts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchasedAccounts");

            migrationBuilder.AddColumn<int>(
                name: "GameInfoId",
                table: "Valorant_Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GameInfoId",
                table: "TocChien_Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GameInfoId",
                table: "Pubg_Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GameInfoId",
                table: "NgocRong_Games",
                type: "int",
                nullable: true);
        }
    }
}
