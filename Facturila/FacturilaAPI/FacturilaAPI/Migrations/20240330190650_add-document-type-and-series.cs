using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FacturilaAPI.Migrations
{
    /// <inheritdoc />
    public partial class adddocumenttypeandseries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentSeries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriesName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstNumber = table.Column<int>(type: "int", nullable: false),
                    CurrentNumber = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentSeries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentSeries_DocumentType_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSeries_DocumentTypeId",
                table: "DocumentSeries",
                column: "DocumentTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentSeries");

            migrationBuilder.DropTable(
                name: "DocumentType");
        }
    }
}
