using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class EnlaceConfiguration : IEntityTypeConfiguration<Enlace>
    {


        public void Configure(EntityTypeBuilder<Enlace> entity)
        {
            entity.HasKey(e => e.Codigoenlace).HasName("pk_enlace");

            entity.ToTable("enlaces");

            entity.Property(e => e.Codigoenlace).HasColumnName("codigoenlace");
            entity.Property(e => e.Activo)
                    .HasDefaultValueSql("true")
                    .HasColumnName("activo");
            entity.Property(e => e.Nombreenlace)
                    .HasMaxLength(30)
                    .HasColumnName("nombreenlace");
            entity.Property(e => e.Ruta)
                    .HasMaxLength(50)
                    .HasColumnName("ruta");

        }
    }
}