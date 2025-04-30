using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzerIsiq.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCounterRelationWithSubscribers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_Counters_CounterId",
                table: "Subscribers");

            migrationBuilder.DropIndex(
                name: "IX_Subscribers_CounterId",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "CounterId",
                table: "Subscribers");

            migrationBuilder.AddColumn<int>(
                name: "SubscriberId",
                table: "Counters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Counters_SubscriberId",
                table: "Counters",
                column: "SubscriberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Counters_Subscribers_SubscriberId",
                table: "Counters",
                column: "SubscriberId",
                principalTable: "Subscribers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Counters_Subscribers_SubscriberId",
                table: "Counters");

            migrationBuilder.DropIndex(
                name: "IX_Counters_SubscriberId",
                table: "Counters");

            migrationBuilder.DropColumn(
                name: "SubscriberId",
                table: "Counters");

            migrationBuilder.AddColumn<int>(
                name: "CounterId",
                table: "Subscribers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscribers_CounterId",
                table: "Subscribers",
                column: "CounterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_Counters_CounterId",
                table: "Subscribers",
                column: "CounterId",
                principalTable: "Counters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
