using Microsoft.EntityFrameworkCore;
using ScreenSound.Dominio.Interfaces.Repositorios;
using ScreenSound.Dominio.Models.Musicas;

namespace ScreenSound.Dados.Repositorios;

public class MusicaRepositorio : RepositorioBase<Musica>, IMusicaRepositorio
{
    public MusicaRepositorio(ScreenSoundContext context) : base(context)
    {
    }

    public override async Task<List<Musica>> ConsultarAsync()
    {
        return await Context.Set<Musica>()
            .Include(x => x.Banda)
            .Include(x => x.Album)
            .ToListAsync();
    }

    public virtual async Task<List<Musica>> ObterPorDataDeCriacaoAsync(DateOnly data)
    {
        return await Context.Set<Musica>()
            .Include(x => x.Banda)
            .Include(x => x.Album)
            .Where(x => x.DataDeCriacao.Year == data.Year)
            .ToListAsync();
    }

    public async Task<Musica?> ObterPorNome(string nome)
    {
        return await Context.Set<Musica>()
            .Include(x => x.Banda)
            .Include(x => x.Album)
            .Where(x => x.Nome.Equals(nome))
            .FirstOrDefaultAsync();
    }

    public async Task<Musica?> ObterPorId(int id)
    {
        return await Context.Set<Musica>()
            .Include(x => x.Banda)
            .Include(x => x.Album)
            .Where(x => x.Id.Equals(id))
            .FirstOrDefaultAsync();
    }
}