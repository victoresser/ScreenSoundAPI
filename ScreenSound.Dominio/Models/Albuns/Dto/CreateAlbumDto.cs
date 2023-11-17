using ScreenSound.Dominio.Models.Bandas;

namespace ScreenSound.Dominio.Models.Albuns.Dto;

public class CreateAlbumDto
{
    public string Nome { get; set; }
    public Banda Banda { get; set; }
}
