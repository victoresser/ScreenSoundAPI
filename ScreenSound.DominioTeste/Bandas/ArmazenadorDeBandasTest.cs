using Bogus;
using Moq;
using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Models.Bandas;
using ScreenSound.Dominio.Models.Bandas.Dto;
using ScreenSound.Dominio.Services.Repositorios;
using ScreenSound.DominioTeste._Builder;

namespace ScreenSound.DominioTeste.Bandas;

public class ArmazenadorDeBandasTest
{
    private readonly Faker _faker;
    private readonly ArmazenadorDeBandas _armazenadorBanda;
    private readonly Mock<IBandaRepositorio> _mockRepositorioBanda;

    public ArmazenadorDeBandasTest()
    {
        _faker = new Faker();

        _mockRepositorioBanda = new Mock<IBandaRepositorio>();
        _armazenadorBanda = new ArmazenadorDeBandas(_mockRepositorioBanda.Object);
    }

    [Fact]
    public async Task DeveAdicionarBanda()
    {
        Banda banda = SetupBandaDto();

        var resultado = await _armazenadorBanda.Armazenar(banda);

        Assert.Equal("Banda registrada!", resultado.ToString());
    }

    [Fact]
    public async Task NaoDeveAdicionarBandaComMesmoNome()
    {
        var nomeBanda = _faker.Person.FirstName;
        var banda = SetupBandaDto(nomeBanda);
        var bandaJaSalva = await BandaBuilder.Novo()
                                             .ComId(432)
                                             .ComNome(nomeBanda)
                                             .BuildAsync();

        _mockRepositorioBanda.Setup(r => r.ObterPorNome(nomeBanda))
                             .ReturnsAsync(bandaJaSalva);

        var excecao = await Assert.ThrowsAsync<ArgumentException>(() => _armazenadorBanda.Armazenar(banda));
        Assert.Equal(Resource.ArtistaExistente, excecao.Message);
    }


    [Fact]
    public async Task NaoDeveAdicionarBandaComNomeMaiorQue255Caracteres()
    {
        var nomeInvalido = _faker.Random.Words(3000);
        var banda = await BandaBuilder.Novo()
                                      .ComNome(nomeInvalido)
                                      .BuildAsync();

        _mockRepositorioBanda.Setup(r => r.Adicionar(It.IsAny<Banda>())).Returns(Task.FromResult(banda));

        var excecao = await Assert.ThrowsAsync<ArgumentException>(() => _armazenadorBanda.Armazenar(banda));
        Assert.Equal(Resource.NomeInvalido, excecao.Message);
    }

    [Fact]
    public async Task NaoDeveAdicionarBandaComDescricaoMaiorQue5000Caracteres()
    {
        var descricaoInvalida = _faker.Random.Words(10000);
        var banda = await BandaBuilder.Novo()
                                      .ComDescricao(descricaoInvalida)
                                      .BuildAsync();

        _mockRepositorioBanda.Setup(r => r.Adicionar(It.IsAny<Banda>())).Returns(Task.FromResult(banda));

        var excecao = await Assert.ThrowsAsync<ArgumentException>(() => _armazenadorBanda.Armazenar(banda));
        Assert.Equal(Resource.DescricaoBandaInvalida, excecao.Message);
    }

    [Fact]
    public async Task DeveAlterarDadosDaBanda()
    {
        var bandaDto = SetupBandaDto();
        var banda = await BandaBuilder.Novo().BuildAsync();
        
        _mockRepositorioBanda.Setup(r => r.ObterPorId(bandaDto.Id)).ReturnsAsync(banda);

        var resultado = await _armazenadorBanda.Editar(banda.Id, bandaDto.Nome, bandaDto.Descricao);
        Assert.Equal($"A banda {bandaDto.Nome} foi editada!", resultado);
        Assert.Equal(bandaDto.Nome, banda.Nome);
        Assert.Equal(bandaDto.Descricao, banda.Descricao);
    }

    private Banda SetupBandaDto(string? nomeBanda = null)
    {
        return new Banda(string.IsNullOrEmpty(nomeBanda) ? _faker.Random.Words(1) : nomeBanda)
        {
            Descricao = _faker.Lorem.Paragraph()
        };
    }
}