using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanShark.EF.Migrations
{
    /// <inheritdoc />
    public partial class FinalDBMessages8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SerializedUserIDsWhoReported",
                table: "Message",
                newName: "SerializedUserIDs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SerializedUserIDs",
                table: "Message",
                newName: "SerializedUserIDsWhoReported");
        }
    }
}
