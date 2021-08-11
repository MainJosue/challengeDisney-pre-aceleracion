using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChallengeDisney.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ChallengeDisney.Context
{
    public class ChallengeDisneyContext : DbContext
    {
        public ChallengeDisneyContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Peliculas>().ToTable("Peliculas");
            builder.Entity<Peliculas>().HasKey(x => x.Id);
            builder.Entity<Peliculas>().HasMany<Personajes>(x => x.Personajes).WithMany(x => x.Peliculas);
            builder.Entity<Peliculas>().HasOne<Generos>(x => x.Genero).WithMany(x => x.Peliculas);

            builder.Entity<Personajes>().HasKey(x => x.Id);
            builder.Entity<Personajes>().ToTable("Personajes");
            builder.Entity<Personajes>().HasMany<Peliculas>(x => x.Peliculas).WithMany(x => x.Personajes);

            builder.Entity<Generos>().HasKey(x => x.Id);
            builder.Entity<Generos>().ToTable("Generos");
            builder.Entity<Generos>().HasMany<Peliculas>(x => x.Peliculas).WithOne(x => x.Genero).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Peliculas>(x =>
            {
                x.Property(x => x.Id).IsRequired().UseIdentityColumn();
                x.Property(x => x.Titulo).IsRequired().HasMaxLength(100);
                x.Property(x => x.FechaCreacion).IsRequired();
                x.Property(x => x.Calificacion).IsRequired();
                x.Property(x => x.Imagen).HasMaxLength(100).HasColumnType("varchar");
                //x.Property(x => x.Genero).IsRequired();
            });            

            builder.Entity<Personajes>(x =>
            {
                x.Property(x => x.Id).IsRequired().UseIdentityColumn().HasColumnType("int");
                x.Property(x => x.Nombre).IsRequired().HasMaxLength(50).HasColumnType("varchar");
                x.Property(x => x.Imagen).HasMaxLength(100).HasColumnType("varchar");
                x.Property(x => x.Año).IsRequired().HasColumnType("int");
                x.Property(x => x.Peso).HasColumnType("float");
                x.Property(x => x.Historia).HasMaxLength(250).HasColumnType("varchar");
            });

            builder.Entity<Generos>(x =>
            {
                x.Property(x => x.Id).IsRequired().UseIdentityColumn().HasColumnType("int");
                x.Property(x => x.Nombre).IsRequired().IsUnicode().HasMaxLength(50).HasColumnType("varchar");
                x.Property(x => x.Imagen).HasMaxLength(100).HasColumnType("varchar");
               
            });



        }

        public DbSet<Personajes> Personajes { get; set; } = null!;
        public DbSet<Peliculas> Peliculas { get; set; } = null!;
        public DbSet<Generos> Generos { get; set; } = null!;
    }
}
