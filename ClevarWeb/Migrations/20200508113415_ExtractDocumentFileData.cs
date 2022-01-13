using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClevarWeb.Migrations
{
    public partial class ExtractDocumentFileData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileData",
                table: "Documents");

            migrationBuilder.AddColumn<int>(
                name: "DocumentDataId",
                table: "Documents",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DocumentDatas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(nullable: false),
                    FileData = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentDatas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentDataId",
                table: "Documents",
                column: "DocumentDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_DocumentDatas_DocumentDataId",
                table: "Documents",
                column: "DocumentDataId",
                principalTable: "DocumentDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_DocumentDatas_DocumentDataId",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "DocumentDatas");

            migrationBuilder.DropIndex(
                name: "IX_Documents_DocumentDataId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "DocumentDataId",
                table: "Documents");

            migrationBuilder.AddColumn<byte[]>(
                name: "FileData",
                table: "Documents",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
