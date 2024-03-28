using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Interfaces;
using ScreenSound.Dominio.Interfaces.Armazenadores;
using ScreenSound.Dominio.Interfaces.Repositorios;
using ScreenSound.Dominio.Models.Musicas.Dto;

namespace ScreenSound.Dominio.Models.Musicas.Services;

public class ArmazenadorDeMusicas : IArmazenadorMusica
{
    private readonly IMusicaRepositorio _musicaRepositorio;
    private readonly IBandaRepositorio _bandaRepositorio;
    private readonly IAlbumRepositorio _albumRepositorio;
    private readonly IBase64Cleaner _base64Cleaner;

    public ArmazenadorDeMusicas(IMusicaRepositorio musicaRepositorio, IBandaRepositorio bandaRepositorio,
        IAlbumRepositorio albumRepositorio, IBase64Cleaner base64Cleaner)
    {
        _musicaRepositorio = musicaRepositorio;
        _bandaRepositorio = bandaRepositorio;
        _albumRepositorio = albumRepositorio;
        _base64Cleaner = base64Cleaner;
    }

    public async Task<string> Armazenar(CreateMusicaDto dto)
    {
        byte[]? imagemBase64 = null;

        if (string.IsNullOrWhiteSpace(dto.Nome))
            throw new ArgumentException("Erro | O nome da música não pode ser nulo ou vazio.");

        var musicaJaSalva = await _musicaRepositorio.ObterPorNome(dto.Nome);

        if (string.IsNullOrWhiteSpace(dto.NomeAlbum))
            throw new ArgumentException("Erro | O nome da álbum não pode ser nulo ou vazio.");

        var album = await _albumRepositorio.ObterPorNome(dto.NomeAlbum);

        if (string.IsNullOrWhiteSpace(dto.NomeBanda))
            throw new ArgumentException("Erro | O nome da banda não pode ser nulo ou vazio.");

        var banda = await _bandaRepositorio.ObterPorNome(dto.NomeBanda);

        if (musicaJaSalva != null && musicaJaSalva.Nome == dto.Nome)
            throw new ArgumentException(Resource.MusicaExistente);

        if (banda == null)
            throw new ArgumentException(Resource.BandaInexistente);

        if (album == null)
            throw new ArgumentException(Resource.AlbumInexistente);

        if (dto.Nome != null && dto.Nome.Length > 255)
            throw new ArgumentException(Resource.NomeAlbumInvalido);

        if (!string.IsNullOrEmpty(dto.Imagem))
            imagemBase64 = _base64Cleaner.ConverterStringBase64ParaBytes(dto.Imagem);

        var musica = new Musica(
            dto.Nome ?? string.Empty,
            dto.Duracao,
            album.Id,
            banda.Id,
            dto.Disponibilidade,
            imagemBase64 ?? null);

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

        musica.AlterarDisponibilidade(dto.DisponibilidadeMusica);
        await Console.Out.WriteLineAsync($"A disponibilidade desta música foi alterarada para {musica.Disponivel}");

        if (dto.Imagem == null) return "Música editada com sucesso!";
        
        musica.AlterarImagem(_base64Cleaner.ConverterStringBase64ParaBytes(dto.Imagem));
        await Console.Out.WriteLineAsync($"O caminho da imagem desta música foi alterado para {musica.Imagem}");

        return "Música editada com sucesso!";
    }
}