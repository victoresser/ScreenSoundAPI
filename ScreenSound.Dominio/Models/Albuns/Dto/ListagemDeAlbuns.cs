using ScreenSound.Dominio.Models.Musicas.Dto;

namespace ScreenSound.Dominio.Models.Albuns.Dto;

public class ListagemDeAlbuns
{
    public string Nome { get; set; }
    public string Banda { get; set; }
    public ICollection<string>? Musicas { get; set; }
}
