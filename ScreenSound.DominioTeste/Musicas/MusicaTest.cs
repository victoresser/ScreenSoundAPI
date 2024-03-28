using Bogus;
using ExpectedObjects;
using ScreenSound.Dominio.Models.Albuns;
using ScreenSound.Dominio.Models.Bandas;
using ScreenSound.Dominio.Models.Musicas;
using ScreenSound.DominioTeste._Builder;

namespace ScreenSound.DominioTeste.Musicas;

public class MusicaTest
{
    private readonly Faker faker;

    public MusicaTest()
    {
        faker = new Faker();
    }

    [Fact]
    public void DeveCriarMusica()
    {
        var musicaEsperada = MusicaBuilder.Novo().Build();

        var musica = new Musica(musicaEsperada.Nome, musicaEsperada.Duracao, musicaEsperada.AlbumId, musicaEsperada.BandaId);

        musicaEsperada.ToExpectedObject().ShouldMatch(musica);
    }

    [Fact]
    public void DeveAlterarNomeDaMusica()
    {
        var nomeEsperado = faker.Person.UserName;
        var musica = MusicaBuilder.Novo().Build();

        musica.AlterarNome(nomeEsperado);

        Assert.Equal(nomeEsperado, musica.Nome);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void NaoDeveAlterarNomeInvalido(string? nomeInvalido)
    {
        var musica = MusicaBuilder.Novo().Build();

        musica.AlterarNome(nomeInvalido);

        Assert.False(musica.Validar());
    }

    [Fact]
    public void DeveAlterarDuracaoDaMusica()
    {
        short duracaoEsperada = 150;
        var musica = MusicaBuilder.Novo().Build();

        musica.AlterarDuracao(duracaoEsperada);

        Assert.Equal(duracaoEsperada, musica.Duracao);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1000)]
    public void NoaDeveAlterarDuracaoInvalida(short duracaoInvalida)
    {
        var musica = MusicaBuilder.Novo().Build();

        musica.AlterarDuracao(duracaoInvalida);

        Assert.False(musica.Validar());
    }

    [Fact]
    public void DeveAlterarDisponibilidadeDaMusicaDaMusica()
    {
        var musica = MusicaBuilder.Novo().ComDisponivel(false).Build();
        var disponibilidadeEsperada = true;

        musica.AlterarDisponibilidade(disponibilidadeEsperada);

        Assert.Equal(disponibilidadeEsperada, musica.Disponivel);
    }

    [Fact]
    public void DeveAlterarArtistaDaMusica()
    {
        var musica = MusicaBuilder.Novo().Build();
        var artistaEsperado = BandaBuilder.Novo().Build();

        musica.AlterarArtista(artistaEsperado);

        Assert.Equal(artistaEsperado.Id, musica.BandaId);
    }

    [Theory]
    [InlineData(null)]
    public void NaoDeveAlterarArtistaInvalidoDaMusica(Banda bandaInvalido)
    {
        var musica = MusicaBuilder.Novo().Build();
        
        musica.AlterarArtista(bandaInvalido);

        Assert.False(musica.Validar());
    }

    [Fact]
    public void DeveAlterarAlbumDaMusica()
    {
        var musica = MusicaBuilder.Novo().Build();
        var album = AlbumBuilder.Novo().Build();

        musica.AlterarAlbum(album);

        Assert.Equal(album.Id, musica.AlbumId);
    }

    [Theory]
    [InlineData(null)]
    public void NaoDeveAlterarAlbumInvalidoDaMusica(Album albumInvalido)
    {
        var musica = MusicaBuilder.Novo().Build();

        musica.AlterarAlbum(albumInvalido);

        Assert.False(musica.Validar());
    }
}
