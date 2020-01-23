using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Core.Migrations
{
    public partial class WallMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VkWallMessages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(nullable: false),
                    AttachmentsCount = table.Column<int>(nullable: false),
                    ViewsCount = table.Column<int>(nullable: false),
                    LikesCount = table.Column<int>(nullable: false),
                    VkUserId = table.Column<long>(nullable: false),
                    AuthorUserId = table.Column<long>(nullable: true),
                    ExternalId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VkWallMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VkWallMessages_VkUsers_AuthorUserId",
                        column: x => x.AuthorUserId,
                        principalTable: "VkUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VkWallMessages_VkUsers_VkUserId",
                        column: x => x.VkUserId,
                        principalTable: "VkUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VkWallMessages_AuthorUserId",
                table: "VkWallMessages",
                column: "AuthorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VkWallMessages_VkUserId",
                table: "VkWallMessages",
                column: "VkUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VkWallMessages");
        }
    }
}
