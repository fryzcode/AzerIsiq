using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzerIsiq.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriberCodeToSubscriberModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AtsCode",
                table: "Subscribers",
                newName: "SubscriberCode");

            migrationBuilder.AddColumn<string>(
                name: "Ats",
                table: "Subscribers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ats",
                table: "Subscribers");

            migrationBuilder.RenameColumn(
                name: "SubscriberCode",
                table: "Subscribers",
                newName: "AtsCode");
        }
    }
}
