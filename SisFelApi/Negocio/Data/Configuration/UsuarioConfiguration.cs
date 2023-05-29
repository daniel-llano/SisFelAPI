using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {


        public void Configure(EntityTypeBuilder<Usuario> entity)
        {
            entity.HasKey(e => e.Nombreusuario).HasName("pk_usuario");

            entity.ToTable("usuario");

            entity.Property(e => e.Nombreusuario)
                .HasMaxLength(30)
                .HasColumnName("nombreusuario");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("true")
                .HasColumnName("activo");
            entity.Property(e => e.Am)
                .HasMaxLength(30)
                .HasColumnName("am");
            entity.Property(e => e.Ap)
                .HasMaxLength(30)
                .HasColumnName("ap");
            entity.Property(e => e.Ci)
                .HasMaxLength(15)
                .HasColumnName("ci");
            entity.Property(e => e.Clave)
                .HasMaxLength(64)
                .IsFixedLength()
                .HasColumnName("clave");
            entity.Property(e => e.Codigorol).HasColumnName("codigorol");
            entity.Property(e => e.Nombres)
                .HasMaxLength(40)
                .HasColumnName("nombres");
            entity.Property(e => e.Telefono)
                .HasMaxLength(40)
                .HasColumnName("telefono");

            entity.HasOne(d => d.CodigorolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Codigorol)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_rol_usuario");
        }
    }
}