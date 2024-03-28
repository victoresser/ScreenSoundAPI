using ScreenSound.Dominio.Interfaces.Consultas;
using ScreenSound.Dominio.Interfaces.Conversores;
using ScreenSound.Dominio.Interfaces.Repositorios;
using ScreenSound.Dominio.Models.Bandas.Dto;

namespace ScreenSound.Dominio.Models.Bandas.Services;

public class ConsultaListagemDeBandas : IConsultaListagemDeBandas
{
    private IBandaRepositorio _bandaRepositorio;
    private IConversorAlbunsDaBanda _conversor;

    public ConsultaListagemDeBandas(IBandaRepositorio bandaRepositorio, IConversorAlbunsDaBanda conversor)
    {
        _bandaRepositorio = bandaRepositorio;
        _conversor = conversor;
    }

    public async Task<IEnumerable<ListagemDeBandas>> RetornaListagemDeBandas(string? nomeBanda,
        int skip,
        int take)
    {
        var consulta = await _bandaRepositorio.ConsultarAsync();

        var dto = MapearBandas(consulta)
            .OrderBy(x => x.Nome)
            .ToList()
            .Skip(skip)
            .Take(take);

        if (!string.IsNullOrEmpty(nomeBanda))
            return dto.Where(x => x.Nome.Contains(nomeBanda));

        return dto.Any() ? dto : new List<ListagemDeBandas>();
    }

    public async Task<ListagemDeBandas> RetornarBandaPorId(int id)
    {
        var consulta = await _bandaRepositorio.ObterPorIdAsync(id);

        if (consulta == null)
            return new ListagemDeBandas();

        var dto = new ListagemDeBandas
        {
            Id = consulta.Id,
            Nome = consulta.Nome,
            Descricao = consulta.Descricao,
            Albuns = _conversor.ConverterParaListagemDeAlbuns(consulta.AlbunsDaBanda),
            Imagem = $"data:image/png;base64,{Convert.ToBase64String(consulta.Imagem ?? Array.Empty<byte>())}"
        };

        return dto;
    }

    private IEnumerable<ListagemDeBandas> MapearBandas(IEnumerable<Banda> bandas)
    => bandas.Select(x => new ListagemDeBandas
    {
        Id = x.Id,
        Nome = x.Nome,
        Descricao = x.Descricao,
        Albuns = _conversor.ConverterParaListagemDeAlbuns(x.AlbunsDaBanda),
        Imagem = $"data:image/png;base64,{Convert.ToBase64String(x.Imagem ?? Array.Empty<byte>())}"

    }).ToList();
}