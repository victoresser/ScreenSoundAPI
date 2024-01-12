using System.Globalization;

namespace ScreenSound.Dominio.Models.Albuns.Dto;

public class EditAlbumDto
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? NomeBanda { get; set; }
    public string? Imagem { get; set; }
}