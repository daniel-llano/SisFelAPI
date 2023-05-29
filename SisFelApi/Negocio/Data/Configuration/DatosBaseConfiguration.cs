using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;
using System.Reflection.Emit;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class DatosBaseConfiguration : IEntityTypeConfiguration<Datosbase>
    {
        public void Configure(EntityTypeBuilder<Datosbase> entity)
        {
            entity.HasKey(e => e.Codigodatobase).HasName("datosbase_pkey");

            entity.ToTable("datosbase");

            entity.Property(e => e.Codigodatobase)
                .HasMaxLength(1)
                .HasDefaultValueSql("1")
                .HasColumnName("codigodatobase");
            entity.Property(e => e.Nrofacturapaquetecv).HasColumnName("nrofacturapaquetecv");
            entity.Property(e => e.Nrofacturapaquetetl).HasColumnName("nrofacturapaquetetl");
            entity.Property(e => e.Nropaquetecv).HasColumnName("nropaquetecv");
            entity.Property(e => e.Nropaquetetl).HasColumnName("nropaquetetl");
            entity.Property(e => e.Nrofacturapaquetemasivocv).HasColumnName("nrofacturapaquetemasivocv");
            entity.Property(e => e.Nrofacturapaquetemasivotl).HasColumnName("nrofacturapaquetemasivotl");
            entity.Property(e => e.Nropaquetemasivocv).HasColumnName("nropaquetemasivocv");
            entity.Property(e => e.Nropaquetemasivotl).HasColumnName("nropaquetemasivotl");
        }
    }
}

