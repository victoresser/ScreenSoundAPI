using ScreenSound.Dominio.Models.Albuns;

namespace ScreenSound.Dominio.Interfaces.Repositorios;

public interface IAlbumRepositorio : IRepositorio<Album>
{
    Task<List<Album>> ObterPorDataDeCriacao(DateTime data);
    Task<Album?> ObterPorId(int id);
    Task<Album?> ObterPorNome(string? nome);
}
