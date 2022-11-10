
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cyberbezpieczenstwo.Data.Migrations
{
    public partial class addedLogHistory4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "LogHistory",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "LogHistory");
        }
    }
}
