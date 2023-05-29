using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class RegistroCompraConfiguration : IEntityTypeConfiguration<Registrocompra>
    {
        public void Configure(EntityTypeBuilder<Registrocompra> entity)
        {
            entity.HasKey(e => e.Codigocompra).HasName("registrocompra_pkey");

            entity.ToTable("registrocompra");

            entity.Property(e => e.Codigocompra).HasColumnName("codigocompra");
            entity.Property(e => e.Codigoautorizacion)
                .HasMaxLength(100)
                .HasColumnName("codigoautorizacion");
            entity.Property(e => e.Codigocontrol)
                .HasMaxLength(17)
                .HasColumnName("codigocontrol");
            entity.Property(e => e.Codigopaqueterecepcioncompra)
                .ValueGeneratedOnAdd()
                .HasColumnName("codigopaqueterecepcioncompra");
            entity.Property(e => e.Creditofiscal)
                .HasPrecision(16, 2)
                .HasColumnName("creditofiscal");
            entity.Property(e => e.Descuento)
                .HasPrecision(16, 2)
                .HasColumnName("descuento");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Fechaemision)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fechaemision");
            entity.Property(e => e.Importeice)
                .HasPrecision(16, 2)
                .HasColumnName("importeice");
            entity.Property(e => e.Importeiehd)
                .HasPrecision(16, 2)
                .HasColumnName("importeiehd");
            entity.Property(e => e.Importeipj)
                .HasPrecision(16, 2)
                .HasColumnName("importeipj");
            entity.Property(e => e.Importesexentos)
                .HasPrecision(16, 2)
                .HasColumnName("importesexentos");
            entity.Property(e => e.Importetasacero)
                .HasPrecision(16, 2)
                .HasColumnName("importetasacero");
            entity.Property(e => e.Montogiftcard)
                .HasPrecision(16, 2)
                .HasColumnName("montogiftcard");
            entity.Property(e => e.Montototalcompra)
                .HasPrecision(16, 2)
                .HasColumnName("montototalcompra");
            entity.Property(e => e.Montototalsujetoiva)
                .HasPrecision(16, 2)
                .HasColumnName("montototalsujetoiva");
            entity.Property(e => e.Nitemisor).HasColumnName("nitemisor");
            entity.Property(e => e.Nrocompra).HasColumnName("nrocompra");
            entity.Property(e => e.Numeroduidim)
                .HasMaxLength(15)
                .HasColumnName("numeroduidim");
            entity.Property(e => e.Numerofactura)
                .HasMaxLength(20)
                .HasColumnName("numerofactura");
            entity.Property(e => e.Otronosujetocredito)
                .HasPrecision(16, 2)
                .HasColumnName("otronosujetocredito");
            entity.Property(e => e.Razonsocialemisor)
                .HasMaxLength(500)
                .HasColumnName("razonsocialemisor");
            entity.Property(e => e.Subtotal)
                .HasPrecision(16, 2)
                .HasColumnName("subtotal");
            entity.Property(e => e.Tasas)
                .HasPrecision(16, 2)
                .HasColumnName("tasas");
            entity.Property(e => e.Tipocompra)
                .HasMaxLength(2)
                .HasColumnName("tipocompra");

            entity.HasOne(d => d.CodigopaqueterecepcioncompraNavigation).WithMany(p => p.Registrocompras)
                .HasForeignKey(d => d.Codigopaqueterecepcioncompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("registrocompra_codigopaqueterecepcioncompra_fkey");
        }
    }
}