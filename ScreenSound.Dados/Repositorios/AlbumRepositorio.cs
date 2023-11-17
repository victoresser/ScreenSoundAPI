using Microsoft.EntityFrameworkCore;
using ScreenSound.Dominio.Models.Albuns;
using ScreenSound.Dominio.Services.Repositorios;

namespace ScreenSound.Dados.Repositorios;

public class AlbumRepositorio : RepositorioBase<Album>, IAlbumRepositorio
{
    public AlbumRepositorio(ScreenSoundContext context) : base(context)
    {

    }

    public override async Task<List<Album>> Consultar()
    {
        var query = Context.Set<Album>()
            .Include(x => x.Banda)
            .Include(x => x.MusicasDoAlbum)
            .ToListAsync();

        if (query == null)
        {
            return new List<Album> { };
        }

        return await query;
    }

    public async Task<List<Album>> ObterPorDataDeCriacao(DateTime data)
    {
        var query = Context.Set<Album>()
            .Include(x => x.Banda)
            .Include(x => x.MusicasDoAlbum)
            .Where(x => x.DataDeCriacao.Year == data.Year)
            .ToListAsync();

        return await query;
    }

    public async Task<Album?> ObterPorNome(string nome)
    {
        var query = Context.Set<Album>()
            .Include(x => x.Banda)
            .Include(x => x.MusicasDoAlbum)
            .Where(x => x.Nome == nome)
            .FirstOrDefaultAsync();

        return await query;
    }

    public async Task<Album?> ObterPorId(int id)
    {
        var query = Context.Set<Album>()
            .Include(x => x.Banda)
            .Include(x => x.MusicasDoAlbum)
            .Where(x => x.Id.Equals(id))
            .FirstOrDefaultAsync();

        return await query;
    }
}
