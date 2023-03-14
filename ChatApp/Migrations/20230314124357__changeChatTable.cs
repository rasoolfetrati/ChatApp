using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Migrations
{
    public partial class _changeChatTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_ConversationRooms_conversationRoomRoomName",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_conversationRoomRoomName",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "RoomName",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "conversationRoomRoomName",
                table: "Chats");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoomName",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "conversationRoomRoomName",
                table: "Chats",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_conversationRoomRoomName",
                table: "Chats",
                column: "conversationRoomRoomName");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_ConversationRooms_conversationRoomRoomName",
                table: "Chats",
                column: "conversationRoomRoomName",
                principalTable: "ConversationRooms",
                principalColumn: "RoomName",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
