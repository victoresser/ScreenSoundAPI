using Microsoft.Extensions.DependencyInjection;
using ScreenSound.Dados;
using ScreenSound.Dados.Repositorios;
using ScreenSound.Dominio.Models.Albuns.Dto;
using ScreenSound.Dominio.Models.Bandas.Dto;
using ScreenSound.Dominio.Models.Musicas.Dto;
using ScreenSound.Dominio.Services;
using ScreenSound.Dominio.Services.Armazenadores;
using ScreenSound.Dominio.Services.Conversores;
using ScreenSound.Dominio.Services.Repositorios;

namespace ScreenSound.Ioc;

public static class StartupIoc
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBandaRepositorio, BandaRepositorio>();
        services.AddScoped<IAlbumRepositorio, AlbumRepositorio>();
        services.AddScoped<IMusicaRepositorio, MusicaRepositorio>();
        services.AddScoped<IArmazenadorAlbum, ArmazenadorDeAlbuns>();
        services.AddScoped<IArmazenadorBanda, ArmazenadorDeBandas>();
        services.AddScoped<IArmazenadorMusica, ArmazenadorDeMusicas>();
        services.AddScoped<IConversorAlbunsDaBanda, ConversorAlbunsDaBanda>();
        services.AddScoped<IConversorMusicasDoAlbum, ConversorMusicasDoAlbum>();
    }
}