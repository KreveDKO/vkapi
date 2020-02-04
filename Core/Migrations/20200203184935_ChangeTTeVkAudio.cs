using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class ChangeTTeVkAudio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                table: "VkAudioArtist",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ExternalId",
                table: "VkAudioArtist",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
