using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Core.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(nullable: false),
                    GroupType = table.Column<int>(nullable: false),
                    ExternalId = table.Column<long>(nullable: false),
                    IsDeactivated = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VkUsers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeactivated = table.Column<bool>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    PhotoUrl = table.Column<string>(nullable: true),
                    ExternalId = table.Column<long>(nullable: false),
                    LastCheck = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VkUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FriendsUserToUsers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeftUserId = table.Column<long>(nullable: false),
                    RightUserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendsUserToUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendsUserToUsers_VkUsers_LeftUserId",
                        column: x => x.LeftUserId,
                        principalTable: "VkUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendsUserToUsers_VkUsers_RightUserId",
                        column: x => x.RightUserId,
                        principalTable: "VkUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToUserGroup",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VkUserId = table.Column<long>(nullable: false),
                    VkUserGroupId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToUserGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserToUserGroup_UserGroups_VkUserGroupId",
                        column: x => x.VkUserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserToUserGroup_VkUsers_VkUserId",
                        column: x => x.VkUserId,
                        principalTable: "VkUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VkMessages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DialogId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    ExternalId = table.Column<long>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VkMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VkMessages_VkUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "VkUsers",
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
                name: "IX_UserToUserGroup_VkUserGroupId",
                table: "UserToUserGroup",
                column: "VkUserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToUserGroup_VkUserId",
                table: "UserToUserGroup",
                column: "VkUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VkMessages_UserId",
                table: "VkMessages",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendsUserToUsers");

            migrationBuilder.DropTable(
                name: "UserToUserGroup");

            migrationBuilder.DropTable(
                name: "VkMessages");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "VkUsers");
        }
    }
}
