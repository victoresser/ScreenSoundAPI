using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Models.Bandas;
using ScreenSound.Dominio.Services.Armazenadores;
using ScreenSound.Dominio.Services.Repositorios;

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

    public async Task<string> Armazenar(string nome, Banda banda)
    {
        var albumSalvo = await _albumRepositorio.ObterPorNome(nome);

        if (albumSalvo != null && albumSalvo.Nome == nome)
        {
            throw new ArgumentException(Resource.AlbumExistente);
        }

        if (nome != null && nome.Length > 255)
        {
            throw new ArgumentException(Resource.NomeAlbumInvalido);
        }

        if (banda == null)
        {
            throw new ArgumentException(Resource.BandaInvalida);
        }

        var newAlbum = new Album(nome, banda.Id);
        await _albumRepositorio.Adicionar(newAlbum);
        return Resource.AlbumCriado;
    }

    public async Task<string> Editar(int id, string? nome, string? nomeBanda, string? imagem)
    {

        var album = await _albumRepositorio.ObterPorId(id);
        var artista = await _bandaRepositorio.ObterPorNome(nomeBanda);

        if (album == null)
        {
            throw new ArgumentNullException(Resource.AlbumInexistente);
        }

        if (nome != null)
        {
            album.AlterarNome(nome);
            await Console.Out.WriteLineAsync("O nome do álbum foi alterado!");
        }

        if (artista != null)
        {
            album.AlterarArtista(artista);
            await Console.Out.WriteLineAsync("O Artista do álbum foi alterado!");
        }

        if (!string.IsNullOrEmpty(imagem))
        {
            album.AlterarImagem(imagem);
            await Console.Out.WriteLineAsync($"O Caminho da imagem deste Álbum foi alterado para '{album.Imagem}'.");
        }

        return Resource.AlbumEditado;
    }
}
