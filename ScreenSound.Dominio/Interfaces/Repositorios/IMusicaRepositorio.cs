using ScreenSound.Dominio.Models.Musicas;

namespace ScreenSound.Dominio.Interfaces.Repositorios;

public interface IMusicaRepositorio : IRepositorio<Musica>
{
    Task<List<Musica>> ObterPorDataDeCriacaoAsync(DateOnly data);
    Task<Musica?> ObterPorNome(string nome);
    Task<Musica?> ObterPorId(int id);
}
