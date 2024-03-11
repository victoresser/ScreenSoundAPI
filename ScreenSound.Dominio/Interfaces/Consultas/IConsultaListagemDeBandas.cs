using ScreenSound.Dominio.Models.Bandas.Dto;

namespace ScreenSound.Dominio.Interfaces.Consultas;

public interface IConsultaListagemDeBandas
{
    Task<IEnumerable<ListagemDeBandas>> RetornaListagemDeBandas(string? nomeBanda, int skip, int take);
    Task<ListagemDeBandas> RetornarBandaPorId(int id);
}