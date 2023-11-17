using ScreenSound.Dominio.Models.Albuns.Dto;

namespace ScreenSound.Dominio.Models.Bandas.Dto;

public class ListagemDeBandas
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public ICollection<string> Albuns { get; set; }
}
