using Microsoft.EntityFrameworkCore.Migrations;

namespace ClevarWeb.Migrations
{
    public partial class PersonPrimaryDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PrimaryDocumentId",
                table: "People",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_People_PrimaryDocumentId",
                table: "People",
                column: "PrimaryDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_People_Documents_PrimaryDocumentId",
                table: "People",
                column: "PrimaryDocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_Documents_PrimaryDocumentId",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_People_PrimaryDocumentId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "PrimaryDocumentId",
                table: "People");
        }
    }
}
