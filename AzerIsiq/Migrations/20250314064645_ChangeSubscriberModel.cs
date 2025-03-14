using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzerIsiq.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSubscriberModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Cities_CityId",
                table: "Districts");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_Cities_CityId",
                table: "Subscribers");

            migrationBuilder.DropIndex(
                name: "IX_Districts_CityId",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Districts");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "Subscribers",
                newName: "RegionId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribers_CityId",
                table: "Subscribers",
                newName: "IX_Subscribers_RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_Regions_RegionId",
                table: "Subscribers",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_Regions_RegionId",
                table: "Subscribers");

            migrationBuilder.RenameColumn(
                name: "RegionId",
                table: "Subscribers",
                newName: "CityId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribers_RegionId",
                table: "Subscribers",
                newName: "IX_Subscribers_CityId");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Districts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CityId",
                table: "Districts",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Cities_CityId",
                table: "Districts",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_Cities_CityId",
                table: "Subscribers",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
