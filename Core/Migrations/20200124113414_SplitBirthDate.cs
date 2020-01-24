using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class SplitBirthDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthdate",
                table: "VkUsers");

            migrationBuilder.AddColumn<int>(
                name: "BirthDay",
                table: "VkUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BirthMonth",
                table: "VkUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BirthYear",
                table: "VkUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDay",
                table: "VkUsers");

            migrationBuilder.DropColumn(
                name: "BirthMonth",
                table: "VkUsers");

            migrationBuilder.DropColumn(
                name: "BirthYear",
                table: "VkUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthdate",
                table: "VkUsers",
                type: "timestamp without time zone",
                nullable: true);
        }
    }
}
