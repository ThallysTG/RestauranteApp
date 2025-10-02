using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestauranteApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SugestaoDoChefe_Base : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SugestoesDoChefe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateOnly>(type: "date", nullable: false),
                    Periodo = table.Column<int>(type: "int", nullable: false),
                    ItemCardapioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SugestoesDoChefe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SugestoesDoChefe_ItensCardapio_ItemCardapioId",
                        column: x => x.ItemCardapioId,
                        principalTable: "ItensCardapio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SugestoesDoChefe_Data_Periodo",
                table: "SugestoesDoChefe",
                columns: new[] { "Data", "Periodo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SugestoesDoChefe_ItemCardapioId",
                table: "SugestoesDoChefe",
                column: "ItemCardapioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SugestoesDoChefe");
        }
    }
}
