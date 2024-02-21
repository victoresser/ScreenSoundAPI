using FluentValidation;
using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Models.Albuns;
using ScreenSound.Dominio.Models.Bandas;

namespace ScreenSound.Dominio.Models.Musicas;

public class Musica : Entity<Musica>
{
    public short Duracao { get; set; }
    public bool? Disponivel { get; set; }
    public DateTime DataDeCriacao { get; set; }
    public int AlbumId { get; set; }
    public int BandaId { get; set; }
    public string? Imagem { get; set; }
    public virtual Album Album { get; set; }
    public virtual Banda Banda { get; set; }

    public Musica(string nome, short duracao, int albumId, int bandaId, bool? disponivel = false, string? imagem = "")
    {
        Nome = nome;
        Duracao = duracao;
        Disponivel = disponivel;
        AlbumId = albumId;
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
        Banda = artista;
        Validar();
    }

    public void AlterarDuracao(short duracao)
    {
        Duracao = duracao;
        Validar();
    }

    public void AlterarDisponibilidade(bool disponibilidade)
    {
        Disponivel = disponibilidade;
        Validar();
    }

    public void AlterarAlbum(Album album)
    {
        Album = album;
        Validar();
    }

    public void AlterarImagem(string? imagem)
    {
        Imagem = imagem;
        Validar();
    }

    public override bool Validar()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .NotNull()
            .MaximumLength(255)
            .WithMessage(Resource.NomeMusicaInvalido);

        RuleFor(x => x.Duracao)
            .NotEmpty()
            .NotNull()
            .When(x => x.Duracao > 1)
            .WithMessage(Resource.DuracaoMusicaInvalida);

        RuleFor(x => x.Banda)
            .NotEmpty()
            .NotNull()
            .WithMessage(Resource.BandaInvalida);

        RuleFor(x => x.Album)
            .NotNull()
            .NotEmpty()
            .WithMessage(Resource.AlbumInvalido);

        RuleFor(x => x.Disponivel)
            .NotNull()
            .NotEmpty()
            .When(x => x.Disponivel == true || x.Disponivel == false);


        ValidationResult = Validate(this);
        return ValidationResult.IsValid;
    }
}