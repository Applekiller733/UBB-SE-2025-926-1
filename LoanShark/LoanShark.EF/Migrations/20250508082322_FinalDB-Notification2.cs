using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanShark.EF.Migrations
{
    /// <inheritdoc />
    public partial class FinalDBNotification2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatUserEF_Chat_ChatId",
                table: "ChatUserEF");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatUserEF_User_UserId",
                table: "ChatUserEF");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatUserEF",
                table: "ChatUserEF");

            migrationBuilder.RenameTable(
                name: "ChatUserEF",
                newName: "ChatUser");

            migrationBuilder.RenameIndex(
                name: "IX_ChatUserEF_UserId",
                table: "ChatUser",
                newName: "IX_ChatUser_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatUser",
                table: "ChatUser",
                columns: new[] { "ChatId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserReceiverID",
                table: "Notification",
                column: "UserReceiverID");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUser_Chat_ChatId",
                table: "ChatUser",
                column: "ChatId",
                principalTable: "Chat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUser_User_UserId",
                table: "ChatUser",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_User_UserReceiverID",
                table: "Notification",
                column: "UserReceiverID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatUser_Chat_ChatId",
                table: "ChatUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatUser_User_UserId",
                table: "ChatUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_User_UserReceiverID",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_UserReceiverID",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatUser",
                table: "ChatUser");

            migrationBuilder.RenameTable(
                name: "ChatUser",
                newName: "ChatUserEF");

            migrationBuilder.RenameIndex(
                name: "IX_ChatUser_UserId",
                table: "ChatUserEF",
                newName: "IX_ChatUserEF_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatUserEF",
                table: "ChatUserEF",
                columns: new[] { "ChatId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUserEF_Chat_ChatId",
                table: "ChatUserEF",
                column: "ChatId",
                principalTable: "Chat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUserEF_User_UserId",
                table: "ChatUserEF",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
