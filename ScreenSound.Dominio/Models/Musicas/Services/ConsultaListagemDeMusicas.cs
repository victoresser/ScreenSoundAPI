using ScreenSound.Dominio.Interfaces;
using ScreenSound.Dominio.Interfaces.Consultas;
using ScreenSound.Dominio.Interfaces.Repositorios;
using ScreenSound.Dominio.Models.Musicas.Dto;

namespace ScreenSound.Dominio.Models.Musicas.Services;

public class ConsultaListagemDeMusicas : IConsultaListagemDeMusicas
{
    private readonly IMusicaRepositorio _musicaRepositorio;
    private readonly IBase64Cleaner _base64Cleaner;

    public ConsultaListagemDeMusicas(IMusicaRepositorio musicaRepositorio, IBase64Cleaner base64Cleaner)
    {
        _musicaRepositorio = musicaRepositorio;
        _base64Cleaner = base64Cleaner;
    }

    public async Task<IEnumerable<ListagemDeMusicas>> RetornaListagemDeMusicas(string? nomeMusica = null,
        int skip = 0,
        int take = 10)
    {
        var consulta = await _musicaRepositorio.ConsultarAsync();

        var dto = MapearMusicas(consulta, skip, take).ToList();

        if (!string.IsNullOrEmpty(nomeMusica))
            return dto.Where(x => x.Nome.Contains(nomeMusica));

        return dto.Any()
            ? dto
            : new List<ListagemDeMusicas>();
    }

    public async Task<ListagemDeMusicas> RetornaMusicaPorId(int id)
    {
        var consulta = await _musicaRepositorio.ObterPorId(id);

        if (consulta == null)
            return new ListagemDeMusicas();

        var dto = new ListagemDeMusicas
        {
            Nome = consulta.Nome,
            Duracao = consulta.Duracao,
            Disponivel = consulta.Disponivel,
            Banda = consulta.Banda?.Nome,
            Album = consulta.Album?.Nome,
            Imagem = _base64Cleaner.ConverterBytesParaStringBase64(consulta.Imagem)
        };

        return dto;
    }

    private IEnumerable<ListagemDeMusicas> MapearMusicas(IEnumerable<Musica> musicas, int skip, int take)
        => musicas.Select(x => new ListagemDeMusicas
        {
            Id = x.Id,
            Nome = x.Nome,
            Imagem = _base64Cleaner.ConverterBytesParaStringBase64(x.Imagem),
            Duracao = x.Duracao,
            Banda = x.Banda?.Nome,
            Album = x.Album?.Nome,
            Disponivel = x.Disponivel
        }).Skip(skip).Take(take).OrderBy(x => x.Nome).ToList();
}