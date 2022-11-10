using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cyberbezpieczenstwo.Data.Migrations
{
    public partial class addedLogHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogHistory", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "CustomUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "AKCR1THZKROiOzsL/LhA4tIrT/+uRESeOtapnWAtMedABbbYt/WGZDbFtUXX7IHfxg==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogHistory");

            migrationBuilder.UpdateData(
                table: "CustomUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "password");
        }
    }
}
