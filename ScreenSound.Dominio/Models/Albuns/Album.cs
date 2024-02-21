using FluentValidation;
using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Models.Bandas;
using ScreenSound.Dominio.Models.Musicas;

namespace ScreenSound.Dominio.Models.Albuns;

public class Album : Entity<Album>
{
    public int BandaId { get; set; }
    public string? Imagem { get; set; }
    public virtual Banda? Banda { get; set; }
    public DateTime DataDeCriacao { get; set; }
    public virtual ICollection<Musica>? MusicasDoAlbum { get; set; }

    public Album(string nome, int bandaId, string? imagem = "")
    {
        Nome = nome;
        BandaId = bandaId;
        Imagem = imagem;
    }

    public void AlterarNome(string nome)
    {
        Nome = nome;
        Validar();
    }

    public void AlterarArtista(Banda artista)
    {
        BandaId = artista.Id;
        Validar();
    }

    public void AlterarImagem(string imagem)
    {
        Imagem = imagem;
        Validar();
    }

    public override bool Validar()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .MaximumLength(255)
            .WithMessage(Resource.NomeAlbumInvalido);

        RuleFor(x => x.Banda)
            .NotEmpty()
            .WithMessage(Resource.BandaInvalida);

        RuleFor(x => x.MusicasDoAlbum)
            .NotEmpty()
            .WithMessage(Resource.MusicaInvalida);

        return ValidationResult.Equals(true);
    }
}