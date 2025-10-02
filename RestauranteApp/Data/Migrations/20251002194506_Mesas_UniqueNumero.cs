using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestauranteApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class Mesas_UniqueNumero : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservas_MesaId",
                table: "Reservas");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_MesaId_DataHora",
                table: "Reservas",
                columns: new[] { "MesaId", "DataHora" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mesas_Numero",
                table: "Mesas",
                column: "Numero",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservas_MesaId_DataHora",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Mesas_Numero",
                table: "Mesas");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_MesaId",
                table: "Reservas",
                column: "MesaId");
        }
    }
}
