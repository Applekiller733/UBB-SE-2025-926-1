using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanShark.EF.Migrations
{
    /// <inheritdoc />
    public partial class FinalDBMessages5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageEF_Chat_ChatID",
                table: "MessageEF");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageEF_MessageTypeEF_TypeID",
                table: "MessageEF");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageEF_User_UserID",
                table: "MessageEF");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageTypeEF",
                table: "MessageTypeEF");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageEF",
                table: "MessageEF");

            migrationBuilder.RenameTable(
                name: "MessageTypeEF",
                newName: "MessageType");

            migrationBuilder.RenameTable(
                name: "MessageEF",
                newName: "Message");

            migrationBuilder.RenameIndex(
                name: "IX_MessageEF_UserID",
                table: "Message",
                newName: "IX_Message_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_MessageEF_TypeID",
                table: "Message",
                newName: "IX_Message_TypeID");

            migrationBuilder.RenameIndex(
                name: "IX_MessageEF_ChatID",
                table: "Message",
                newName: "IX_Message_ChatID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageType",
                table: "MessageType",
                column: "TypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                column: "MessageID");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Chat_ChatID",
                table: "Message",
                column: "ChatID",
                principalTable: "Chat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_MessageType_TypeID",
                table: "Message",
                column: "TypeID",
                principalTable: "MessageType",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_UserID",
                table: "Message",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Chat_ChatID",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_MessageType_TypeID",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_UserID",
                table: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageType",
                table: "MessageType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.RenameTable(
                name: "MessageType",
                newName: "MessageTypeEF");

            migrationBuilder.RenameTable(
                name: "Message",
                newName: "MessageEF");

            migrationBuilder.RenameIndex(
                name: "IX_Message_UserID",
                table: "MessageEF",
                newName: "IX_MessageEF_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Message_TypeID",
                table: "MessageEF",
                newName: "IX_MessageEF_TypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Message_ChatID",
                table: "MessageEF",
                newName: "IX_MessageEF_ChatID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageTypeEF",
                table: "MessageTypeEF",
                column: "TypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageEF",
                table: "MessageEF",
                column: "MessageID");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageEF_Chat_ChatID",
                table: "MessageEF",
                column: "ChatID",
                principalTable: "Chat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageEF_MessageTypeEF_TypeID",
                table: "MessageEF",
                column: "TypeID",
                principalTable: "MessageTypeEF",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageEF_User_UserID",
                table: "MessageEF",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
