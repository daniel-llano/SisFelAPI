using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;
using System.Reflection.Emit;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> entity)
        {
            entity.HasKey(e => e.Codigoproducto).HasName("producto_pkey");

            entity.ToTable("producto");

            entity.Property(e => e.Codigoproducto)
                .HasMaxLength(50)
                .HasColumnName("codigoproducto");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("true")
                .HasColumnName("activo");
            entity.Property(e => e.Codigocategoria).HasColumnName("codigocategoria");
            entity.Property(e => e.Codigounidadmedida).HasColumnName("codigounidadmedida");
            entity.Property(e => e.Nombreproducto)
                .HasMaxLength(500)
                .HasColumnName("nombreproducto");
            entity.Property(e => e.Precio)
                .HasPrecision(19, 2)
                .HasColumnName("precio");
            entity.Property(e => e.Tipoproducto)
                .HasMaxLength(1)
                .HasColumnName("tipoproducto");

            entity.HasOne(d => d.CodigocategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.Codigocategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("producto_codigocategoria_fkey");
            entity.HasOne(d => d.CodigounidadmedidaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.Codigounidadmedida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("producto_codigounidadmedida_fkey");
        }
    }
}

