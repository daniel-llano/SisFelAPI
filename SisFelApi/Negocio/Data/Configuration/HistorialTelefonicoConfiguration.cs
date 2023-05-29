using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class HistorialTelefonicoConfiguration : IEntityTypeConfiguration<Historialtelefonico>
    {
        public void Configure(EntityTypeBuilder<Historialtelefonico> entity)
        {
            entity.HasKey(e => e.Codigohistorialtelefonico).HasName("historialtelefonico_pkey");

            entity.ToTable("historialtelefonico");

            entity.Property(e => e.Codigohistorialtelefonico).HasColumnName("codigohistorialtelefonico");
            entity.Property(e => e.Activo)
                        .HasDefaultValueSql("true")
                        .HasColumnName("activo");
            entity.Property(e => e.Codigocliente)
                        .HasMaxLength(10)
                        .HasColumnName("codigocliente");
            entity.Property(e => e.Codigotelefonocliente)
                .HasMaxLength(10)
                .HasColumnName("codigotelefonocliente");
            entity.Property(e => e.Codigotipodocumentoidentidad).HasColumnName("codigotipodocumentoidentidad");
            entity.Property(e => e.Fechacambio).HasColumnName("fechacambio");
            entity.Property(e => e.Nit)
                .HasMaxLength(20)
                .HasColumnName("nit");
            entity.Property(e => e.Ci)
                .HasMaxLength(20)
                .HasColumnName("ci");
            entity.Property(e => e.Complemento)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("complemento");
            entity.Property(e => e.Razonsocial)
                        .HasMaxLength(100)
                        .HasColumnName("razonsocial");
            entity.Property(e => e.Telefono).HasColumnName("telefono");
            entity.Property(e => e.Email)
                .HasMaxLength(70)
                .HasColumnName("email");

            entity.HasOne(d => d.CodigoclienteNavigation).WithMany(p => p.Historialtelefonicos)
                        .HasForeignKey(d => d.Codigocliente)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("historialtelefonico_codigocliente_fkey");
        }
    }
}
