using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nsr.ActivityLogs.Web.Migrations
{
    public partial class AddedActivityLogItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityLogItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityLogId = table.Column<int>(type: "int", nullable: false),
                    EntityKey = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLogItem_ActivityLog_ActivityLogId",
                        column: x => x.ActivityLogId,
                        principalTable: "ActivityLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogItem_ActivityLogId",
                table: "ActivityLogItem",
                column: "ActivityLogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogItem");
        }
    }
}
