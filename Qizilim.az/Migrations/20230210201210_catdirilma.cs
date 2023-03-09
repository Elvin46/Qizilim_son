using Microsoft.EntityFrameworkCore.Migrations;

namespace Qizilim.az.Migrations
{
    public partial class catdirilma : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "catdirilma",
                schema: "Membership",
                table: "Users",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "catdirilma",
                schema: "Membership",
                table: "Users");
        }
    }
}
