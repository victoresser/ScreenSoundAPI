using Bogus;
using ExpectedObjects;
using ScreenSound.Dominio.Models.Bandas;
using ScreenSound.DominioTeste._Builder;

namespace ScreenSound.DominioTeste.Bandas;

public class BandaTest
{
    private readonly Faker _faker;


    public BandaTest()
    {
        _faker = new Faker();
    }

    [Fact]
    public void DeveCriarBanda()
    {
        var bandaEsperada = new
        {
            Nome = _faker.Name.FirstName(),
            Descricao = _faker.Lorem.Paragraph(1)
        };

        var banda = new Banda(bandaEsperada.Nome, bandaEsperada.Descricao);

        bandaEsperada.ToExpectedObject().ShouldMatch(banda);
    }

    [Fact]
    public void DeveAlterarNome()
    {
        var novoNomeEsperado = _faker.Name.FirstName();
        var banda = BandaBuilder.Novo().Build();

        banda.AlterarNome(novoNomeEsperado);

        Assert.Equal(novoNomeEsperado, banda.Nome);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void NaoDeveAlterarComNomeInvalido(string nomeInvalido)
    {
        var banda = BandaBuilder.Novo().Build();

        banda.AlterarNome(nomeInvalido);

        Assert.False(banda.Validar());
    }

    [Fact]
    public void NaoDeveAlterarComNomeMaiorQue255Caracteres()
    {
        var nomeInvalido = _faker.Random.Words(300);
        var banda = BandaBuilder.Novo().Build();

        banda.AlterarNome(nomeInvalido);

        Assert.False(banda.Validar());
    }


    [Fact]
    public void DeveAlterarDescricao()
    {
        var novaDescricaoEsperada = _faker.Random.Words(300);
        var banda = BandaBuilder.Novo().Build();

        banda.AlterarDescricao(novaDescricaoEsperada);

        Assert.Equal(novaDescricaoEsperada, banda.Descricao);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void NaoDeveAlterarComDescricaoInvalida(string descricaoInvalida)
    {
        var banda = BandaBuilder.Novo().Build();

        banda.AlterarDescricao(descricaoInvalida);

        Assert.False(banda.Validar());
    }
    
    [Fact]
    public void NaoDeveAlterarComDescricaoMaiorQue5000MilCaracteres()
    {
        var descricaoInvalida = _faker.Random.Words(10000);
        var banda = BandaBuilder.Novo().Build();

        banda.AlterarDescricao(descricaoInvalida);

        Assert.False(banda.Validar());
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void NaoDeveCriarComNomeInvalido(string nomeInvalido)
    {
        var banda = BandaBuilder.Novo().ComNome(nomeInvalido).Build();

        var isValid = banda.Validar();

        Assert.False(isValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void NaoDeveCriarDescricaoInvalida(string descricaoInvalida)
    {
        var banda = BandaBuilder.Novo().ComDescricao(descricaoInvalida).Build();

        var isValid = banda?.Validar();

        Assert.False(isValid);
    }
}
