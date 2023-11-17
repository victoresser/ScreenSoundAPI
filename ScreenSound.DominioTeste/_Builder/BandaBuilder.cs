using Bogus;
using ScreenSound.Dominio.Models.Bandas;

namespace ScreenSound.DominioTeste._Builder;

public class BandaBuilder
{
    protected int Id;
    protected string Nome;
    protected string Descricao;

    public static BandaBuilder Novo()
    {
        var faker = new Faker();

        return new BandaBuilder
        {
            Nome = faker.Name.FirstName(),
            Descricao = faker.Lorem.Paragraph(1)
        };
    }

    public BandaBuilder ComNome(string nome)
    {
        Nome = nome;
        return this;
    }

    public BandaBuilder ComDescricao(string descricao)
    {
        Descricao = descricao;
        return this;
    }

    public BandaBuilder ComId(int id)
    {
        Id = id;
        return this;
    }

    public Banda Build()
    {
        var banda = new Banda(Nome, Descricao);

        if (Id <= 0) return banda;

        var propertyInfo = banda.GetType().GetProperty("Id");
        propertyInfo.SetValue(banda, Convert.ChangeType(Id, propertyInfo.PropertyType), null);

        return banda;
    }

    public async Task<Banda> BuildAsync()
    {
        var banda = new Banda(Nome, Descricao);

        if (Id <= 0) return banda;

        var propertyInfo = banda.GetType().GetProperty("Id");
        propertyInfo.SetValue(banda, Convert.ChangeType(Id, propertyInfo.PropertyType), null);

        return await Task.FromResult(banda);
    }
}
