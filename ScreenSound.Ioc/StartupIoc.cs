using Microsoft.Extensions.DependencyInjection;
using ScreenSound.Dados;
using ScreenSound.Dados.Repositorios;
using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Interfaces;
using ScreenSound.Dominio.Interfaces.Armazenadores;
using ScreenSound.Dominio.Interfaces.Consultas;
using ScreenSound.Dominio.Interfaces.Conversores;
using ScreenSound.Dominio.Interfaces.Repositorios;
using ScreenSound.Dominio.Models.Albuns.Dto;
using ScreenSound.Dominio.Models.Albuns.Services;
using ScreenSound.Dominio.Models.Bandas.Dto;
using ScreenSound.Dominio.Models.Bandas.Services;
using ScreenSound.Dominio.Models.Musicas.Dto;
using ScreenSound.Dominio.Models.Musicas.Services;

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
        services.AddScoped<IConsultaListagemDeMusicas, ConsultaListagemDeMusicas>();
        services.AddScoped<IBase64Cleaner, Base64Cleaner>();
        services.AddScoped<ConsultaListagemDeAlbuns>();
        services.AddScoped<ConsultaListagemDeBandas>();
    }
}