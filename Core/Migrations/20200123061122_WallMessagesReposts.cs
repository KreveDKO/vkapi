using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class WallMessagesReposts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RepostsCount",
                table: "VkWallMessages",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepostsCount",
                table: "VkWallMessages");
        }
    }
}
