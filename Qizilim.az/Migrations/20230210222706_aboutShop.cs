using Microsoft.EntityFrameworkCore.Migrations;

namespace Qizilim.az.Migrations
{
    public partial class aboutShop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "aboutShop",
                schema: "Membership",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "aboutShop",
                schema: "Membership",
                table: "Users");
        }
    }
}
