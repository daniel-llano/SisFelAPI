using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;
using System.Reflection.Emit;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class TelefonoClienteConfiguration : IEntityTypeConfiguration<Telefonocliente>
    {
        public void Configure(EntityTypeBuilder<Telefonocliente> entity)
        {
            entity.HasKey(e => e.Codigotelefonocliente).HasName("telefonocliente_pkey");

            entity.ToTable("telefonocliente");

            entity.Property(e => e.Codigotelefonocliente)
                .HasMaxLength(10)
                .HasColumnName("codigotelefonocliente");
            entity.Property(e => e.Codigotipodocumentoidentidad).HasColumnName("codigotipodocumentoidentidad");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("true")
                .HasColumnName("activo");
            entity.Property(e => e.Codigocliente)
                .HasMaxLength(10)
                .HasColumnName("codigocliente");
            entity.Property(e => e.Nit)
                .HasMaxLength(20)
                .HasColumnName("nit");
            entity.Property(e => e.Ci)
                .HasMaxLength(20)
                .HasColumnName("ci");
            entity.Property(e => e.Complemento)
                .HasMaxLength(5)
                .HasColumnName("complemento");
            entity.Property(e => e.Razonsocial)
                .HasMaxLength(500)
                .HasColumnName("razonsocial");
            entity.Property(e => e.Telefono).HasColumnName("telefono");
            entity.Property(e => e.Email)
                .HasMaxLength(70)
                .HasColumnName("email");

            entity.HasOne(d => d.CodigoclienteNavigation).WithMany(p => p.Telefonoclientes)
                .HasForeignKey(d => d.Codigocliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("telefonocliente_codigocliente_fkey");
        }
    }
}