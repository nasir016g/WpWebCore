using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nsr.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommonLanguage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LanguageCulture = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UniqueSeoCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    FlagImageFileName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Rtl = table.Column<bool>(type: "bit", nullable: false),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonLanguage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommonSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommonLocaleStringResource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ResourceValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonLocaleStringResource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommonLocaleStringResource_CommonLanguage_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "CommonLanguage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommonLocalizedProperty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    LocaleKeyGroup = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    LocaleKey = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    LocaleValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonLocalizedProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommonLocalizedProperty_CommonLanguage_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "CommonLanguage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommonLocaleStringResource_LanguageId",
                table: "CommonLocaleStringResource",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_CommonLocalizedProperty_LanguageId",
                table: "CommonLocalizedProperty",
                column: "LanguageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommonLocaleStringResource");

            migrationBuilder.DropTable(
                name: "CommonLocalizedProperty");

            migrationBuilder.DropTable(
                name: "CommonSetting");

            migrationBuilder.DropTable(
                name: "CommonLanguage");
        }
    }
}
