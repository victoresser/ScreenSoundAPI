using Microsoft.EntityFrameworkCore;
using ScreenSound.Dominio.Models.Bandas;
using ScreenSound.Dominio.Services.Repositorios;

namespace ScreenSound.Dados.Repositorios;

public class BandaRepositorio : RepositorioBase<Banda>, IBandaRepositorio
{
    public BandaRepositorio(ScreenSoundContext context) : base(context)
    {
    }

    public override async Task<List<Banda>?> Consultar()
    {
        var query = Context.Set<Banda>()
            .Include(x => x.AlbunsDaBanda)
            .Include(x => x.MusicasDaBanda)
            .ToListAsync();

        if (query == null)
        {
            return new List<Banda>();
        }

        return await query;
    }

    public async Task<Banda> ObterPorIdAsync(int id)
    {
        var query = Context.Set<Banda>()
            .Where(x => x.Id == id)
            .Include(x => x.AlbunsDaBanda)
            .Include(x => x.MusicasDaBanda)
            .FirstAsync();

        return await query;
    }

    public async Task<Banda> ObterPorNome(string nome)
    {
        var query = Context.Set<Banda>()
            .Where(x => x.Nome == nome)
            .Include(x => x.AlbunsDaBanda)
            .Include(x => x.MusicasDaBanda)
            .FirstOrDefaultAsync();

        return await query;
    }
}
