using ScreenSound.Dominio.Models.Bandas;

namespace ScreenSound.Dominio.Interfaces.Repositorios;

public interface IBandaRepositorio : IRepositorio<Banda>
{
    Task<Banda> ObterPorIdAsync(int id);
    Task<Banda?> ObterPorNome(string? nome);
}
