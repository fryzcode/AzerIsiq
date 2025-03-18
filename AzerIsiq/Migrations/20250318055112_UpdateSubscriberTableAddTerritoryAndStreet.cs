using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzerIsiq.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubscriberTableAddTerritoryAndStreet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StreetId",
                table: "Subscribers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TerritoryId",
                table: "Subscribers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscribers_StreetId",
                table: "Subscribers",
                column: "StreetId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscribers_TerritoryId",
                table: "Subscribers",
                column: "TerritoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_Streets_StreetId",
                table: "Subscribers",
                column: "StreetId",
                principalTable: "Streets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_Territories_TerritoryId",
                table: "Subscribers",
                column: "TerritoryId",
                principalTable: "Territories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_Streets_StreetId",
                table: "Subscribers");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_Territories_TerritoryId",
                table: "Subscribers");

            migrationBuilder.DropIndex(
                name: "IX_Subscribers_StreetId",
                table: "Subscribers");

            migrationBuilder.DropIndex(
                name: "IX_Subscribers_TerritoryId",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "StreetId",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "TerritoryId",
                table: "Subscribers");
        }
    }
}
