using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WVUPSM.DAL.Migrations
{
    public partial class Notifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                schema: "SM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    InteractingUserId = table.Column<string>(nullable: true),
                    CommentId = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Comments_CommentId",
                        column: x => x.CommentId,
                        principalSchema: "SM",
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_InteractingUserId",
                        column: x => x.InteractingUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CommentId",
                schema: "SM",
                table: "Notifications",
                column: "CommentId",
                unique: true,
                filter: "[CommentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_InteractingUserId",
                schema: "SM",
                table: "Notifications",
                column: "InteractingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                schema: "SM",
                table: "Notifications",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications",
                schema: "SM");
        }
    }
}
