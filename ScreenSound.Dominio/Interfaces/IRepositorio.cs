namespace ScreenSound.Dominio.Interfaces;

public interface IRepositorio<TEntity>
{
    Task Adicionar(TEntity tentidade);
    Task<List<TEntity>> ConsultarAsync();
    Task Deletar(int id);
}
