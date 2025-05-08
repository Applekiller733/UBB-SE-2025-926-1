using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanShark.EF.Migrations
{
    /// <inheritdoc />
    public partial class FinalDBReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Report_MessageID",
                table: "Report",
                column: "MessageID");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Message_MessageID",
                table: "Report",
                column: "MessageID",
                principalTable: "Message",
                principalColumn: "MessageID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_Message_MessageID",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Report_MessageID",
                table: "Report");
        }
    }
}
