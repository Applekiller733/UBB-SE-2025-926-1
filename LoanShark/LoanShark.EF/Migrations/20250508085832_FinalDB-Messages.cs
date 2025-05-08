using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanShark.EF.Migrations
{
    /// <inheritdoc />
    public partial class FinalDBMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MessageTypeEF",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageTypeEF", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "MessageEF",
                columns: table => new
                {
                    MessageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ChatID = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Content = table.Column<string>(type: "NVARCHAR(260)", maxLength: 260, nullable: false),
                    Status = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    Amount = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: true),
                    Currency = table.Column<string>(type: "VARCHAR(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageEF", x => x.MessageID);
                    table.ForeignKey(
                        name: "FK_MessageEF_Chat_ChatID",
                        column: x => x.ChatID,
                        principalTable: "Chat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageEF_MessageTypeEF_TypeID",
                        column: x => x.TypeID,
                        principalTable: "MessageTypeEF",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MessageEF_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageEF_ChatID",
                table: "MessageEF",
                column: "ChatID");

            migrationBuilder.CreateIndex(
                name: "IX_MessageEF_TypeID",
                table: "MessageEF",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_MessageEF_UserID",
                table: "MessageEF",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageEF");

            migrationBuilder.DropTable(
                name: "MessageTypeEF");
        }
    }
}
