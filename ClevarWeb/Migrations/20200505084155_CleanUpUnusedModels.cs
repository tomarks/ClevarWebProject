using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClevarWeb.Migrations
{
    public partial class CleanUpUnusedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BioTitle",
                table: "People");

            migrationBuilder.DropColumn(
                name: "IsDisplayedOnPeoplePage",
                table: "People");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedDateTime",
                table: "Projects",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedDateTime",
                table: "Projects",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<string>(
                name: "BioTitle",
                table: "People",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisplayedOnPeoplePage",
                table: "People",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
