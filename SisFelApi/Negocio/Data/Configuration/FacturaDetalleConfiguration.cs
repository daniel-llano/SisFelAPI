using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class FacturaDetalleConfiguration : IEntityTypeConfiguration<Facturadetalle>
    {
        public void Configure(EntityTypeBuilder<Facturadetalle> entity)
        {
            entity.HasKey(e => e.Codigofacturadetalle).HasName("facturadetalle_pkey");

            entity.ToTable("facturadetalle");

            entity.Property(e => e.Codigofacturadetalle).HasColumnName("codigofacturadetalle");
            entity.Property(e => e.Actividadeconomica)
                .HasMaxLength(100)
                .HasColumnName("actividadeconomica");
            entity.Property(e => e.Cantidad)
                .HasPrecision(7, 2)
                .HasColumnName("cantidad");
            entity.Property(e => e.Codigofactura).HasColumnName("codigofactura");
            entity.Property(e => e.Codigogrupo)
                .HasMaxLength(3)
                .HasColumnName("codigogrupo");
            entity.Property(e => e.Codigoproducto)
                .HasMaxLength(5)
                .HasColumnName("codigoproducto");
            entity.Property(e => e.Codigoproductosin).HasColumnName("codigoproductosin");
            entity.Property(e => e.Cuenta)
                .HasMaxLength(10)
                .HasColumnName("cuenta");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .HasColumnName("descripcion");
            entity.Property(e => e.Montodescuento)
                .HasPrecision(9, 2)
                .HasColumnName("montodescuento");
            entity.Property(e => e.Numeroimei)
                .HasMaxLength(100)
                .HasColumnName("numeroimei");
            entity.Property(e => e.Numeroserie)
                .HasMaxLength(100)
                .HasColumnName("numeroserie");
            entity.Property(e => e.Preciounitario)
                .HasPrecision(9, 2)
                .HasColumnName("preciounitario");
            entity.Property(e => e.Subtotal)
                .HasPrecision(9, 2)
                .HasColumnName("subtotal");
            entity.Property(e => e.Unidadmedida).HasColumnName("unidadmedida");

            entity.HasOne(d => d.CodigofacturaNavigation).WithMany(p => p.Facturadetalles)
                .HasForeignKey(d => d.Codigofactura)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("facturadetalle_codigofactura_fkey");

            entity.HasOne(d => d.CodigoproductoNavigation).WithMany(p => p.Facturadetalles)
                .HasForeignKey(d => d.Codigoproducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("facturadetalle_codigoproducto_fkey");
        }
    }
}
