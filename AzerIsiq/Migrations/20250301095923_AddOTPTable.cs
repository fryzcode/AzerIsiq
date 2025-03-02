using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzerIsiq.Migrations
{
    /// <inheritdoc />
    public partial class AddOTPTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OtpCode_Users_UserId",
                table: "OtpCode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OtpCode",
                table: "OtpCode");

            migrationBuilder.RenameTable(
                name: "OtpCode",
                newName: "OtpCodes");

            migrationBuilder.RenameIndex(
                name: "IX_OtpCode_UserId",
                table: "OtpCodes",
                newName: "IX_OtpCodes_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OtpCodes",
                table: "OtpCodes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OtpCodes_Users_UserId",
                table: "OtpCodes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OtpCodes_Users_UserId",
                table: "OtpCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OtpCodes",
                table: "OtpCodes");

            migrationBuilder.RenameTable(
                name: "OtpCodes",
                newName: "OtpCode");

            migrationBuilder.RenameIndex(
                name: "IX_OtpCodes_UserId",
                table: "OtpCode",
                newName: "IX_OtpCode_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OtpCode",
                table: "OtpCode",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OtpCode_Users_UserId",
                table: "OtpCode",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
