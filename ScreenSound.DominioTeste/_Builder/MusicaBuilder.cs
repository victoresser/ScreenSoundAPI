using Bogus;
using ScreenSound.Dominio.Models.Albuns;
using ScreenSound.Dominio.Models.Bandas;
using ScreenSound.Dominio.Models.Musicas;

namespace ScreenSound.DominioTeste._Builder;

public class MusicaBuilder
{
    protected int Id;
    protected string? Nome;
    protected short Duracao;
    protected bool? Disponivel;
    protected Album Album;
    protected Banda Banda;

    public static MusicaBuilder Novo()
    {
        var faker = new Faker();
        var banda = BandaBuilder.Novo().Build();
        var album = AlbumBuilder.Novo().Build();

        return new MusicaBuilder
        {
            Nome = faker.Name.FullName(),
            Duracao = faker.Random.Short(),
            Disponivel = faker.Random.Bool(),
            Album = album,
            Banda = banda,
        };
    }

    public MusicaBuilder ComNome(string? nome)
    {
        Nome = nome;
        return this;
    }

    public MusicaBuilder ComDuracao(short duracao)
    {
        Duracao = duracao;
        return this;
    }

    public MusicaBuilder ComDisponivel(bool disponivel)
    {
        Disponivel = disponivel;
        return this;
    }

    public MusicaBuilder ComId(int id)
    {
        Id = id;
        return this;
    }

    public MusicaBuilder ComAlbum(Album album)
    {
        Album = album;
        return this;
    }

    public MusicaBuilder ComBanda(Banda banda)
    {
        Banda = banda;
        return this;
    }

    public Musica Build()
    {
        Musica musica = new Musica(Nome, Duracao, Album.Id, Banda.Id, Disponivel);

        if (Id <= 0) return musica;

        var propertyInfo = musica.GetType().GetProperty("Id");
        propertyInfo.SetValue(musica, Convert.ChangeType(Id, propertyInfo.PropertyType), null);

        return musica;
    }
}
