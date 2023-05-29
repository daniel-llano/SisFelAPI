using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class UsuarioPuntoVentaConfiguration : IEntityTypeConfiguration<Usuariopuntoventum>
    {
        public void Configure(EntityTypeBuilder<Usuariopuntoventum> entity)
        {
            entity.HasKey(e => new { e.Nombreusuario, e.Codigopuntoventa }).HasName("usuariopuntoventa_pkey");

            entity.ToTable("usuariopuntoventa");

            entity.Property(e => e.Nombreusuario)
                .HasMaxLength(30)
                .HasColumnName("nombreusuario");
            entity.Property(e => e.Codigopuntoventa).HasColumnName("codigopuntoventa");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("true")
                .HasColumnName("activo");

            entity.HasOne(d => d.CodigopuntoventaNavigation).WithMany(p => p.Usuariopuntoventa)
                .HasForeignKey(d => d.Codigopuntoventa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuariopuntoventa_codigopuntoventa_fkey");

            entity.HasOne(d => d.NombreusuarioNavigation).WithMany(p => p.Usuariopuntoventa)
                .HasForeignKey(d => d.Nombreusuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuariopuntoventa_nombreusuario_fkey");
        }
    }
}