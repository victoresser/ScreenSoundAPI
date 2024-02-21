using Microsoft.EntityFrameworkCore;
using ScreenSound.Dominio.Interfaces.Repositorios;
using ScreenSound.Dominio.Models.Bandas;

namespace ScreenSound.Dados.Repositorios;

public class BandaRepositorio : RepositorioBase<Banda>, IBandaRepositorio
{
    public BandaRepositorio(ScreenSoundContext context) : base(context)
    {
    }

    public override async Task<List<Banda>> ConsultarAsync()
    {
        return await Context.Set<Banda>()
            .Include(x => x.AlbunsDaBanda)
            .Include(x => x.MusicasDaBanda)
            .ToListAsync();
    }

    public async Task<Banda> ObterPorIdAsync(int id)
    {
        return await Context.Set<Banda>()
            .Where(x => x.Id == id)
            .Include(x => x.AlbunsDaBanda)
            .Include(x => x.MusicasDaBanda)
            .FirstAsync();
    }

    public async Task<Banda?> ObterPorNome(string nome)
    {
        return await Context.Set<Banda>()
            .Where(x => x.Nome == nome)
            .Include(x => x.AlbunsDaBanda)
            .Include(x => x.MusicasDaBanda)
            .FirstOrDefaultAsync();
    }
}