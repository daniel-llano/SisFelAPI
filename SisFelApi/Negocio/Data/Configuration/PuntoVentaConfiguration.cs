using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class PuntoVentaConfiguration : IEntityTypeConfiguration<Puntoventum>
    {
        public void Configure(EntityTypeBuilder<Puntoventum> entity)
        {
            entity.HasKey(e => e.Codigopuntoventa).HasName("puntoventa_pkey");

            entity.ToTable("puntoventa");

            entity.Property(e => e.Codigopuntoventa)
                .ValueGeneratedNever()
                .HasColumnName("codigopuntoventa");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("true")
                .HasColumnName("activo");
            entity.Property(e => e.Codigosucursal).HasColumnName("codigosucursal");
            entity.Property(e => e.Nombrepuntoventa)
                .HasMaxLength(60)
                .HasColumnName("nombrepuntoventa");
            entity.Property(e => e.Tipopuntoventa)
                .HasMaxLength(60)
                .HasColumnName("tipopuntoventa");

            entity.HasOne(d => d.CodigosucursalNavigation).WithMany(p => p.Puntoventa)
                .HasForeignKey(d => d.Codigosucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("puntoventa_codigosucursal_fkey");
        }
    }

   
}
