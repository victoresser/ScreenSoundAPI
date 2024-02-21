using Bogus;
using Moq;
using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Interfaces.Repositorios;
using ScreenSound.Dominio.Models.Albuns;
using ScreenSound.Dominio.Models.Bandas;
using ScreenSound.Dominio.Models.Musicas;
using ScreenSound.Dominio.Models.Musicas.Dto;
using ScreenSound.DominioTeste._Builder;

namespace ScreenSound.DominioTeste.Musicas;

public class ArmazenadorDeMusicasTest
{
    private readonly Faker _faker;
    private readonly ArmazenadorDeMusicas _armazenadorMusica;
    private readonly Mock<IMusicaRepositorio> _mockMusicaRepositorio;
    private readonly Mock<IBandaRepositorio> _mockBandaRepositorio;
    private readonly Mock<IAlbumRepositorio> _mockAlbumRepositorio;

    public ArmazenadorDeMusicasTest()
    {
        _faker = new Faker();
        _mockMusicaRepositorio = new Mock<IMusicaRepositorio>();
        Mock<IBandaRepositorio> mockBandaRepositorio = new();
        Mock<IAlbumRepositorio> mockAlbumRepositorio = new();
        _armazenadorMusica = new ArmazenadorDeMusicas(_mockMusicaRepositorio.Object, mockBandaRepositorio.Object, mockAlbumRepositorio.Object);
    }

    [Fact]
    public async void DeveAdicionarMusica()
    {
        var banda = BandaBuilder.Novo().Build();
        var album = AlbumBuilder.Novo().ComBanda(banda).Build();
        var musicaDto = SetupCreateMusicaDto(nomeBanda: banda.Nome, nomeAlbum: album.Nome);
        
        var resultado = await _armazenadorMusica.Armazenar(musicaDto);

        Assert.Equal(Resource.MusicaCriada, resultado);
    }

    [Fact]
    public async void NaoDeveAdicionarMusicaComMesmoNome()
    {
        var musicaExistente = MusicaBuilder.Novo().Build();

        var musicaDto = SetupCreateMusicaDto(nome: musicaExistente.Nome);
        _mockMusicaRepositorio.Setup(x => x.ObterPorNome(musicaDto.Nome)).ReturnsAsync(musicaExistente);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(() => _armazenadorMusica.Armazenar(musicaDto));
        Assert.Equal(Resource.MusicaExistente, resultado.Message);
    }

    [Fact]
    public async void NaoDeveAdicionarMusicaSemBanda()
    {
        var musicaDto = SetupCreateMusicaDto();
        var musica = MusicaBuilder.Novo().Build();
        var album = AlbumBuilder.Novo().Build();

        _mockMusicaRepositorio.Setup(x => x.Adicionar(It.IsAny<Musica>())).Returns(Task.FromResult(musica));

        var resultado = await Assert.ThrowsAsync<ArgumentException>(() => _armazenadorMusica.Armazenar(musicaDto));
        Assert.Equal(Resource.BandaInexistente, resultado.Message);
    }

    [Fact]
    public async void NaoDeveAdicionarMusicaSemAlbum()
    {
        var musicaDto = SetupCreateMusicaDto();
        var musica = MusicaBuilder.Novo().Build();
        musica.Banda = BandaBuilder.Novo().Build();

        _mockMusicaRepositorio.Setup(x => x.Adicionar(It.IsAny<Musica>())).Returns(Task.FromResult(musica));
        var resultado = await Assert.ThrowsAsync<ArgumentException>(() => _armazenadorMusica.Armazenar(musicaDto));
        
        Assert.Equal(Resource.AlbumInexistente, resultado.Message);
    }

    private CreateMusicaDto SetupCreateMusicaDto(short? duracao = null, string? nome = null, string? nomeBanda = null, string? nomeAlbum = null)
    {
        return new CreateMusicaDto
        {
            Nome = string.IsNullOrEmpty(nome) ? _faker.Random.Words(1) : nome,
            NomeBanda = string.IsNullOrEmpty(nomeBanda) ? _faker.Name.FirstName() : nomeBanda,
            NomeAlbum = string.IsNullOrEmpty(nomeAlbum) ? _faker.Name.FirstName() : nomeAlbum,
            Duracao = duracao ?? _faker.Random.Short(10, 256),
            Imagem = _faker.Image.ToString()
        };
    }
}
