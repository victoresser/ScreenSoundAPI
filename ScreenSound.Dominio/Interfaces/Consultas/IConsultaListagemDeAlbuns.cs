using ScreenSound.Dominio.Models.Albuns.Dto;

namespace ScreenSound.Dominio.Interfaces.Consultas;

public interface IConsultaListagemDeAlbuns
{
    Task<IEnumerable<ListagemDeAlbuns>> RetornaListagemDeAlbuns(string? nomeAlbum, int skip, int take);
    Task<ListagemDeAlbuns> RetornaListagemDeAlbunsPorId(int id);
}