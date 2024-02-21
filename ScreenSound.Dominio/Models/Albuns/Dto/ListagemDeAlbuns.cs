using ScreenSound.Dominio.Models.Musicas.Dto;

namespace ScreenSound.Dominio.Models.Albuns.Dto;

public class ListagemDeAlbuns
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Banda { get; set; }
    public int BandaId { get; set; }
    public ICollection<string>? Musicas { get; set; }
    public string? Imagem { get; set; }
}