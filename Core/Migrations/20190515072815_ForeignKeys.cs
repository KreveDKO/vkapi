using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class ForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FriendsUserToUsers_LeftUserId",
                table: "FriendsUserToUsers",
                column: "LeftUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendsUserToUsers_RightUserId",
                table: "FriendsUserToUsers",
                column: "RightUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendsUserToUsers_Users_LeftUserId",
                table: "FriendsUserToUsers",
                column: "LeftUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendsUserToUsers_Users_RightUserId",
                table: "FriendsUserToUsers",
                column: "RightUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendsUserToUsers_Users_LeftUserId",
                table: "FriendsUserToUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendsUserToUsers_Users_RightUserId",
                table: "FriendsUserToUsers");

            migrationBuilder.DropIndex(
                name: "IX_FriendsUserToUsers_LeftUserId",
                table: "FriendsUserToUsers");

            migrationBuilder.DropIndex(
                name: "IX_FriendsUserToUsers_RightUserId",
                table: "FriendsUserToUsers");
        }
    }
}
