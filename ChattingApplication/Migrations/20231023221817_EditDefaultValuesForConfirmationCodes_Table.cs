using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChattingApplication.Migrations
{
    /// <inheritdoc />
    public partial class EditDefaultValuesForConfirmationCodes_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "RemainingCodesForThisDay",
                table: "ConfirmationCodes",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)4,
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned",
                oldDefaultValue: (byte)5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "RemainingCodesForThisDay",
                table: "ConfirmationCodes",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)5,
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned",
                oldDefaultValue: (byte)4);
        }
    }
}
