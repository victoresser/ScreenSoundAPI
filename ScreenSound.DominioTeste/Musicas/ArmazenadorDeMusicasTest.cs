using Bogus;
using Moq;
using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Models.Albuns;
using ScreenSound.Dominio.Models.Bandas;
using ScreenSound.Dominio.Models.Musicas;
using ScreenSound.Dominio.Models.Musicas.Dto;
using ScreenSound.Dominio.Services.Repositorios;
using ScreenSound.DominioTeste._Builder;

namespace ScreenSound.DominioTeste.Musicas;

public class ArmazenadorDeMusicasTest
{
    private readonly Faker faker;
    private readonly ArmazenadorDeMusicas _armazenadorMusica;
    private readonly Mock<IMusicaRepositorio> _mockMusicaRepositorio;
    private readonly Mock<IBandaRepositorio> _mockBandaRepositorio;
    private readonly Mock<IAlbumRepositorio> _mockAlbumRepositorio;

    public ArmazenadorDeMusicasTest()
    {
        faker = new Faker();
        _mockMusicaRepositorio = new Mock<IMusicaRepositorio>();
        _mockBandaRepositorio = new Mock<IBandaRepositorio>();
        _mockAlbumRepositorio = new Mock<IAlbumRepositorio>();
        _armazenadorMusica = new ArmazenadorDeMusicas(_mockMusicaRepositorio.Object, _mockBandaRepositorio.Object, _mockAlbumRepositorio.Object);
    }

    [Fact]
    public async void DeveAdicionarMusica()
    {
        var musica = MusicaBuilder.Novo().Build();
        var banda = BandaBuilder.Novo().Build();
        var album = AlbumBuilder.Novo().Build();

        var resultado = await _armazenadorMusica.Armazenar(musica.Nome, musica.Duracao, banda, album, musica.Disponivel);

        Assert.Equal(Resource.MusicaCriada, resultado);
    }

    [Fact]
    public async void NaoDeveAdicionarMusicaComMesmoNome()
    {
        var musicaExistente = MusicaBuilder.Novo().Build();
        var musica = MusicaBuilder.Novo().ComNome(musicaExistente.Nome).Build();
        var banda = BandaBuilder.Novo().Build();
        var album = AlbumBuilder.Novo().Build();

        _mockMusicaRepositorio.Setup(x => x.ObterPorNome(musica.Nome)).ReturnsAsync(musicaExistente);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(() => _armazenadorMusica.Armazenar(musica.Nome,
                                                                                                       musica.Duracao,
                                                                                                       banda,
                                                                                                       album,
                                                                                                       musica.Disponivel));
        Assert.Equal(Resource.MusicaExistente, resultado.Message);
    }

    [Fact]
    public async void NaoDeveAdicionarMusicaSemBanda()
    {
        var musica = MusicaBuilder.Novo().Build();
        var album = AlbumBuilder.Novo().Build();

        _mockMusicaRepositorio.Setup(x => x.Adicionar(It.IsAny<Musica>())).Returns(Task.FromResult(musica));

        var resultado = await Assert.ThrowsAsync<ArgumentException>(() => _armazenadorMusica.Armazenar(musica.Nome,
                                                                                                       musica.Duracao,
                                                                                                       null,
                                                                                                       album,
                                                                                                       musica.Disponivel));
        Assert.Equal(Resource.ArtistaInexistente, resultado.Message);
    }

    [Fact]
    public async void NaoDeveAdicionarMusicaSemAlbum()
    {
        var musica = MusicaBuilder.Novo().Build();
        var banda = BandaBuilder.Novo().Build();

        _mockMusicaRepositorio.Setup(x => x.Adicionar(It.IsAny<Musica>())).Returns(Task.FromResult(musica));
        var resultado = await Assert.ThrowsAsync<ArgumentException>(() => _armazenadorMusica.Armazenar(musica.Nome,
                                                                                                       musica.Duracao,
                                                                                                       banda,
                                                                                                       null,
                                                                                                       musica.Disponivel));
        
        Assert.Equal(Resource.AlbumInexistente, resultado.Message);
    }
}
