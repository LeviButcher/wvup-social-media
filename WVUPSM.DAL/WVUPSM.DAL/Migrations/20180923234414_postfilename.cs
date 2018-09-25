using Microsoft.EntityFrameworkCore.Migrations;

namespace WVUPSM.DAL.Migrations
{
    public partial class postfilename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                schema: "SM",
                table: "Posts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                schema: "SM",
                table: "Posts");
        }
    }
}
