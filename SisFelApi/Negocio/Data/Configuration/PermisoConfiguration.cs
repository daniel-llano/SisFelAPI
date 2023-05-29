using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class PermisoConfiguration : IEntityTypeConfiguration<Permiso>
    {

        public void Configure(EntityTypeBuilder<Permiso> entity)
        {
            entity.HasKey(e => new {
                e.Codigorol,
                e.Codigoenlace
            }).HasName("permisos_pkey");

            entity.ToTable("permisos");

            entity.Property(e => e.Codigorol).HasColumnName("codigorol");
            entity.Property(e => e.Codigoenlace).HasColumnName("codigoenlace");
            entity.Property(e => e.Activo)
                        .HasDefaultValueSql("true")
                        .HasColumnName("activo");

            entity.HasOne(d => d.CodigoenlaceNavigation).WithMany(p => p.Permisos)
                        .HasForeignKey(d => d.Codigoenlace)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("permisos_codigoenlace_fkey");

            entity.HasOne(d => d.CodigorolNavigation).WithMany(p => p.Permisos)
                        .HasForeignKey(d => d.Codigorol)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("permisos_codigorol_fkey");
        }
    }
}