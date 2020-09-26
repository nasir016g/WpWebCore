using Microsoft.EntityFrameworkCore.Migrations;

namespace Wp.Data.Migrations.Frontend
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebPage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    VirtualPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebPage", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebPage");
        }
    }
}
