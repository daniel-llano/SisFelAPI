using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;
using System.Reflection.Emit;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categorium>
    {
        public void Configure(EntityTypeBuilder<Categorium> entity)
        {
            entity.HasKey(e => e.Codigocategoria).HasName("categoria_pkey");

            entity.ToTable("categoria");

            entity.Property(e => e.Codigocategoria)
                .ValueGeneratedNever()
                .HasColumnName("codigocategoria");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("true")
                .HasColumnName("activo");
            entity.Property(e => e.Codigoactividad).HasColumnName("codigoactividad");
            entity.Property(e => e.Codigoproductosin).HasColumnName("codigoproductosin");
            entity.Property(e => e.Descripcionproducto)
                .HasMaxLength(500)
                .HasColumnName("descripcionproducto");
        }
    }
}