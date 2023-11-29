using ScreenSound.Dominio.Models.Albuns;

namespace ScreenSound.Dominio.Services.Conversores;

public interface IConversorAlbunsDaBanda
{
    ICollection<string> ConverterParaListagemDeAlbuns(ICollection<Album>? albuns);
}
