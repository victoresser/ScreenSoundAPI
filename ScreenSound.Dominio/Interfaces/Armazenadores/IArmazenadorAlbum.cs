using ScreenSound.Dominio.Models.Albuns.Dto;
using ScreenSound.Dominio.Models.Bandas;

namespace ScreenSound.Dominio.Interfaces.Armazenadores;

public interface IArmazenadorAlbum
{
    Task<string> Armazenar(string nome, Banda banda);
    Task<string> Editar(EditAlbumDto dto);
}
