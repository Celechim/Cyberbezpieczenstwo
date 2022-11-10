using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cyberbezpieczenstwo.Data.Migrations
{
    public partial class AddedSecurityLogoutParams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSetAutoLogout",
                table: "SecuritySettings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SecToAutoLogout",
                table: "SecuritySettings",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSetAutoLogout",
                table: "SecuritySettings");

            migrationBuilder.DropColumn(
                name: "SecToAutoLogout",
                table: "SecuritySettings");
        }
    }
}
