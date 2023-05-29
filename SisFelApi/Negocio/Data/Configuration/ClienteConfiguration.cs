using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;
using System.Reflection.Emit;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> entity)
        {
            entity.HasKey(e => e.Codigocliente).HasName("cliente_pkey");

            entity.ToTable("cliente");

            entity.Property(e => e.Codigocliente)
                .HasMaxLength(10)
                .HasColumnName("codigocliente");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("true")
                .HasColumnName("activo");
            entity.Property(e => e.Datoscliente)
                .HasMaxLength(100)
                .HasColumnName("datoscliente");
            entity.Property(e => e.Ci)
                .HasMaxLength(50)
                .HasColumnName("ci");
            entity.Property(e => e.Tipopersona)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("tipopersona");
        }
    }
}