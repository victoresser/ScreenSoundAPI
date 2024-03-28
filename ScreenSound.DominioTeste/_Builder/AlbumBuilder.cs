using Bogus;
using ScreenSound.Dominio.Models.Albuns;
using ScreenSound.Dominio.Models.Bandas;

namespace ScreenSound.DominioTeste._Builder;

public class AlbumBuilder
{
    protected int Id;
    protected string? Nome;
    protected Banda Banda;
    protected string? Imagem;

    public static AlbumBuilder Novo()
    {
        var faker = new Faker();

        return new AlbumBuilder
        {
            Nome = faker.Name.FirstName(),
            Banda = BandaBuilder.Novo().Build(),
        };
    }

    public AlbumBuilder ComNome(string? nome)
    {
        Nome = nome;
        return this;
    }

    public AlbumBuilder ComBanda(Banda banda)
    {
        Banda = banda;
        return this;
    }

    public AlbumBuilder ComId(int id)
    {
        Id = id;
        return this;
    }

    public AlbumBuilder ComImagem(string imagem)
    {
        Imagem = imagem;
        return this;
    }

    public Album Build()
    {
        var album = new Album(Nome, Banda.Id);

        if (Id <= 0) return album;

        var propertyInfo = album.GetType().GetProperty("Id");
        propertyInfo.SetValue(album, Convert.ChangeType(Id, propertyInfo.PropertyType), null);

        return album;
    }
}
