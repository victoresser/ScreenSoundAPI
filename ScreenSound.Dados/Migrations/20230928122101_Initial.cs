using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScreenSound.Dados.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bandas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(255)", nullable: false),
                    descricao = table.Column<string>(type: "varchar(5000)", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bandas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "albuns",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(255)", nullable: false),
                    banda_id = table.Column<int>(type: "int", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_albuns", x => x.id);
                    table.ForeignKey(
                        name: "FK_albuns_bandas_banda_id",
                        column: x => x.banda_id,
                        principalTable: "bandas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "musicas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(255)", nullable: false),
                    duracao = table.Column<short>(type: "smallint", nullable: false),
                    disponivel = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    data_criacao = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    album_id = table.Column<int>(type: "int", nullable: false),
                    banda_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_musicas", x => x.id);
                    table.ForeignKey(
                        name: "FK_musicas_albuns_album_id",
                        column: x => x.album_id,
                        principalTable: "albuns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_musicas_bandas_banda_id",
                        column: x => x.banda_id,
                        principalTable: "bandas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_albuns_banda_id",
                table: "albuns",
                column: "banda_id");

            migrationBuilder.CreateIndex(
                name: "IX_musicas_album_id",
                table: "musicas",
                column: "album_id");

            migrationBuilder.CreateIndex(
                name: "IX_musicas_banda_id",
                table: "musicas",
                column: "banda_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "musicas");

            migrationBuilder.DropTable(
                name: "albuns");

            migrationBuilder.DropTable(
                name: "bandas");
        }
    }
}
