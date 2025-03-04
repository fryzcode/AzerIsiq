using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzerIsiq.Migrations
{
    /// <inheritdoc />
    public partial class FixImageEntityDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImageEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubstationId = table.Column<int>(type: "int", nullable: true),
                    TmId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageEntities_Substations_SubstationId",
                        column: x => x.SubstationId,
                        principalTable: "Substations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImageEntities_Tms_TmId",
                        column: x => x.TmId,
                        principalTable: "Tms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageEntities_SubstationId",
                table: "ImageEntities",
                column: "SubstationId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageEntities_TmId",
                table: "ImageEntities",
                column: "TmId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageEntities");
        }
    }
}
