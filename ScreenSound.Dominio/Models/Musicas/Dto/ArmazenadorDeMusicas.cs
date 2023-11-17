using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Models.Albuns;
using ScreenSound.Dominio.Models.Bandas;
using ScreenSound.Dominio.Services.Armazenadores;
using ScreenSound.Dominio.Services.Repositorios;

namespace ScreenSound.Dominio.Models.Musicas.Dto;

public class ArmazenadorDeMusicas : IArmazenadorMusica
{
    private readonly IMusicaRepositorio _musicaRepositorio;
    private readonly IBandaRepositorio _bandaRepositorio;
    private readonly IAlbumRepositorio _albumRepositorio;

    public ArmazenadorDeMusicas(IMusicaRepositorio musicaRepositorio, IBandaRepositorio bandaRepositorio, IAlbumRepositorio albumRepositorio)
    {
        _musicaRepositorio = musicaRepositorio;
        _bandaRepositorio = bandaRepositorio;
        _albumRepositorio = albumRepositorio;
    }

    public async Task<string> Armazenar(string nome, short duracao, Banda banda, Album album, bool? disponivel = true, string? imagem = "")
    {
        Musica? musicaJaSalva = await _musicaRepositorio.ObterPorNome(nome);

        if (musicaJaSalva == null || musicaJaSalva.Nome != nome)
        {
            if (banda == null)
            {
                throw new ArgumentException(Resource.ArtistaInexistente);
            }

            if (album == null)
            {
                throw new ArgumentException(Resource.AlbumInexistente);
            }

            if (nome != null && nome.Length > 255)
            {
                throw new ArgumentException(Resource.NomeAlbumInvalido);
            }

            if (nome != null && banda != null && album != null)
            {
                var musica = new Musica(nome, duracao, album.Id, banda.Id, disponivel, imagem);
                await _musicaRepositorio.Adicionar(musica);
                return Resource.MusicaCriada;
            }

            return "Erro | Aparentemente, algo deu errado com o registro desta música, verifique as informações inseridas";
        }

        throw new ArgumentException(Resource.MusicaExistente);
    }

    public async Task<string> Editar(int id, string? nome, string? nomeArtista, string? nomeAlbum, short? duracaoMusica, bool disponibilidade, string? imagem)
    {
        var musica = await _musicaRepositorio.ObterPorId(id);
        var banda = await _bandaRepositorio.ObterPorNome(nomeArtista);
        var album = await _albumRepositorio.ObterPorNome(nomeAlbum);

        if (musica == null)
        {
            return "Música não encontrada!";
        }

        if (!string.IsNullOrEmpty(nome))
        {
            musica.AlterarNome(nome);
            await Console.Out.WriteLineAsync($"O nome da música foi alterado para {musica.Nome}");
        }

        if (!string.IsNullOrEmpty(nomeArtista) && banda != null)
        {
            musica.AlterarArtista(banda);
            await Console.Out.WriteLineAsync($"A autoria desta música foi alterada para {musica.Banda.Nome}");
        }

        if (!string.IsNullOrEmpty(nomeAlbum) && album != null)
        {
            musica.AlterarAlbum(album);
            await Console.Out.WriteLineAsync($"Esta música agora faz parte do álbum: {musica.Album.Nome}");
        }

        if (duracaoMusica != null && duracaoMusica > 0)
        {
            musica.AlterarDuracao((short)duracaoMusica);
            await Console.Out.WriteLineAsync($"A duração desta música foi alterada para {musica.Duracao}");
        }

        if (disponibilidade)
        {
            musica.AlterarDisponibilidade(disponibilidade);
            await Console.Out.WriteLineAsync($"A disponibilidade desta música foi alterarada para {musica.Disponivel}");
        }

        if (imagem != null)
        {
            musica.AlterarImagem(imagem);
            await Console.Out.WriteLineAsync($"O caminho desta música foi alterado para {musica.Imagem}");
        }

        return "Música editada com sucesso!";

    }
}
