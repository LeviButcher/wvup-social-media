using Microsoft.EntityFrameworkCore.Migrations;

namespace WVUPSM.DAL.Migrations
{
    public partial class messagecompkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                schema: "SM",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                schema: "SM",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                schema: "SM",
                table: "Messages",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                schema: "SM",
                table: "Messages",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverId",
                schema: "SM",
                table: "Messages",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Messages_Id",
                schema: "SM",
                table: "Messages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                schema: "SM",
                table: "Messages",
                columns: new[] { "Id", "ReceiverId", "SenderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                schema: "SM",
                table: "Messages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                schema: "SM",
                table: "Messages");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Messages_Id",
                schema: "SM",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                schema: "SM",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                schema: "SM",
                table: "Messages",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                schema: "SM",
                table: "Messages",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverId",
                schema: "SM",
                table: "Messages",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                schema: "SM",
                table: "Messages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                schema: "SM",
                table: "Messages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
