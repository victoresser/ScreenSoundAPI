using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Interfaces.Armazenadores;
using ScreenSound.Dominio.Interfaces.Repositorios;
using ScreenSound.Dominio.Models.Bandas;

namespace ScreenSound.Dominio.Models.Albuns.Dto;

public class ArmazenadorDeAlbuns : IArmazenadorAlbum
{
    private readonly IAlbumRepositorio _albumRepositorio;
    private readonly IBandaRepositorio _bandaRepositorio;

    public ArmazenadorDeAlbuns(IAlbumRepositorio albumRepositorio, IBandaRepositorio bandaRepositorio)
    {
        _albumRepositorio = albumRepositorio;
        _bandaRepositorio = bandaRepositorio;
    }

    public async Task<string> Armazenar(CreateAlbumDto dto)
    {
        var banda = await _bandaRepositorio.ObterPorNome(dto.NomeBanda);
        var album = await _albumRepositorio.ObterPorNome(dto.Nome);

        if (banda == null)
            throw new ArgumentException(Resource.BandaInexistente);

        if (album.Nome == dto.Nome)
            throw new ArgumentException(Resource.AlbumExistente);

        if (!string.IsNullOrEmpty(dto.Nome) && dto.Nome.Length > 255)
            throw new ArgumentException(Resource.NomeAlbumInvalido);

        var newAlbum = new Album(dto.Nome, banda.Id, dto.Imagem);
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
        album.AlterarImagem(dto.Imagem);
        await Console.Out.WriteLineAsync($"O Caminho da imagem deste Álbum foi alterado para '{album.Imagem}'.");

        return Resource.AlbumEditado;
    }
}