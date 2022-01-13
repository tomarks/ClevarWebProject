using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClevarWeb.Migrations
{
    public partial class initialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Title = table.Column<string>(maxLength: 255, nullable: true),
                    SubTitle = table.Column<string>(maxLength: 255, nullable: true),
                    BioTitle = table.Column<string>(nullable: true),
                    BioHtml = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDisplayedOnPeoplePage = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    SiteTitle = table.Column<string>(maxLength: 255, nullable: true),
                    SiteKeywords = table.Column<string>(maxLength: 4000, nullable: true),
                    SiteDescription = table.Column<string>(maxLength: 4000, nullable: true),
                    SocialTwitterURL = table.Column<string>(maxLength: 255, nullable: true),
                    SocialFacebookURL = table.Column<string>(maxLength: 255, nullable: true),
                    SocialInstagramURL = table.Column<string>(maxLength: 255, nullable: true),
                    SocialLinkedinURL = table.Column<string>(maxLength: 255, nullable: true),
                    PublicEmailAddress = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartoonImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(nullable: false),
                    PersonId = table.Column<int>(nullable: true),
                    DocumentName = table.Column<string>(nullable: true),
                    LayerNumber = table.Column<int>(nullable: false),
                    Styles = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartoonImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartoonImages_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 255, nullable: false),
                    Description = table.Column<string>(maxLength: 4000, nullable: false),
                    AuthorId = table.Column<int>(nullable: true),
                    SupervisorId = table.Column<int>(nullable: true),
                    Tagline = table.Column<string>(maxLength: 255, nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    HTMLContent = table.Column<string>(nullable: true),
                    PublishedDateTime = table.Column<DateTime>(nullable: true),
                    PrimaryDocumentName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_People_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_People_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartoonImages_PersonId",
                table: "CartoonImages",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AuthorId",
                table: "Projects",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_SupervisorId",
                table: "Projects",
                column: "SupervisorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartoonImages");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.DropTable(
                name: "People");
        }
    }
}
