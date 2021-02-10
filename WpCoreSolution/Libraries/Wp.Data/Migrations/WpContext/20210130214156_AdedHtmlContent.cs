using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wp.Data.Migrations.WpContext
{
    public partial class AdedHtmlContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Resume_ResumeId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "EducationItem");

            migrationBuilder.DropTable(
                name: "LocaleStringResource");

            migrationBuilder.DropTable(
                name: "LocalizedProperty");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "SkillItem");

            migrationBuilder.DropTable(
                name: "Education");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "Experience");

            migrationBuilder.DropTable(
                name: "Skill");

            migrationBuilder.DropTable(
                name: "Resume");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ResumeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserName",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "EmailBcc",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "EmailCc",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "EmailTo",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "ExtendedEnabled",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "Html",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "IntroText",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "NameEnabled",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "ThankYouText",
                table: "Section");

            migrationBuilder.CreateTable(
                name: "Section_HtmlContent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Html = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section_HtmlContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Section_HtmlContent_Section_Id",
                        column: x => x.Id,
                        principalTable: "Section",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Section_HtmlContent");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserName",
                table: "Section",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Section",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmailBcc",
                table: "Section",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailCc",
                table: "Section",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailTo",
                table: "Section",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ExtendedEnabled",
                table: "Section",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Html",
                table: "Section",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IntroText",
                table: "Section",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NameEnabled",
                table: "Section",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Section",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThankYouText",
                table: "Section",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    FlagImageFileName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LanguageCulture = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    Rtl = table.Column<bool>(type: "bit", nullable: false),
                    UniqueSeoCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resume",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkedIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Town = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resume", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocaleStringResource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    ResourceName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ResourceValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocaleStringResource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocaleStringResource_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocalizedProperty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    LocaleKey = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    LocaleKeyGroup = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    LocaleValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizedProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocalizedProperty_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Education",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResumeId = table.Column<int>(type: "int", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Education_Resume_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Experience",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Function = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Period = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResumeId = table.Column<int>(type: "int", nullable: false),
                    Tasks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experience", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Experience_Resume_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResumeId = table.Column<int>(type: "int", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skill_Resume_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EducationItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Period = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationItem_Education_EducationId",
                        column: x => x.EducationId,
                        principalTable: "Education",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExperienceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Technology = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_Experience_ExperienceId",
                        column: x => x.ExperienceId,
                        principalTable: "Experience",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    LevelDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillItem_Skill_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ResumeId",
                table: "AspNetUsers",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_Education_ResumeId",
                table: "Education",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationItem_EducationId",
                table: "EducationItem",
                column: "EducationId");

            migrationBuilder.CreateIndex(
                name: "IX_Experience_ResumeId",
                table: "Experience",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_LocaleStringResource_LanguageId",
                table: "LocaleStringResource",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_LocalizedProperty_LanguageId",
                table: "LocalizedProperty",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ExperienceId",
                table: "Project",
                column: "ExperienceId");

            migrationBuilder.CreateIndex(
                name: "IX_Skill_ResumeId",
                table: "Skill",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillItem_SkillId",
                table: "SkillItem",
                column: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Resume_ResumeId",
                table: "AspNetUsers",
                column: "ResumeId",
                principalTable: "Resume",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
