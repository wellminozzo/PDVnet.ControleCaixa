using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDVnet.ControleCaixa.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSaldoMinimo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SaldoMinimo",
                table: "ConfiguracoesCaixa",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaldoMinimo",
                table: "ConfiguracoesCaixa");
        }
    }
}
