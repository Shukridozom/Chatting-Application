using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChattingApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddRemainingCodesForThisDay_ColumnFor_ConfirmationCodes_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "RemainingCodesForThisDay",
                table: "ConfirmationCodes",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingCodesForThisDay",
                table: "ConfirmationCodes");
        }
    }
}
