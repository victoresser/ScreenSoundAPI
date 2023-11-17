namespace ScreenSound.Dominio.Services;

public interface IRepositorio<TEntity>
{
    Task Adicionar(TEntity tentidade);
    Task<List<TEntity>> Consultar();
    Task Deletar(int id);
}
