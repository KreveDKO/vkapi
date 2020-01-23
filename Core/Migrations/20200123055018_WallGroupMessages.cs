using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class WallGroupMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VkWallMessages_VkUsers_VkUserId",
                table: "VkWallMessages");

            migrationBuilder.AlterColumn<long>(
                name: "VkUserId",
                table: "VkWallMessages",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "VkGroupId",
                table: "VkWallMessages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VkWallMessages_VkGroupId",
                table: "VkWallMessages",
                column: "VkGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_VkWallMessages_UserGroups_VkGroupId",
                table: "VkWallMessages",
                column: "VkGroupId",
                principalTable: "UserGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VkWallMessages_VkUsers_VkUserId",
                table: "VkWallMessages",
                column: "VkUserId",
                principalTable: "VkUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VkWallMessages_UserGroups_VkGroupId",
                table: "VkWallMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_VkWallMessages_VkUsers_VkUserId",
                table: "VkWallMessages");

            migrationBuilder.DropIndex(
                name: "IX_VkWallMessages_VkGroupId",
                table: "VkWallMessages");

            migrationBuilder.DropColumn(
                name: "VkGroupId",
                table: "VkWallMessages");

            migrationBuilder.AlterColumn<long>(
                name: "VkUserId",
                table: "VkWallMessages",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VkWallMessages_VkUsers_VkUserId",
                table: "VkWallMessages",
                column: "VkUserId",
                principalTable: "VkUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
