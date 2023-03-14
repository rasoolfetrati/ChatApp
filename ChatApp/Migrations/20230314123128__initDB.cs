using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Migrations
{
    public partial class _initDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConversationRooms",
                columns: table => new
                {
                    RoomName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    User1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    User2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationRooms", x => x.RoomName);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserStatus = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    ChatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ReciverId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoomName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    conversationRoomRoomName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.ChatId);
                    table.ForeignKey(
                        name: "FK_Chats_ConversationRooms_conversationRoomRoomName",
                        column: x => x.conversationRoomRoomName,
                        principalTable: "ConversationRooms",
                        principalColumn: "RoomName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chats_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConversationRoomUser",
                columns: table => new
                {
                    RoomsRoomName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsersUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationRoomUser", x => new { x.RoomsRoomName, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_ConversationRoomUser_ConversationRooms_RoomsRoomName",
                        column: x => x.RoomsRoomName,
                        principalTable: "ConversationRooms",
                        principalColumn: "RoomName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConversationRoomUser_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersConnectionIds",
                columns: table => new
                {
                    ConsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConnectionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersConnectionIds", x => x.ConsId);
                    table.ForeignKey(
                        name: "FK_UsersConnectionIds_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_conversationRoomRoomName",
                table: "Chats",
                column: "conversationRoomRoomName");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_UserId",
                table: "Chats",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationRoomUser_UsersUserId",
                table: "ConversationRoomUser",
                column: "UsersUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersConnectionIds_UserId",
                table: "UsersConnectionIds",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "ConversationRoomUser");

            migrationBuilder.DropTable(
                name: "UsersConnectionIds");

            migrationBuilder.DropTable(
                name: "ConversationRooms");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
