using ScreenSound.Dominio.Models.Musicas.Dto;

namespace ScreenSound.Dominio.Interfaces.Consultas;

public interface IConsultaListagemDeMusicas
{
    Task<IEnumerable<ListagemDeMusicas>> RetornaListagemDeMusicas(string? nomeMusica = null, int skip = 0, int take = 10);
    Task<ListagemDeMusicas> RetornaMusicaPorId(int id);
}