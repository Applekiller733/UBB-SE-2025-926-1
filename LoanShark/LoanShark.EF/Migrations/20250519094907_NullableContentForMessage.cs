using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanShark.EF.Migrations
{
    /// <inheritdoc />
    public partial class NullableContentForMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Message",
                type: "NVARCHAR(260)",
                maxLength: 260,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(260)",
                oldMaxLength: 260);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Message",
                type: "NVARCHAR(260)",
                maxLength: 260,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(260)",
                oldMaxLength: 260,
                oldNullable: true);
        }
    }
}
