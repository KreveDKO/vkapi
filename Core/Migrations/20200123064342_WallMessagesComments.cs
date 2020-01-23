using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class WallMessagesComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentsCount",
                table: "VkWallMessages",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentsCount",
                table: "VkWallMessages");
        }
    }
}
