namespace ScreenSound.Dominio.Models.Musicas.Dto;

public class ListagemDeMusicas
{
    public string Nome { get; set; }
    public short Duracao { get; set; }
    public bool? Disponivel { get; set; }
    public string Album { get; set; }
    public string Banda { get; set; }
    public string? Imagem { get; set; }
}
