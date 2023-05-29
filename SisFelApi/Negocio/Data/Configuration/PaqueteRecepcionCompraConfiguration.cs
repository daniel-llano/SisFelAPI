using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class PaqueteRecepcionCompraConfiguration : IEntityTypeConfiguration<Paqueterecepcioncompra>
    {
        public void Configure(EntityTypeBuilder<Paqueterecepcioncompra> entity)
        {
            entity.HasKey(e => e.Codigopaqueterecepcioncompra).HasName("paqueterecepcioncompra_pkey");

            entity.ToTable("paqueterecepcioncompra");

            entity.Property(e => e.Codigopaqueterecepcioncompra).HasColumnName("codigopaqueterecepcioncompra");
            entity.Property(e => e.Cantidadfacturas).HasColumnName("cantidadfacturas");
            entity.Property(e => e.Codigorecepcionpaquete)
                .HasMaxLength(100)
                .HasColumnName("codigorecepcionpaquete");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Fechaenvio)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fechaenvio");
            entity.Property(e => e.Gestion).HasColumnName("gestion");
            entity.Property(e => e.Numerofacturafin).HasColumnName("numerofacturafin");
            entity.Property(e => e.Numerofacturainicio).HasColumnName("numerofacturainicio");
            entity.Property(e => e.Periodo).HasColumnName("periodo");
        }
    }
}