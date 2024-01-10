namespace ScreenSound.Dominio.Services;

public interface IRepositorio<TEntity>
{
    Task Adicionar(TEntity tentidade);
    Task<List<TEntity>> ConsultarAsync();
    Task Deletar(int id);
}
