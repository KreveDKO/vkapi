using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Core.Migrations
{
    public partial class AddVkAudio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VkAudio",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(nullable: false),
                    ExternalId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VkAudio", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VkAudioArtist",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(nullable: false),
                    ExternalId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VkAudioArtist", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VkAudioToUser",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VkAudioId = table.Column<long>(nullable: false),
                    VkUserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VkAudioToUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VkAudioToUser_VkAudio_VkAudioId",
                        column: x => x.VkAudioId,
                        principalTable: "VkAudio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VkAudioToUser_VkUsers_VkUserId",
                        column: x => x.VkUserId,
                        principalTable: "VkUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VkAudioToArtist",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VkAudioId = table.Column<long>(nullable: false),
                    VkAudioArtistsId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VkAudioToArtist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VkAudioToArtist_VkAudioArtist_VkAudioArtistsId",
                        column: x => x.VkAudioArtistsId,
                        principalTable: "VkAudioArtist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VkAudioToArtist_VkAudio_VkAudioId",
                        column: x => x.VkAudioId,
                        principalTable: "VkAudio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VkAudioToArtist_VkAudioArtistsId",
                table: "VkAudioToArtist",
                column: "VkAudioArtistsId");

            migrationBuilder.CreateIndex(
                name: "IX_VkAudioToArtist_VkAudioId",
                table: "VkAudioToArtist",
                column: "VkAudioId");

            migrationBuilder.CreateIndex(
                name: "IX_VkAudioToUser_VkAudioId",
                table: "VkAudioToUser",
                column: "VkAudioId");

            migrationBuilder.CreateIndex(
                name: "IX_VkAudioToUser_VkUserId",
                table: "VkAudioToUser",
                column: "VkUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VkAudioToArtist");

            migrationBuilder.DropTable(
                name: "VkAudioToUser");

            migrationBuilder.DropTable(
                name: "VkAudioArtist");

            migrationBuilder.DropTable(
                name: "VkAudio");
        }
    }
}
