using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DragonAcc.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "Valorant_Games");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "TocChien_Games");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "Reactions");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "Pubg_Games");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "NgocRong_Games");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "Lol_Games");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "AdminUpdate",
                table: "Valorant_Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdminUpdate",
                table: "TocChien_Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdminUpdate",
                table: "Pubg_Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdminUpdate",
                table: "NgocRong_Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdminUpdate",
                table: "Lol_Games",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminUpdate",
                table: "Valorant_Games");

            migrationBuilder.DropColumn(
                name: "AdminUpdate",
                table: "TocChien_Games");

            migrationBuilder.DropColumn(
                name: "AdminUpdate",
                table: "Pubg_Games");

            migrationBuilder.DropColumn(
                name: "AdminUpdate",
                table: "NgocRong_Games");

            migrationBuilder.DropColumn(
                name: "AdminUpdate",
                table: "Lol_Games");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "Valorant_Games",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "TocChien_Games",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "Reactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "Pubg_Games",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "NgocRong_Games",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "Lol_Games",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "Comments",
                type: "datetime2",
                nullable: true);
        }
    }
}
