using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanShark.EF.Migrations
{
    /// <inheritdoc />
    public partial class FinalDB2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ChatUserEF_UserId",
                table: "ChatUserEF",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUserEF_User_UserId",
                table: "ChatUserEF",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatUserEF_User_UserId",
                table: "ChatUserEF");

            migrationBuilder.DropIndex(
                name: "IX_ChatUserEF_UserId",
                table: "ChatUserEF");
        }
    }
}
