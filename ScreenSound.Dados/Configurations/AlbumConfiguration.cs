using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScreenSound.Dominio.Models.Albuns;

namespace ScreenSound.Dados.Configurations;

public class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.ToTable("albuns");

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.Nome)
            .HasColumnName("nome")
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(x => x.BandaId)
            .HasColumnName("banda_id");

        builder.Property(x => x.DataDeCriacao)
            .HasColumnName("data_criacao")
            .HasColumnType("datetime")
            .HasDefaultValueSql("getdate()")
            .IsRequired();

        builder.Property(x => x.Imagem)
            .HasColumnType("varchar(255)")
            .HasColumnName("caminhoDaImagem")
            .HasDefaultValue("");

        builder
            .HasOne(x => x.Banda)
            .WithMany(x => x.AlbunsDaBanda)
            .HasForeignKey(x => x.BandaId).IsRequired();

        builder.Ignore(x => x.RuleLevelCascadeMode);
        builder.Ignore(x => x.ClassLevelCascadeMode);
    }
}
