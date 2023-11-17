using Microsoft.EntityFrameworkCore;
using ScreenSound.Dados.Configurations;
using ScreenSound.Dominio.Models.Albuns;
using ScreenSound.Dominio.Models.Bandas;
using ScreenSound.Dominio.Models.Musicas;

namespace ScreenSound.Dados;

public class ScreenSoundContext : DbContext
{
    public ScreenSoundContext(DbContextOptions<ScreenSoundContext> opts) : base(opts)
    {

    }

    public DbSet<Banda> Bandas { get; set; }
    public DbSet<Musica> Musicas { get; set; }
    public DbSet<Album> Albums { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        // Banda
        modelBuilder.ApplyConfiguration(new BandaConfiguration());

        // Música
        modelBuilder.ApplyConfiguration(new MusicaConfiguration());

        // Album
        modelBuilder.ApplyConfiguration(new  AlbumConfiguration());
    }
}
