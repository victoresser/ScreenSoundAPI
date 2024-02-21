using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScreenSound.Dominio.Models.Bandas;

namespace ScreenSound.Dados.Configurations;

public class BandaConfiguration : IEntityTypeConfiguration<Banda>
{
    public void Configure(EntityTypeBuilder<Banda> builder)
    {
        builder.ToTable("bandas");

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.Nome)
            .HasColumnName("nome")
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(x => x.Descricao)
            .HasColumnName("descricao")
            .HasColumnType("varchar(5000)")
            .IsRequired();

        builder.Property(x => x.DataDeCriacao)
            .HasColumnName("data_criacao")
            .HasColumnType("datetime")
            .HasDefaultValueSql("getdate()")
            .IsRequired();

        builder.Property(x => x.Imagem)
            .HasColumnType("varchar(255)")
            .HasColumnName("caminhoDaImagem")
            .HasDefaultValue("");

        builder.Ignore(x => x.RuleLevelCascadeMode);
        builder.Ignore(x => x.ClassLevelCascadeMode);
    }
}
