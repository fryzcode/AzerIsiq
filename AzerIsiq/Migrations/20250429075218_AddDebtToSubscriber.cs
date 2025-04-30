using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzerIsiq.Migrations
{
    /// <inheritdoc />
    public partial class AddDebtToSubscriber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Debt",
                table: "Subscribers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CurrentValue",
                table: "Counters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Debt",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "CurrentValue",
                table: "Counters");
        }
    }
}
