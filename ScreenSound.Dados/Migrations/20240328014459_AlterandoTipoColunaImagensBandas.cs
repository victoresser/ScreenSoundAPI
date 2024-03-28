using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScreenSound.Dados.Migrations
{
    /// <inheritdoc />
    public partial class AlterandoTipoColunaImagensBandas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "caminhoDaImagem", 
                table:"bandas");
            
            migrationBuilder.AddColumn<byte[]>(
                name: "imagem",
                table: "bandas",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "descricao",
                table: "bandas",
                type: "varchar(5000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(5000)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imagem", 
                table:"bandas");
            
            migrationBuilder.AddColumn<byte[]>(
                name: "imagem",
                table: "bandas",
                nullable: true,
                type: "varchar(255)",
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "descricao",
                table: "bandas",
                type: "varchar(5000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(5000)",
                oldNullable: true);
        }
    }
}
