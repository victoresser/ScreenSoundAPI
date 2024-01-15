using ScreenSound.Dominio.Models.Albuns;
using ScreenSound.Dominio.Models.Bandas;
using ScreenSound.Dominio.Models.Musicas.Dto;

namespace ScreenSound.Dominio.Services.Armazenadores;

public interface IArmazenadorMusica
{
    Task<string> Armazenar(CreateMusicaDto dto);
    Task<string> Editar(EditMusicaDto dto);
}
