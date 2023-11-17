using ScreenSound.Dominio.Models.Bandas;

namespace ScreenSound.Dominio.Services.Armazenadores;

public interface IArmazenadorBanda
{
    Task<string> Armazenar(Banda bandaDto);
    Task<string> Editar(int id, string nome, string descricao);
}
