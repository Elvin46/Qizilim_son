using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Qizilim.az.Migrations
{
    public partial class kateqoriyalar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kateqoriya",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedById = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kateqoriya", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductKateqoriya",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    KateqoriyaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductKateqoriya", x => new { x.KateqoriyaId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ProductKateqoriya_Kateqoriya_KateqoriyaId",
                        column: x => x.KateqoriyaId,
                        principalTable: "Kateqoriya",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductKateqoriya_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductKateqoriya_ProductId",
                table: "ProductKateqoriya",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductKateqoriya");

            migrationBuilder.DropTable(
                name: "Kateqoriya");
        }
    }
}
