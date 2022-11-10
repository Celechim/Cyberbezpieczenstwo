using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cyberbezpieczenstwo.Data.Migrations
{
    public partial class addedLogHistory3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "LogHistory",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LogHistory",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "LogHistory",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "LogHistory");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "LogHistory");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "LogHistory");
        }
    }
}
