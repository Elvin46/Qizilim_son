using Microsoft.EntityFrameworkCore.Migrations;

namespace Qizilim.az.Migrations
{
    public partial class UserElaveleri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "instagramLink",
                schema: "Membership",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shopLocation",
                schema: "Membership",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shopName",
                schema: "Membership",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shopNumber",
                schema: "Membership",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tiktokLink",
                schema: "Membership",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "whatsappNumber",
                schema: "Membership",
                table: "Users",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "instagramLink",
                schema: "Membership",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "shopLocation",
                schema: "Membership",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "shopName",
                schema: "Membership",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "shopNumber",
                schema: "Membership",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "tiktokLink",
                schema: "Membership",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "whatsappNumber",
                schema: "Membership",
                table: "Users");
        }
    }
}
