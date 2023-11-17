using Bogus;
using ExpectedObjects;
using ScreenSound.Dominio.Models.Albuns;
using ScreenSound.Dominio.Models.Bandas;
using ScreenSound.DominioTeste._Builder;

namespace ScreenSound.DominioTeste.Albuns;

public class AlbumTest
{

    private readonly Faker _faker;

    public AlbumTest()
    {
        _faker = new Faker();
    }

    [Fact]
    public void DeveCriarAlbum()
    {
        var albumEsperado = AlbumBuilder.Novo().Build();

        var album = new Album(albumEsperado.Nome, albumEsperado.BandaId);

        albumEsperado.ToExpectedObject().ShouldMatch(album);
    }

    [Fact]
    public void DeveAlterarNome()
    {
        var novoNomeEsperado = _faker.Name.FirstName();
        var album = AlbumBuilder.Novo().Build();

        album.AlterarNome(novoNomeEsperado);

        Assert.Equal(novoNomeEsperado, album.Nome);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void NaoDeveAlterarNomeInvalido(string nomeInvalido)
    {
        var album = AlbumBuilder.Novo().Build();

        album.AlterarNome(nomeInvalido);

        Assert.False(album.Validar());
    }

    [Fact]
    public void NaoDeveAlterarComNomeMaiorQue255Caracteres()
    {
        var nomeInvalido = _faker.Random.Words(300);
        var album = AlbumBuilder.Novo().Build();

        album.AlterarNome(nomeInvalido);

        Assert.False(album.Validar());
    }

    [Fact]
    public void DeveAlterarArtista()
    {
        var album = AlbumBuilder.Novo().Build();
        var banda = BandaBuilder.Novo().Build();

        album.AlterarArtista(banda);

        Assert.Equal(banda.Id, album.BandaId);
    }

    [Theory]
    [InlineData(null)]
    public void NaoDeveAlterarComArtistaInvalido(Banda banda)
    {
        var album = AlbumBuilder.Novo().Build();

        album.AlterarArtista(banda);

        Assert.False(album.Validar());
    }
}
