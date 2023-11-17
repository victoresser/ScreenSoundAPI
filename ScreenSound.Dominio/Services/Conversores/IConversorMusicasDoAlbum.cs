using ScreenSound.Dominio.Models.Musicas;

namespace ScreenSound.Dominio.Services.Conversores;

public interface IConversorMusicasDoAlbum
{
    ICollection<string> ConverterParaListagemDeMusicas(ICollection<Musica> musicas);
}