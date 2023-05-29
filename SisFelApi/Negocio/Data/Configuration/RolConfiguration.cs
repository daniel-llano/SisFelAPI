using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class RolConfiguration : IEntityTypeConfiguration<Rol>
    {


        public void Configure(EntityTypeBuilder<Rol> entity)
        {
            entity.HasKey(e => e.Codigorol).HasName("pk_rol");

            entity.ToTable("rol");

            entity.HasIndex(e => e.Nombrerol, "unq_rol_nombre").IsUnique();

            entity.Property(e => e.Codigorol).HasColumnName("codigorol");
            entity.Property(e => e.Activo)
                    .HasDefaultValueSql("true")
                    .HasColumnName("activo");
            entity.Property(e => e.Descripcion)
                    .HasMaxLength(150)
                    .HasColumnName("descripcion");
            entity.Property(e => e.Nombrerol)
                    .HasMaxLength(30)
                    .HasColumnName("nombrerol");
        }
    }
}