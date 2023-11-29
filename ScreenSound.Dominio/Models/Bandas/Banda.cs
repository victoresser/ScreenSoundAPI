using FluentValidation;
using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Models.Albuns;
using ScreenSound.Dominio.Models.Musicas;

namespace ScreenSound.Dominio.Models.Bandas;

public class Banda : Entity<Banda>
{
    public string Descricao { get; set; }
    public DateTime DataDeCriacao { get; set; }
    public string? Imagem { get; set; }
    public virtual ICollection<Album>? AlbunsDaBanda { get; set; }
    public virtual ICollection<Musica>? MusicasDaBanda { get; set; }

    public Banda(string nome, string descricao = "", string? imagem = "")
    {
        Nome = nome;
        Descricao = descricao;
        Imagem = imagem;
    }

    public void AlterarNome(string nome)
    {
        Nome = nome;
        Validar();
    }

    public void AlterarDescricao(string descricao)
    {
        Descricao = descricao;
        Validar();
    }

    public override bool Validar()
    {
        RuleFor(x => x.Nome)
            .NotNull()
            .NotEmpty()
            .MaximumLength(255)
            .WithMessage(Resource.NomeInvalido);

        RuleFor(x => x.Descricao)
            .NotNull()
            .NotEmpty()
            .MaximumLength(5000)
            .WithMessage(Resource.DescricaoBandaInvalida);


        ValidationResult = Validate(this);
        return ValidationResult.IsValid;
    }
}