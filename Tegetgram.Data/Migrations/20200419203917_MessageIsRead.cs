using Microsoft.EntityFrameworkCore.Migrations;

namespace Tegetgram.Data.Migrations
{
    public partial class MessageIsRead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsNew",
                table: "Messages",
                newName: "IsRead");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsRead",
                table: "Messages",
                newName: "IsNew");
        }
    }
}
