using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillShare.Migrations
{
    public partial class GroupAddBannerAndAnnouncement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupAnnouncement",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GroupBanner",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupAnnouncement",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GroupBanner",
                table: "Groups");
        }
    }
}
