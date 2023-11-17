﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScreenSound.Dados;

#nullable disable

namespace ScreenSound.Dados.Migrations
{
    [DbContext(typeof(ScreenSoundContext))]
    [Migration("20231116191101_AdicionandoImagens")]
    partial class AdicionandoImagens
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ScreenSound.Dominio.Models.Albuns.Album", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BandaId")
                        .HasColumnType("int")
                        .HasColumnName("banda_id");

                    b.Property<DateTime>("DataDeCriacao")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("data_criacao")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Imagem")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)")
                        .HasDefaultValue("")
                        .HasColumnName("caminhoDaImagem");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("nome");

                    b.HasKey("Id");

                    b.HasIndex("BandaId");

                    b.ToTable("albuns", (string)null);
                });

            modelBuilder.Entity("ScreenSound.Dominio.Models.Bandas.Banda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataDeCriacao")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("data_criacao")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("varchar(5000)")
                        .HasColumnName("descricao");

                    b.Property<string>("Imagem")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)")
                        .HasDefaultValue("")
                        .HasColumnName("caminhoDaImagem");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("nome");

                    b.HasKey("Id");

                    b.ToTable("bandas", (string)null);
                });

            modelBuilder.Entity("ScreenSound.Dominio.Models.Musicas.Musica", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AlbumId")
                        .HasColumnType("int")
                        .HasColumnName("album_id");

                    b.Property<int>("BandaId")
                        .HasColumnType("int")
                        .HasColumnName("banda_id");

                    b.Property<DateTime>("DataDeCriacao")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("data_criacao")
                        .HasDefaultValueSql("getdate()");

                    b.Property<bool?>("Disponivel")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("disponivel")
                        .HasDefaultValueSql("1");

                    b.Property<short>("Duracao")
                        .HasColumnType("smallint")
                        .HasColumnName("duracao");

                    b.Property<string>("Imagem")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)")
                        .HasDefaultValue("")
                        .HasColumnName("caminhoDaImagem");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("nome");

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.HasIndex("BandaId");

                    b.ToTable("musicas", (string)null);
                });

            modelBuilder.Entity("ScreenSound.Dominio.Models.Albuns.Album", b =>
                {
                    b.HasOne("ScreenSound.Dominio.Models.Bandas.Banda", "Banda")
                        .WithMany("AlbunsDaBanda")
                        .HasForeignKey("BandaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Banda");
                });

            modelBuilder.Entity("ScreenSound.Dominio.Models.Musicas.Musica", b =>
                {
                    b.HasOne("ScreenSound.Dominio.Models.Albuns.Album", "Album")
                        .WithMany("MusicasDoAlbum")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ScreenSound.Dominio.Models.Bandas.Banda", "Banda")
                        .WithMany("MusicasDaBanda")
                        .HasForeignKey("BandaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Album");

                    b.Navigation("Banda");
                });

            modelBuilder.Entity("ScreenSound.Dominio.Models.Albuns.Album", b =>
                {
                    b.Navigation("MusicasDoAlbum");
                });

            modelBuilder.Entity("ScreenSound.Dominio.Models.Bandas.Banda", b =>
                {
                    b.Navigation("AlbunsDaBanda");

                    b.Navigation("MusicasDaBanda");
                });
#pragma warning restore 612, 618
        }
    }
}
