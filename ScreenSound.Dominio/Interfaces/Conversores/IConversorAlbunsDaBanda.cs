using ScreenSound.Dominio.Models.Albuns;

namespace ScreenSound.Dominio.Interfaces.Conversores;

public interface IConversorAlbunsDaBanda
{
    ICollection<string> ConverterParaListagemDeAlbuns(ICollection<Album>? albuns);
}
