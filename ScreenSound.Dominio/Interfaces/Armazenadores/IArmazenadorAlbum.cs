using ScreenSound.Dominio.Models.Albuns.Dto;
using ScreenSound.Dominio.Models.Bandas;

namespace ScreenSound.Dominio.Interfaces.Armazenadores;

public interface IArmazenadorAlbum
{
    Task<string> Armazenar(CreateAlbumDto dto);
    Task<string> Editar(EditAlbumDto dto);
}
