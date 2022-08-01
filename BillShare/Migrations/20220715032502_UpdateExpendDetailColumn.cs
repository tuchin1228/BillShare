using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillShare.Migrations
{
    public partial class UpdateExpendDetailColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LineId",
                table: "ExpendDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LineId",
                table: "ExpendDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
