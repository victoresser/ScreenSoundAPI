using Newtonsoft.Json.Serialization;

namespace ScreenSound.Dominio.Models.Musicas.Dto;

public class EditMusicaDto
{
    public int Id { get; set; }
    public string? NomeMusica { get; set; }
    public string? nomeBanda { get; set; }
    public string? nomeAlbum { get; set; }
    public short? Duracao { get; set; }
    public bool DisponibilidadeMusica { get; set; }
    public string? Imagem { get; set; }
}