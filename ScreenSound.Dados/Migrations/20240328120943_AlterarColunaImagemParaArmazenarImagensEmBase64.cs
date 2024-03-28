using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScreenSound.Dados.Migrations
{
    /// <inheritdoc />
    public partial class AlterarColunaImagemParaArmazenarImagensEmBase64 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "caminhoDaImagem", 
                table:"musicas");

            migrationBuilder.DropColumn(
                name: "caminhoDaImagem", 
                table:"albuns");

            migrationBuilder.AddColumn<byte[]>(
                name: "imagem",
                table: "musicas",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "imagem",
                table: "albuns",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imagem", 
                table:"musicas");

            migrationBuilder.DropColumn(
                name: "imagem", 
                table:"albuns");

            migrationBuilder.AddColumn<string>(
                name: "imagem",
                table: "musicas",
                type: "varchar(255)",
                defaultValue: "",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "imagem",
                table: "albuns",
                type: "varchar(255)",
                defaultValue: "",
                nullable: true);
        }
    }
}
