using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;
using System.Reflection.Emit;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class GeneralConfiguration : IEntityTypeConfiguration<General>
    {
        public void Configure(EntityTypeBuilder<General> entity)
        {
            entity.HasKey(e => e.Codigopuntoventa).HasName("general_pkey");

            entity.ToTable("general");

            entity.Property(e => e.Codigopuntoventa)
                .ValueGeneratedNever()
                .HasColumnName("codigopuntoventa");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(35)
                .HasColumnName("ciudad");
            entity.Property(e => e.Codigoautorizacion).HasColumnName("codigoautorizacion");
            entity.Property(e => e.Codigocontrol)
                .HasMaxLength(20)
                .HasColumnName("codigocontrol");
            entity.Property(e => e.Codigosistema)
                .HasMaxLength(40)
                .HasColumnName("codigosistema");
            entity.Property(e => e.Cufd).HasColumnName("cufd");
            entity.Property(e => e.Cuis)
                .HasMaxLength(16)
                .HasColumnName("cuis");
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .HasColumnName("direccion");
            entity.Property(e => e.Fechavigenciacufd).HasColumnName("fechavigenciacufd");
            entity.Property(e => e.Fechavigenciacuis).HasColumnName("fechavigenciacuis");
            entity.Property(e => e.Nit)
                .HasMaxLength(20)
                .HasColumnName("nit");
            entity.Property(e => e.Nombreempresa)
                .HasMaxLength(200)
                .IsFixedLength()
                .HasColumnName("nombreempresa");
            entity.Property(e => e.Telefono)
                .HasMaxLength(40)
                .HasColumnName("telefono");
        }
    }
}

