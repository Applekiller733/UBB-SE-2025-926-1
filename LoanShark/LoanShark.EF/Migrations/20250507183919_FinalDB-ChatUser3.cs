using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanShark.EF.Migrations
{
    /// <inheritdoc />
    public partial class FinalDBChatUser3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friends");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatUserEF",
                table: "ChatUserEF");

            migrationBuilder.DropIndex(
                name: "IX_ChatUserEF_ChatId",
                table: "ChatUserEF");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ChatUserEF");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatUserEF",
                table: "ChatUserEF",
                columns: new[] { "ChatId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatUserEF",
                table: "ChatUserEF");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ChatUserEF",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatUserEF",
                table: "ChatUserEF",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    FriendID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => new { x.UserID, x.FriendID });
                    table.ForeignKey(
                        name: "FK_Friends_User_FriendID",
                        column: x => x.FriendID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friends_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatUserEF_ChatId",
                table: "ChatUserEF",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_FriendID",
                table: "Friends",
                column: "FriendID");
        }
    }
}
