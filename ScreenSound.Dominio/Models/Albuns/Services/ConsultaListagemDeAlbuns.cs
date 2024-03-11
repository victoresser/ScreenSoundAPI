using ScreenSound.Dominio.Interfaces.Consultas;
using ScreenSound.Dominio.Interfaces.Conversores;
using ScreenSound.Dominio.Interfaces.Repositorios;
using ScreenSound.Dominio.Models.Albuns.Dto;

namespace ScreenSound.Dominio.Models.Albuns.Services;

public class ConsultaListagemDeAlbuns : IConsultaListagemDeAlbuns
{
    private readonly IAlbumRepositorio _albumRepositorio;
    private readonly IConversorMusicasDoAlbum _conversor;

    public ConsultaListagemDeAlbuns(IAlbumRepositorio albumRepositorio, IConversorMusicasDoAlbum conversor)
    {
        _albumRepositorio = albumRepositorio;
        _conversor = conversor;
    }

    public async Task<IEnumerable<ListagemDeAlbuns>> RetornaListagemDeAlbuns(string? nomeAlbum, int skip, int take)
    {
        var albuns = await _albumRepositorio.ConsultarAsync();

        var dto = MapearAlbum(albuns).Skip(skip).Take(take);

        return !string.IsNullOrEmpty(nomeAlbum) 
            ? dto.Where(x => x.Nome.Contains(nomeAlbum))
            : dto;
    }

    public async Task<ListagemDeAlbuns> RetornaListagemDeAlbunsPorId(int id)
    {
        var album = await _albumRepositorio.ObterPorId(id);

        if (album is null)
            return new ListagemDeAlbuns();

        var dto = new ListagemDeAlbuns
        {
            Nome = album.Nome,
            Banda = album.Banda.Nome,
            Musicas = _conversor.ConverterParaListagemDeMusicas(album?.MusicasDoAlbum)
        };

        return dto;
    }

    private IEnumerable<ListagemDeAlbuns> MapearAlbum(IEnumerable<Album> albuns)
    {
        var dto = albuns.Select(x => new ListagemDeAlbuns
        {
            Id = x.Id,
            Nome = x.Nome,
            Imagem = x.Imagem,
            BandaId = x.BandaId,
            Banda = x.Banda?.Nome ?? string.Empty,
            Musicas = _conversor.ConverterParaListagemDeMusicas(x.MusicasDoAlbum)
        }).ToList().OrderBy(x => x.Nome);

        return dto;
    }
}