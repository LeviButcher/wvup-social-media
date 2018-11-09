using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WVUPSM.DAL.Migrations
{
    public partial class FileTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                schema: "SM",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "FilePath",
                schema: "SM",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IsPicture",
                schema: "SM",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                schema: "SM",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Files",
                schema: "SM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FileName = table.Column<string>(maxLength: 255, nullable: true),
                    ContentType = table.Column<string>(maxLength: 100, nullable: true),
                    Content = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_FileId",
                schema: "SM",
                table: "Posts",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FileId",
                table: "AspNetUsers",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Files_FileId",
                table: "AspNetUsers",
                column: "FileId",
                principalSchema: "SM",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Files_FileId",
                schema: "SM",
                table: "Posts",
                column: "FileId",
                principalSchema: "SM",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Files_FileId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Files_FileId",
                schema: "SM",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "Files",
                schema: "SM");

            migrationBuilder.DropIndex(
                name: "IX_Posts_FileId",
                schema: "SM",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FileId",
                schema: "SM",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                schema: "SM",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                schema: "SM",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPicture",
                schema: "SM",
                table: "Posts",
                nullable: false,
                defaultValue: false);
        }
    }
}
