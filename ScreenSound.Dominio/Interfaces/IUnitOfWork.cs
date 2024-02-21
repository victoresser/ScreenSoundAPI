namespace ScreenSound.Dominio.Interfaces;

public interface IUnitOfWork
{
    Task Commit();
}
