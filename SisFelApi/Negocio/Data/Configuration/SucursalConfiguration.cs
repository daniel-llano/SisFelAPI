using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class SucursalConfiguration : IEntityTypeConfiguration<Sucursal>
    {
        public void Configure(EntityTypeBuilder<Sucursal> entity)
        {
            entity.HasKey(e => e.Codigosucursal).HasName("sucursal_pkey");

            entity.ToTable("sucursal");

            entity.HasIndex(e => e.Nombresucursal, "unq_sucursal_nombresucursal").IsUnique();

            entity.Property(e => e.Codigosucursal)
                        .ValueGeneratedNever()
                        .HasColumnName("codigosucursal");
            entity.Property(e => e.Activo)
                        .HasDefaultValueSql("true")
                        .HasColumnName("activo");
            entity.Property(e => e.Barrio)
                        .HasMaxLength(60)
                        .HasColumnName("barrio");
            entity.Property(e => e.Direccion)
                        .HasMaxLength(100)
                        .HasColumnName("direccion");
            entity.Property(e => e.Municipio)
                        .HasMaxLength(35)
                        .HasColumnName("municipio");
            entity.Property(e => e.Nombresucursal)
                        .HasMaxLength(60)
                        .HasColumnName("nombresucursal");
            entity.Property(e => e.Telefono)
                        .HasMaxLength(40)
                        .HasColumnName("telefono");
        }
    }
}
