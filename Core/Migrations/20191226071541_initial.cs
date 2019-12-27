using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Core.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    Id = table.Column<long>()
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Title = table.Column<string>(nullable: true),
                    GroupType = table.Column<int>(),
                    GroupId = table.Column<long>(),
                    IsDeactivated = table.Column<bool>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>()
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsDeactivated = table.Column<bool>(),
                    FullName = table.Column<string>(nullable: true),
                    PhotoUrl = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(),
                    LastCheck = table.Column<DateTime>(),
                    VkUserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_VkUserId",
                        column: x => x.VkUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FriendsUserToUsers",
                columns: table => new
                {
                    Id = table.Column<long>()
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    LeftUserId = table.Column<long>(),
                    RightUserId = table.Column<long>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendsUserToUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendsUserToUsers_Users_LeftUserId",
                        column: x => x.LeftUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendsUserToUsers_Users_RightUserId",
                        column: x => x.RightUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<long>()
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DialogId = table.Column<long>(),
                    UserId = table.Column<long>(),
                    ExternalId = table.Column<long>(),
                    Text = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>()
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

            migrationBuilder.CreateTable(
                name: "UserToUserGroup",
                columns: table => new
                {
                    Id = table.Column<long>()
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<long>(),
                    UserGroupId = table.Column<long>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToUserGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserToUserGroup_UserGroups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserToUserGroup_Users_UserId",
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

            migrationBuilder.CreateIndex(
                name: "IX_Users_VkUserId",
                table: "Users",
                column: "VkUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToUserGroup_UserGroupId",
                table: "UserToUserGroup",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToUserGroup_UserId",
                table: "UserToUserGroup",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendsUserToUsers");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "UserToUserGroup");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
