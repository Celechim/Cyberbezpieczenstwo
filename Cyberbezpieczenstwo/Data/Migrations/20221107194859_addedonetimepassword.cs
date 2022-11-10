using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cyberbezpieczenstwo.Data.Migrations
{
    public partial class addedonetimepassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasOneUsePassword",
                table: "CustomUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OneUsePassword",
                table: "CustomUsers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasOneUsePassword",
                table: "CustomUsers");

            migrationBuilder.DropColumn(
                name: "OneUsePassword",
                table: "CustomUsers");
        }
    }
}
