using ScreenSound.Dominio.Interfaces.Conversores;
using ScreenSound.Dominio.Models.Musicas;

namespace ScreenSound.Dominio.Models.Albuns.Dto;

public class ConversorMusicasDoAlbum : IConversorMusicasDoAlbum
{
    public ICollection<string> ConverterParaListagemDeMusicas(IEnumerable<Musica>? musicas) =>
        musicas.Select(x => x.Nome).ToList();
}