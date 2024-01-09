using ScreenSound.Dominio.Models.Albuns;
using ScreenSound.Dominio.Models.Bandas;
using ScreenSound.Dominio.Models.Musicas.Dto;

namespace ScreenSound.Dominio.Services.Armazenadores;

public interface IArmazenadorMusica
{
    Task<string> Armazenar(CreateMusicaDto dto, Album album, Banda banda);
    Task<string> Editar(int id, string? nome, string? nomeArtista, string? nomeAlbum, short? duracaoMusica, bool disponibilidade, string? imagem);
}
