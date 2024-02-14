namespace ScreenSound.Dominio.Models.Musicas.Dto;

public class CreateMusicaDto
{
    public string Nome { get; set; }
    public string NomeAlbum { get; set; }
    public string NomeBanda { get; set; }
    public short Duracao { get; set; }
    public bool Disponibilidade { get; set; }
    public string? Imagem { get; set; }
}