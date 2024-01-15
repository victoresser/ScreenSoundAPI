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

    public async Task<string> Armazenar(CreateMusicaDto dto)
    {
        var musicaJaSalva = await _musicaRepositorio.ObterPorNome(dto.NomeMusica);
        var album = await _albumRepositorio.ObterPorNome(dto.NomeAlbum);
        var banda = await _bandaRepositorio.ObterPorNome(dto.NomeBanda);

        if (musicaJaSalva != null && musicaJaSalva.Nome == dto.NomeMusica)
            throw new ArgumentException(Resource.MusicaExistente);
        
        if (banda == null)
            throw new ArgumentException(Resource.BandaInexistente);

        if (album == null)
            throw new ArgumentException(Resource.AlbumInexistente);

        if (dto.NomeMusica != null && dto.NomeMusica.Length > 255)
            throw new ArgumentException(Resource.NomeAlbumInvalido);

        if (string.IsNullOrWhiteSpace(dto.NomeMusica) || banda == null || album == null)
            throw new ArgumentException(
            "Erro | Aparentemente, algo deu errado com o registro desta música, verifique as informações inseridas"
                );
        
        var musica = new Musica(dto.NomeMusica, dto.Duracao, album.Id, banda.Id, dto.Disponibilidade, dto.Imagem);
        
        await _musicaRepositorio.Adicionar(musica);
        return Resource.MusicaCriada;

    }

    public async Task<string> Editar(EditMusicaDto dto)
    {
        var musica = await _musicaRepositorio.ObterPorId(dto.Id);
        var banda = await _bandaRepositorio.ObterPorNome(dto.nomeBanda ?? string.Empty);
        var album = await _albumRepositorio.ObterPorNome(dto.nomeAlbum ?? string.Empty);

        if (musica == null)
            throw new ArgumentException(Resource.MusicaInexistente);
        
        if (!string.IsNullOrEmpty(dto.NomeMusica))
        {
            musica.AlterarNome(dto.NomeMusica);
            await Console.Out.WriteLineAsync($"O nome da música foi alterado para {musica.Nome}");
        }

        if (!string.IsNullOrEmpty(dto.nomeBanda) && banda != null)
        {
            musica.AlterarArtista(banda);
            await Console.Out.WriteLineAsync($"A autoria desta música foi alterada para {musica.Banda.Nome}");
        }

        if (!string.IsNullOrEmpty(dto.nomeAlbum) && album != null)
        {
            musica.AlterarAlbum(album);
            await Console.Out.WriteLineAsync($"Esta música agora faz parte do álbum: {musica.Album.Nome}");
        }

        if (dto.Duracao != null && dto.Duracao > 0)
        {
            musica.AlterarDuracao((short)dto.Duracao);
            await Console.Out.WriteLineAsync($"A duração desta música foi alterada para {musica.Duracao}");
        }

        if (dto.DisponibilidadeMusica)
        {
            musica.AlterarDisponibilidade(dto.DisponibilidadeMusica);
            await Console.Out.WriteLineAsync($"A disponibilidade desta música foi alterarada para {musica.Disponivel}");
        }

        if (string.IsNullOrEmpty(dto.Imagem)) return "Música editada com sucesso!";
        musica.AlterarImagem(dto.Imagem);
        await Console.Out.WriteLineAsync($"O caminho da imagem desta música foi alterado para {musica.Imagem}");

        return "Música editada com sucesso!";

    }
}
