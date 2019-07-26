using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Core.Migrations
{
    public partial class AddMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DialogId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    ExternalId = table.Column<long>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendsUserToUsers_LeftUserId",
                table: "FriendsUserToUsers",
                column: "LeftUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendsUserToUsers_RightUserId",
                table: "FriendsUserToUsers",
                column: "RightUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");

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

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_FriendsUserToUsers_LeftUserId",
                table: "FriendsUserToUsers");

            migrationBuilder.DropIndex(
                name: "IX_FriendsUserToUsers_RightUserId",
                table: "FriendsUserToUsers");
        }
    }
}
