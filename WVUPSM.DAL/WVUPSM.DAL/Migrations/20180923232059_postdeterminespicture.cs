using Microsoft.EntityFrameworkCore.Migrations;

namespace WVUPSM.DAL.Migrations
{
    public partial class postdeterminespicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PicturePath",
                schema: "SM",
                table: "Posts",
                newName: "FilePath");

            migrationBuilder.AddColumn<bool>(
                name: "IsPicture",
                schema: "SM",
                table: "Posts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPicture",
                schema: "SM",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                schema: "SM",
                table: "Posts",
                newName: "PicturePath");
        }
    }
}
