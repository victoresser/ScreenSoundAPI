using ScreenSound.Dominio.Models.Bandas;

namespace ScreenSound.Dominio.Models.Albuns.Dto;

public class CreateAlbumDto
{
    public string? Nome { get; set; }
    public string? NomeBanda { get; set; }
    public string? Imagem { get; set; }
}