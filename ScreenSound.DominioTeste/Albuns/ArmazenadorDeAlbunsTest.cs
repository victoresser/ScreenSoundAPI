using Bogus;
using FluentValidation.TestHelper;
using Moq;
using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Interfaces.Repositorios;
using ScreenSound.Dominio.Models.Albuns;
using ScreenSound.Dominio.Models.Albuns.Dto;
using ScreenSound.DominioTeste._Builder;

namespace ScreenSound.DominioTeste.Albuns;

public class ArmazenadorDeAlbunsTest
{
    private readonly Faker _faker;
    private readonly ArmazenadorDeAlbuns _armazenadorDeAlbuns;
    private readonly Mock<IAlbumRepositorio> _mockAlbumRepositorio;
    private readonly Mock<IBandaRepositorio> _mockBandaRepositorio;

    public ArmazenadorDeAlbunsTest()
    {
        _faker = new Faker();
        _mockAlbumRepositorio = new Mock<IAlbumRepositorio>();
        _mockBandaRepositorio = new Mock<IBandaRepositorio>();
        _armazenadorDeAlbuns = new ArmazenadorDeAlbuns(_mockAlbumRepositorio.Object, _mockBandaRepositorio.Object);
    }

    [Fact]
    public async void DeveAdicionarAlbum()
    {
        var album = AlbumBuilder.Novo().Build();
        var banda = BandaBuilder.Novo().Build();

        var resultado = await _armazenadorDeAlbuns.Armazenar(album.Nome, banda);

        Assert.Equal(Resource.AlbumCriado, resultado);
    }

    [Fact]
    public async void NaoDeveAdicionarAlbumComMesmoNome()
    {
        var banda = BandaBuilder.Novo().Build();
        var albumExistente = AlbumBuilder.Novo().ComBanda(banda).Build();
        var album = AlbumBuilder.Novo().ComNome(albumExistente.Nome).Build();

        _mockAlbumRepositorio
            .Setup(x => x.ObterPorNome(album.Nome))
            .ReturnsAsync(albumExistente);

        var resultado =
            await Assert.ThrowsAsync<ArgumentException>(() => _armazenadorDeAlbuns.Armazenar(album.Nome, banda));
        Assert.Equal(Resource.AlbumExistente, resultado.Message);
    }

    [Fact]
    public async void NaoDeveAdicionarAlbumComNomeMaiorQue255Caracteres()
    {
        var nomeInvalido = _faker.Random.Words(300);
        var banda = BandaBuilder.Novo().Build();
        var album = AlbumBuilder.Novo().ComNome(nomeInvalido).Build();

        _mockAlbumRepositorio
            .Setup(r => r
            .Adicionar(It.IsAny<Album>()))
            .Returns(Task.FromResult(album));

        var resultado =
            await Assert.ThrowsAsync<ArgumentException>(() => _armazenadorDeAlbuns.Armazenar(album.Nome, banda));
        Assert.Equal(Resource.NomeAlbumInvalido, resultado.Message);
    }

    [Fact]
    public async void NaoDeveAdicionarAlbumSemArtista()
    {
        var album = AlbumBuilder.Novo().Build();

        _mockAlbumRepositorio
            .Setup(r => r.Adicionar(It.IsAny<Album>()))
            .Returns(Task.FromResult(album));

        var resultado =
            await Assert.ThrowsAsync<ArgumentException>(() => _armazenadorDeAlbuns.Armazenar(album.Nome, null));
        Assert.Equal(Resource.BandaInvalida, resultado.Message);
    }

    // [Fact]
    // public async void DeveAlterarDadosDeAlbum()
    // {
    //     var banda = BandaBuilder.Novo().Build();
    //     var albumDto = AlbumBuilder.Novo().ComBanda(banda).Build();
    //     var album = AlbumBuilder.Novo().Build();
    //     var dto = SetupEditAlbumDto(albumDto.Id, album.Nome, banda.Nome, album.Imagem);
    //
    //     _mockAlbumRepositorio.Setup(r => r.ObterPorId(albumDto.Id)).ReturnsAsync(album);
    //     var resultado = await _armazenadorDeAlbuns.Editar(dto);
    //
    //     Assert.Equal(Resource.AlbumEditado, resultado);
    //     Assert.Equal(albumDto.Nome, album.Nome);
    //     Assert.Equal(albumDto.Banda, album.Banda);
    // }


    private EditAlbumDto SetupEditAlbumDto(int id = 0, string? nome = null, string? nomeBanda = null,
        string? imagem = null)
    {
        return new EditAlbumDto
        {
            Id = id == 0 ? _faker.Random.Int(1, 10) : id,
            Nome = string.IsNullOrEmpty(nome) ? _faker.Name.FirstName() : nome,
            NomeBanda = string.IsNullOrEmpty(nomeBanda) ? _faker.Name.FirstName() : nomeBanda,
            Imagem = string.IsNullOrEmpty(imagem) ? _faker.Image.ToString() : imagem
        };
    }
}