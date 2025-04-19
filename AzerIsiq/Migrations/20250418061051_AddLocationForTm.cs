using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzerIsiq.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationForTm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tms_Locations_LocationId",
                table: "Tms");

            migrationBuilder.AddForeignKey(
                name: "FK_Tms_Locations_LocationId",
                table: "Tms",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tms_Locations_LocationId",
                table: "Tms");

            migrationBuilder.AddForeignKey(
                name: "FK_Tms_Locations_LocationId",
                table: "Tms",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id");
        }
    }
}
