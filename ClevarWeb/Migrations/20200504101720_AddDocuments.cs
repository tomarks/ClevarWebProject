using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClevarWeb.Migrations
{
    public partial class AddDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryDocumentName",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DocumentName",
                table: "CartoonImages");

            migrationBuilder.AddColumn<int>(
                name: "PrimaryDocumentId",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                table: "CartoonImages",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(nullable: false),
                    DocumentName = table.Column<string>(nullable: false),
                    FileName = table.Column<string>(nullable: false),
                    DocumentType = table.Column<string>(nullable: false),
                    FileData = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_PrimaryDocumentId",
                table: "Projects",
                column: "PrimaryDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_CartoonImages_DocumentId",
                table: "CartoonImages",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartoonImages_Documents_DocumentId",
                table: "CartoonImages",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Documents_PrimaryDocumentId",
                table: "Projects",
                column: "PrimaryDocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartoonImages_Documents_DocumentId",
                table: "CartoonImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Documents_PrimaryDocumentId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Projects_PrimaryDocumentId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_CartoonImages_DocumentId",
                table: "CartoonImages");

            migrationBuilder.DropColumn(
                name: "PrimaryDocumentId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "CartoonImages");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryDocumentName",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentName",
                table: "CartoonImages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
