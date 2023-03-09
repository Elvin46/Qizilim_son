using Microsoft.EntityFrameworkCore.Migrations;

namespace Qizilim.az.Migrations
{
    public partial class producteyar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eyars_Products_ProductId",
                table: "Eyars");

            migrationBuilder.DropIndex(
                name: "IX_Eyars_ProductId",
                table: "Eyars");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Eyars");

            migrationBuilder.CreateTable(
                name: "ProductEyar",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    EyarId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductEyar", x => new { x.EyarId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ProductEyar_Eyars_EyarId",
                        column: x => x.EyarId,
                        principalTable: "Eyars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductEyar_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductEyar_ProductsId",
                table: "ProductEyar",
                column: "ProductsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductEyar");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Eyars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Eyars_ProductId",
                table: "Eyars",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Eyars_Products_ProductId",
                table: "Eyars",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
