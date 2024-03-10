using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FacturilaAPI.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsClient",
                table: "UserFirm",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFirmId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_ActiveFirmId",
                table: "User",
                column: "ActiveFirmId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Firm_ActiveFirmId",
                table: "User",
                column: "ActiveFirmId",
                principalTable: "Firm",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Firm_ActiveFirmId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ActiveFirmId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsClient",
                table: "UserFirm");

            migrationBuilder.DropColumn(
                name: "ActiveFirmId",
                table: "User");
        }
    }
}
