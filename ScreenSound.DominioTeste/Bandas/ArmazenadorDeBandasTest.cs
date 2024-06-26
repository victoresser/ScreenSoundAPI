﻿using Bogus;
using ExpectedObjects;
using Moq;
using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Interfaces;
using ScreenSound.Dominio.Interfaces.Repositorios;
using ScreenSound.Dominio.Models.Bandas;
using ScreenSound.Dominio.Models.Bandas.Dto;
using ScreenSound.Dominio.Models.Bandas.Services;
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
        Mock<IBase64Cleaner> mockBase64Cleaner = new();
        _armazenadorBanda = new ArmazenadorDeBandas(_mockRepositorioBanda.Object, mockBase64Cleaner.Object);
    }

    [Fact]
    public async Task DeveAdicionarBanda()
    {
        var banda = SetupCreateBandaDto();

        var resultado = await _armazenadorBanda.Armazenar(banda);

        Assert.Equal(Resource.BandaCriada, resultado);
    }

    [Fact]
    public async Task NaoDeveAdicionarBandaComMesmoNome()
    {
        var nomeBanda = _faker.Person.FirstName;
        var banda = SetupCreateBandaDto(nomeBanda);
        var bandaJaSalva = await BandaBuilder.Novo()
            .ComId(432)
            .ComNome(nomeBanda)
            .BuildAsync();

        _mockRepositorioBanda.Setup(r => r.ObterPorNome(nomeBanda))
            .ReturnsAsync(bandaJaSalva);

        var excecao = await Assert.ThrowsAsync<ArgumentException>(() => _armazenadorBanda.Armazenar(banda));
        Assert.Equal(Resource.BandaJaExiste, excecao.Message);
    }


    [Fact]
    public async Task NaoDeveAdicionarBandaComNomeMaiorQue255Caracteres()
    {
        var nomeInvalido = _faker.Random.Words(3000);
        var bandaDto = SetupCreateBandaDto(nomeBanda: nomeInvalido);
        var banda = await BandaBuilder.Novo()
            .ComNome(nomeInvalido)
            .BuildAsync();

        _mockRepositorioBanda.Setup(r => r.Adicionar(It.IsAny<Banda>())).Returns(Task.FromResult(banda));

        var excecao = await Assert.ThrowsAsync<ArgumentException>(() => _armazenadorBanda.Armazenar(bandaDto));
        Assert.Equal(Resource.NomeBandaInvalido, excecao.Message);
    }

    [Fact]
    public async Task NaoDeveAdicionarBandaComDescricaoMaiorQue5000Caracteres()
    {
        var descricaoInvalida = _faker.Random.Words(10000);
        var bandaDto = SetupCreateBandaDto(descricao: descricaoInvalida);
        var banda = await BandaBuilder.Novo()
            .ComDescricao(descricaoInvalida)
            .BuildAsync();

        _mockRepositorioBanda.Setup(r => r.Adicionar(It.IsAny<Banda>())).Returns(Task.FromResult(banda));

        var excecao = await Assert.ThrowsAsync<ArgumentException>(() => _armazenadorBanda.Armazenar(bandaDto));
        Assert.Equal(Resource.DescricaoBandaInvalida, excecao.Message);
    }

    [Fact]
    public async Task DeveAlterarDadosDaBanda()
    {
        var bandaDto = SetupBanda();
        var banda = await BandaBuilder.Novo().BuildAsync();

        _mockRepositorioBanda.Setup(r => r.ObterPorIdAsync(bandaDto.Id)).ReturnsAsync(banda);

        var resultado = await _armazenadorBanda.Editar(banda.Id, bandaDto.Nome, bandaDto.Descricao);
        Assert.Equal($"A banda {bandaDto.Nome} foi editada!", resultado);
        Assert.Equal(bandaDto.Nome, banda.Nome);
        Assert.Equal(bandaDto.Descricao, banda.Descricao);
    }

    private CreateBandaDto SetupCreateBandaDto(string? nomeBanda = null, string? descricao = null)
    {
        return new CreateBandaDto
        {
            Nome = string.IsNullOrEmpty(nomeBanda) ? _faker.Random.Words(1) : nomeBanda,
            Descricao = string.IsNullOrEmpty(descricao) ? _faker.Lorem.Paragraph() : descricao,
            Imagem = _faker.Image.ToString(),
        };
    }

    private Banda SetupBanda(string? nomeBanda = null)
    {
        return new Banda(nome: string.IsNullOrEmpty(nomeBanda) ? _faker.Random.Words(1) : nomeBanda)
        {
            Descricao = _faker.Lorem.Paragraph()
        };
    }
}