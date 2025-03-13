using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzerIsiq.Migrations
{
    /// <inheritdoc />
    public partial class EditSubscriberTableDistrictColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "District",
                table: "Subscribers");

            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "Subscribers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Subscribers_DistrictId",
                table: "Subscribers",
                column: "DistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_Districts_DistrictId",
                table: "Subscribers",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_Districts_DistrictId",
                table: "Subscribers");

            migrationBuilder.DropIndex(
                name: "IX_Subscribers_DistrictId",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "Subscribers");

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Subscribers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
