namespace ScreenSound.Dominio.Services;

public interface IUnitOfWork
{
    Task Commit();
}
