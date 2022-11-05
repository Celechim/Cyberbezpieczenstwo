using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cyberbezpieczenstwo.Data.Migrations
{
    public partial class seedSomeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CustomUsers",
                columns: new[] { "Id", "ExpirationTime", "IsBlocked", "Login", "Password", "Role" },
                values: new object[] { 1, null, false, "ADMIN", "AKCR1THZKROiOzsL/LhA4tIrT/+uRESeOtapnWAtMedABbbYt/WGZDbFtUXX7IHfxg==", 0 });

            migrationBuilder.InsertData(
                table: "PasswordLimitations",
                columns: new[] { "Id", "IsActive", "LimitationName" },
                values: new object[,]
                {
                    { 2, false, "Co najmniej 1 wielka litera" },
                    { 3, false, "Co najmniej 1 znak specjalny" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CustomUsers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PasswordLimitations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PasswordLimitations",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
