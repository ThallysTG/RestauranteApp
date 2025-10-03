using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestauranteApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class PedidosBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Atendimentos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Atendimentos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
