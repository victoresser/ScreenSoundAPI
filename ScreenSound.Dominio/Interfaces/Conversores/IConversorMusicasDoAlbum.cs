using ScreenSound.Dominio.Models.Musicas;

namespace ScreenSound.Dominio.Interfaces.Conversores;

public interface IConversorMusicasDoAlbum
{
    ICollection<string> ConverterParaListagemDeMusicas(IEnumerable<Musica>? musicas);
}