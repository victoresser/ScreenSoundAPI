using ScreenSound.Dominio.Models.Albuns;
using ScreenSound.Dominio.Services.Conversores;

namespace ScreenSound.Dominio.Models.Bandas.Dto;

public class ConversorAlbunsDaBanda : IConversorAlbunsDaBanda
{
    public ICollection<string> ConverterParaListagemDeAlbuns(ICollection<Album> albuns) =>
        albuns.Select(x => x.Nome).ToList();
}
