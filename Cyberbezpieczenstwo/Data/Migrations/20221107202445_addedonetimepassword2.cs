using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cyberbezpieczenstwo.Data.Migrations
{
    public partial class addedonetimepassword2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "X",
                table: "CustomUsers",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "X",
                table: "CustomUsers");
        }
    }
}
