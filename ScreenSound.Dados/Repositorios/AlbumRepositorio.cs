using Microsoft.EntityFrameworkCore;
using ScreenSound.Dominio.Interfaces.Repositorios;
using ScreenSound.Dominio.Models.Albuns;

namespace ScreenSound.Dados.Repositorios;

public class AlbumRepositorio : RepositorioBase<Album>, IAlbumRepositorio
{
    public AlbumRepositorio(ScreenSoundContext context) : base(context)
    {
    }

    public override async Task<List<Album>> ConsultarAsync()
    {
        return await Context.Set<Album>()
            .Include(x => x.Banda)
            .Include(x => x.MusicasDoAlbum)
            .ToListAsync();
    }

    public async Task<List<Album>> ObterPorDataDeCriacao(DateTime data)
    {
        return await Context.Set<Album>()
            .Include(x => x.Banda)
            .Include(x => x.MusicasDoAlbum)
            .Where(x => x.DataDeCriacao.Year == data.Year)
            .ToListAsync();
        ;
    }

    public async Task<Album?> ObterPorNome(string? nome)
    {
        return await Context.Set<Album>()
            .Include(x => x.Banda)
            .Include(x => x.MusicasDoAlbum)
            .Where(x => x.Nome == nome)
            .FirstOrDefaultAsync();
    }

    public async Task<Album?> ObterPorId(int id)
    {
        return await Context.Set<Album>()
            .Include(x => x.Banda)
            .Include(x => x.MusicasDoAlbum)
            .Where(x => x.Id.Equals(id))
            .FirstOrDefaultAsync();
    }
}