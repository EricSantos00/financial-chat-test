using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialChat.Infrastructure.Data.Migrations
{
    public partial class ChangeUserIdToUserName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ChatMessages",
                newName: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "ChatMessages",
                newName: "UserId");
        }
    }
}
