using Microsoft.EntityFrameworkCore.Migrations;

namespace Wp.Data.Migrations.WpContext
{
    public partial class AddedContactFormAndResumeSections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Section_ContactForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EmailTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailCc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailBcc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntroText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThankYouText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEnabled = table.Column<bool>(type: "bit", nullable: false),
                    ExtendedEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section_ContactForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Section_ContactForm_Section_Id",
                        column: x => x.Id,
                        principalTable: "Section",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Section_Resume",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section_Resume", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Section_Resume_Section_Id",
                        column: x => x.Id,
                        principalTable: "Section",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Section_ContactForm");

            migrationBuilder.DropTable(
                name: "Section_Resume");
        }
    }
}
