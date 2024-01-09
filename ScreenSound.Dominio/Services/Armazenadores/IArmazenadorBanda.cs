using ScreenSound.Dominio.Models.Bandas;
using ScreenSound.Dominio.Models.Bandas.Dto;

namespace ScreenSound.Dominio.Services.Armazenadores;

public interface IArmazenadorBanda
{
    Task<string> Armazenar(CreateBandaDto dto);
    Task<string> Editar(int id, string nome, string descricao);
}
