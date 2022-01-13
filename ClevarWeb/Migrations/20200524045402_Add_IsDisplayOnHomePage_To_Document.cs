using Microsoft.EntityFrameworkCore.Migrations;

namespace ClevarWeb.Migrations
{
    public partial class Add_IsDisplayOnHomePage_To_Document : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisplayOnHomePage",
                table: "Documents",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisplayOnHomePage",
                table: "Documents");
        }
    }
}
