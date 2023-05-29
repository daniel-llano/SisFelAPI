using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;
using System.Reflection.Emit;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class EventoConfiguration : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> entity)
        {
            entity.HasKey(e => e.Codigoevento).HasName("evento_pkey");

            entity.ToTable("evento");

            entity.Property(e => e.Codigoevento).HasColumnName("codigoevento");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("true")
                .HasColumnName("activo");
            entity.Property(e => e.Cafccompraventa)
                .HasMaxLength(50)
                .HasColumnName("cafccompraventa");
            entity.Property(e => e.Cafctelecom)
                .HasMaxLength(50)
                .HasColumnName("cafctelecom");
            entity.Property(e => e.Codigomotivoevento).HasColumnName("codigomotivoevento");
            entity.Property(e => e.Codigopuntoventa).HasColumnName("codigopuntoventa");
            entity.Property(e => e.Codigorecepcionevento).HasColumnName("codigorecepcionevento");
            entity.Property(e => e.Cufd)
                .HasMaxLength(100)
                .HasColumnName("cufd");
            entity.Property(e => e.Cufdevento)
                .HasMaxLength(100)
                .HasColumnName("cufdevento");
            entity.Property(e => e.Cuis)
                .HasMaxLength(16)
                .HasColumnName("cuis");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(1500)
                .HasColumnName("descripcion");
            entity.Property(e => e.Fechahorafinevento)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fechahorafinevento");
            entity.Property(e => e.Fechahorainicioevento)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fechahorainicioevento");

            entity.HasOne(d => d.CodigomotivoeventoNavigation).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.Codigomotivoevento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("evento_codigomotivoevento_fkey");
        }
    }
}