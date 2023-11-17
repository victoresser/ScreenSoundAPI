using ScreenSound.Dominio.Models.Albuns;
using ScreenSound.Dominio.Models.Bandas;

namespace ScreenSound.Dominio.Services.Armazenadores;

public interface IArmazenadorMusica
{
    Task<string> Armazenar(string nome, short duracao, Banda banda, Album album, bool? disponivel = true, string? imagem = "");
    Task<string> Editar(int id, string? nome, string? nomeArtista, string? nomeAlbum, short? duracaoMusica, bool disponibilidade, string? imagem);
}
