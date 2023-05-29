using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;
using System.Reflection.Emit;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class FacturacionMasivaConfiguration : IEntityTypeConfiguration<Facturacionmasiva>
    {
        public void Configure(EntityTypeBuilder<Facturacionmasiva> entity)
        {
                entity.HasKey(e => e.Codigofacturacionmasiva).HasName("facturacionmasiva_pkey");

                entity.ToTable("facturacionmasiva");

                entity.Property(e => e.Codigofacturacionmasiva).HasColumnName("codigofacturacionmasiva");
                entity.Property(e => e.Codigorecepcion)
                    .HasMaxLength(100)
                    .HasColumnName("codigorecepcion");
                entity.Property(e => e.Cufdmasivo)
                    .HasMaxLength(120)
                    .HasColumnName("cufdmasivo");
                entity.Property(e => e.Estado).HasColumnName("estado");
                entity.Property(e => e.Fechafin)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("fechafin");
                entity.Property(e => e.Fechainicio)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("fechainicio");
                entity.Property(e => e.Numerofacturafin).HasColumnName("numerofacturafin");
                entity.Property(e => e.Numerofacturainicio).HasColumnName("numerofacturainicio");
                entity.Property(e => e.Numerofacturasenviadas).HasColumnName("numerofacturasenviadas");
        }
    }
}
