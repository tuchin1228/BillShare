using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillShare.Migrations
{
    public partial class UpdateCheckoutColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkouts_Users_UserId",
                table: "Checkouts");

            migrationBuilder.DropIndex(
                name: "IX_Checkouts_UserId",
                table: "Checkouts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Checkouts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Checkouts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Checkouts_UserId",
                table: "Checkouts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Checkouts_Users_UserId",
                table: "Checkouts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
