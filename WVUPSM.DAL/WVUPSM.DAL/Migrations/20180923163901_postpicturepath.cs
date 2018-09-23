﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace WVUPSM.DAL.Migrations
{
    public partial class postpicturepath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PicturePath",
                schema: "SM",
                table: "Posts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PicturePath",
                schema: "SM",
                table: "Posts");
        }
    }
}
