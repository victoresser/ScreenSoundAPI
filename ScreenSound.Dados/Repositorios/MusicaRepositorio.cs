using Microsoft.EntityFrameworkCore;
using ScreenSound.Dominio.Models.Musicas;
using ScreenSound.Dominio.Services.Repositorios;

namespace ScreenSound.Dados.Repositorios;

public class MusicaRepositorio : RepositorioBase<Musica>, IMusicaRepositorio
{
    public MusicaRepositorio(ScreenSoundContext context) : base(context)
    {
    }

    public override async Task<List<Musica>> ConsultarAsync()
    {
        var query = Context.Set<Musica>()
            .Include(x => x.Banda)
            .Include(x => x.Album)
            .ToListAsync();

        return await query;
    }

    public virtual async Task<List<Musica>> ObterPorDataDeCriacaoAsync(DateOnly data)
    {
        var query = Context.Set<Musica>()
            .Include(x => x.Banda)
            .Include(x => x.Album)
            .Where(x => x.DataDeCriacao.Year == data.Year)
            .ToListAsync();

        return await query;
    }

    public async Task<Musica?> ObterPorNome(string nome)
    {
        var query = Context.Set<Musica>()
            .Include(x => x.Banda) 
            .Include(x => x.Album)
            .Where(x => x.Nome.Equals(nome))
            .FirstOrDefaultAsync();

        return await query;
    }
    
    public async Task<Musica?> ObterPorId(int id)
    {
        var query = Context.Set<Musica>()
            .Include(x => x.Banda) 
            .Include(x => x.Album)
            .Where(x => x.Id.Equals(id))
            .FirstOrDefaultAsync();

        return await query;
    }

    
}
