using Bogus;
using FluentValidation.TestHelper;
using Moq;
using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Interfaces;
using ScreenSound.Dominio.Interfaces.Repositorios;
using ScreenSound.Dominio.Models.Albuns;
using ScreenSound.Dominio.Models.Albuns.Dto;
using ScreenSound.Dominio.Models.Bandas;
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
        var mockBase64Cleaner = new Mock<IBase64Cleaner>();
        _mockAlbumRepositorio = new Mock<IAlbumRepositorio>();
        _mockBandaRepositorio = new Mock<IBandaRepositorio>();
        _armazenadorDeAlbuns = new ArmazenadorDeAlbuns(_mockAlbumRepositorio.Object, _mockBandaRepositorio.Object,
                                                        mockBase64Cleaner.Object);
    }

    [Fact]
    public async void DeveAdicionarAlbum()
    {
        var album = AlbumBuilder.Novo().Build();
        var banda = BandaBuilder.Novo().Build();
        var setup = SetupCreateAlbumDto(nomeBanda: banda.Nome);

        _mockAlbumRepositorio
            .Setup(a => a.ObterPorNome(setup.Nome))
            .ReturnsAsync(album);
        
        _mockBandaRepositorio
            .Setup(b => b.ObterPorNome(banda.Nome))
            .ReturnsAsync(banda);
        
        var resultado = await _armazenadorDeAlbuns.Armazenar(setup);

        Assert.Equal(Resource.AlbumCriado, resultado);
    }

    [Fact]
    public async void NaoDeveAdicionarAlbumComMesmoNome()
    {
        var banda = BandaBuilder.Novo().Build();
        var albumExistente = AlbumBuilder.Novo().ComBanda(banda).Build();
        var album = AlbumBuilder.Novo().ComNome(albumExistente.Nome).Build();
        var setup = SetupCreateAlbumDto(album.Nome, banda.Nome);

        _mockAlbumRepositorio
            .Setup(x => x.ObterPorNome(album.Nome))
            .ReturnsAsync(albumExistente);
        _mockBandaRepositorio
            .Setup(x => x.ObterPorNome(banda.Nome))
            .ReturnsAsync(banda);

        var resultado =
            await Assert.ThrowsAsync<ArgumentException>(() => _armazenadorDeAlbuns.Armazenar(setup));
        Assert.Equal(Resource.AlbumExistente, resultado.Message);
    }

    [Fact]
    public async void NaoDeveAdicionarAlbumComNomeMaiorQue255Caracteres()
    {
        var nomeInvalido = _faker.Random.Words(300);
        var album = AlbumBuilder.Novo().Build();
        var setup = SetupCreateAlbumDto(nome: nomeInvalido);
        var banda = await BandaBuilder.Novo().ComNome(setup.NomeBanda).BuildAsync();

        _mockBandaRepositorio
            .Setup(r =>
                r.ObterPorNome(banda.Nome))
            .ReturnsAsync(banda);

        _mockAlbumRepositorio
            .Setup(a =>
                a.ObterPorNome(setup.Nome))
            .ReturnsAsync(album);

        var resultado =
            await Assert.ThrowsAsync<ArgumentException>(() => _armazenadorDeAlbuns.Armazenar(setup));
        Assert.Equal(Resource.NomeAlbumInvalido, resultado.Message);
    }

    [Fact]
    public async void NaoDeveAdicionarAlbumSemArtista()
    {
        var setup = SetupCreateAlbumDto(nomeBanda: null);

        _mockAlbumRepositorio
            .Setup(r => r.Adicionar(It.IsAny<Album>()))
            .Returns(Task.FromResult(setup));

        var resultado =
            await Assert.ThrowsAsync<ArgumentException>(() => _armazenadorDeAlbuns.Armazenar(setup));
        Assert.Equal(Resource.BandaInexistente, resultado.Message);
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

    private EditAlbumDto SetupEditAlbumDto(string? nome, string? nomeBanda, string? imagem,int id = 0)
        => new()
        {
            Id = id == 0 ? _faker.Random.Int(1, 10) : id,
            Nome = string.IsNullOrEmpty(nome) ? _faker.Name.FirstName() : nome,
            NomeBanda = string.IsNullOrEmpty(nomeBanda) ? _faker.Name.FirstName() : nomeBanda,
            Imagem = string.IsNullOrEmpty(imagem) ? _faker.Image.ToString() : imagem
        };

    private CreateAlbumDto SetupCreateAlbumDto(string? nome = null, string? nomeBanda = null)
        => new()
        {
            Nome = string.IsNullOrEmpty(nome) ? _faker.Name.FirstName() : nome,
            NomeBanda = string.IsNullOrEmpty(nomeBanda) ? _faker.Name.FirstName() : nomeBanda
        };
}