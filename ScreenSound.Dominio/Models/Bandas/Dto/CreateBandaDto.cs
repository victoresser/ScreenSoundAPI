namespace ScreenSound.Dominio.Models.Bandas.Dto;

public class CreateBandaDto
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string? Imagem { get; set; }

    public CreateBandaDto(string nome, string descricao, string? imagem = "")
    {
        Nome = nome;
        Descricao = descricao;
        Imagem = imagem;
    }
}