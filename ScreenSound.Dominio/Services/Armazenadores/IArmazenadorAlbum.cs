using ScreenSound.Dominio.Models.Bandas;

namespace ScreenSound.Dominio.Services.Armazenadores;

public interface IArmazenadorAlbum
{
    Task<string> Armazenar(string nome, Banda banda);
    Task<string> Editar(int id, string? nome, string? nomeBanda, string? imagem);
}
