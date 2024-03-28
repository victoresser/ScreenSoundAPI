using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Interfaces;
using ScreenSound.Dominio.Interfaces.Armazenadores;
using ScreenSound.Dominio.Interfaces.Repositorios;
using ScreenSound.Dominio.Models.Bandas;

namespace ScreenSound.Dominio.Models.Albuns.Dto;

public class ArmazenadorDeAlbuns : IArmazenadorAlbum
{
    private readonly IAlbumRepositorio _albumRepositorio;
    private readonly IBandaRepositorio _bandaRepositorio;
    private readonly IBase64Cleaner _base64Cleaner;

    public ArmazenadorDeAlbuns(IAlbumRepositorio albumRepositorio, IBandaRepositorio bandaRepositorio,
        IBase64Cleaner base64Cleaner)
    {
        _albumRepositorio = albumRepositorio;
        _bandaRepositorio = bandaRepositorio;
        _base64Cleaner = base64Cleaner;
    }

    public async Task<string> Armazenar(CreateAlbumDto dto)
    {
        byte[]? imagemBase64 = null;
        
        if (string.IsNullOrEmpty(dto.Nome))
            throw new ArgumentException("Erro | O nome do álbum não pode ser nulo ou vazío");
        
        var album = await _albumRepositorio.ObterPorNome(dto.Nome);
        
        if (string.IsNullOrEmpty(dto.NomeBanda))
            throw new ArgumentException("Erro | O nome da banda não pode ser nulo ou vazío");
        
        var banda = await _bandaRepositorio.ObterPorNome(dto.NomeBanda);

        if (!string.IsNullOrEmpty(dto.Nome) && dto.Nome.Length > 255)
            throw new ArgumentException(Resource.NomeAlbumInvalido);

        if (banda == null)
            throw new ArgumentException(Resource.BandaInexistente);

        if (album != null && album.Nome == dto.Nome)
            throw new ArgumentException(Resource.AlbumExistente);

        if (!string.IsNullOrEmpty(dto.Imagem))
            imagemBase64 = _base64Cleaner.ConverterStringBase64ParaBytes(dto.Imagem);

        var newAlbum = new Album(dto.Nome, banda.Id, imagemBase64 ?? Array.Empty<byte>());
        await _albumRepositorio.Adicionar(newAlbum);

        return Resource.AlbumCriado;
    }

    public async Task<string> Editar(EditAlbumDto dto)
    {
        var album = await _albumRepositorio.ObterPorId(dto.Id);
        var banda = await _bandaRepositorio.ObterPorNome(dto.NomeBanda ?? string.Empty);

        if (album == null)
            return Resource.AlbumInexistente;

        if (!string.IsNullOrWhiteSpace(dto.Nome))
        {
            album.AlterarNome(dto.Nome);
            await Console.Out.WriteLineAsync("O nome do álbum foi alterado!");
        }

        if (!string.IsNullOrWhiteSpace(dto.NomeBanda) && banda != null)
        {
            album.AlterarArtista(banda);
            await Console.Out.WriteLineAsync("O Artista do álbum foi alterado!");
        }

        if (string.IsNullOrWhiteSpace(dto.Imagem)) return Resource.AlbumEditado;
        
        album.AlterarImagem(_base64Cleaner.ConverterStringBase64ParaBytes(dto.Imagem));
        await Console.Out.WriteLineAsync($"O Caminho da imagem deste Álbum foi alterado para '{album.Imagem}'.");

        return Resource.AlbumEditado;
    }
}