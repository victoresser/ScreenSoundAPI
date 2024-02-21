using ScreenSound.Dominio.Models.Musicas.Dto;

namespace ScreenSound.Dominio.Interfaces.Armazenadores;

public interface IArmazenadorMusica
{
    Task<string> Armazenar(CreateMusicaDto dto);
    Task<string> Editar(EditMusicaDto dto);
}
