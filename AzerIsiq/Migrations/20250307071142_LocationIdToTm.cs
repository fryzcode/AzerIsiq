using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzerIsiq.Migrations
{
    /// <inheritdoc />
    public partial class LocationIdToTm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Tms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tms_LocationId",
                table: "Tms",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tms_Locations_LocationId",
                table: "Tms",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tms_Locations_LocationId",
                table: "Tms");

            migrationBuilder.DropIndex(
                name: "IX_Tms_LocationId",
                table: "Tms");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Tms");
        }
    }
}
