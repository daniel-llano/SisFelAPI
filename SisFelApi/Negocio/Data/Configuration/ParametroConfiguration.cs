using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;
using System.Reflection.Emit;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class ParametroConfiguration : IEntityTypeConfiguration<Parametro>
    {
        public void Configure(EntityTypeBuilder<Parametro> entity)
        {
            entity.HasKey(e => new { e.Codigoparametro }).HasName("parametros_pkey");

            entity.ToTable("parametros");

            entity.Property(e => e.Codigoparametro).HasColumnName("codigoparametro");
            entity.Property(e => e.Nombregrupo)
                .HasMaxLength(10)
                .HasColumnName("nombregrupo");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("true")
                .HasColumnName("activo");
            entity.Property(e => e.Nombreparametro)
                .HasMaxLength(70)
                .HasColumnName("nombreparametro");
        }
    }
}