using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScreenSound.Dominio.Models.Musicas;

namespace ScreenSound.Dados.Configurations;

public class MusicaConfiguration : IEntityTypeConfiguration<Musica>
{
    public void Configure(EntityTypeBuilder<Musica> builder)
    {
        builder.ToTable("musicas");

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.Nome)
            .HasColumnName("nome")
            .HasColumnType("varchar(255)");

        builder.Property(x => x.Duracao)
            .HasColumnName("duracao")
            .IsRequired();

        builder.Property(x => x.Disponivel)
            .HasColumnName("disponivel")
            .HasDefaultValueSql("1");

        builder.Property(x => x.DataDeCriacao)
            .HasColumnName("data_criacao")
            .HasColumnType("datetime")
            .HasDefaultValueSql("getdate()")
            .IsRequired();

        builder.Property(x => x.BandaId)
            .HasColumnName("banda_id");

        builder.Property(x => x.AlbumId)
            .HasColumnName("album_id");

        builder.Property(x => x.Imagem)
            .HasColumnType("varchar(255)")
            .HasColumnName("caminhoDaImagem")
            .HasDefaultValue("");

        builder.HasOne(x => x.Album)
            .WithMany(x => x.MusicasDoAlbum)
            .HasForeignKey(x => x.AlbumId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Banda)
            .WithMany(x => x.MusicasDaBanda)
            .HasForeignKey(x => x.BandaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.RuleLevelCascadeMode);
        builder.Ignore(x => x.ClassLevelCascadeMode);
        builder.Ignore(x => x.CascadeMode);
    }
}