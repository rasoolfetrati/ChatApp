using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Migrations
{
    public partial class _changeDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersConnectionIds_Users_UserId",
                table: "UsersConnectionIds");

            migrationBuilder.DropIndex(
                name: "IX_UsersConnectionIds_UserId",
                table: "UsersConnectionIds");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "UsersConnectionIds");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UsersConnectionIds",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "UsersConnectionIds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UsersConnectionIds_UserId1",
                table: "UsersConnectionIds",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersConnectionIds_Users_UserId1",
                table: "UsersConnectionIds",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersConnectionIds_Users_UserId1",
                table: "UsersConnectionIds");

            migrationBuilder.DropIndex(
                name: "IX_UsersConnectionIds_UserId1",
                table: "UsersConnectionIds");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UsersConnectionIds");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UsersConnectionIds",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "UsersConnectionIds",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UsersConnectionIds_UserId",
                table: "UsersConnectionIds",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersConnectionIds_Users_UserId",
                table: "UsersConnectionIds",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
