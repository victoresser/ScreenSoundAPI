using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScreenSound.Dados.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoImagens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "caminhoDaImagem",
                table: "musicas",
                type: "varchar(255)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "caminhoDaImagem",
                table: "bandas",
                type: "varchar(255)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "caminhoDaImagem",
                table: "albuns",
                type: "varchar(255)",
                nullable: true,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "caminhoDaImagem",
                table: "musicas");

            migrationBuilder.DropColumn(
                name: "caminhoDaImagem",
                table: "bandas");

            migrationBuilder.DropColumn(
                name: "caminhoDaImagem",
                table: "albuns");
        }
    }
}
