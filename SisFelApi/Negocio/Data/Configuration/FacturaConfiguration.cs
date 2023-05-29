using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Data.Configuration
{
    public class FacturaConfiguration : IEntityTypeConfiguration<Factura>
    {
        public void Configure(EntityTypeBuilder<Factura> entity)
        {
            entity.HasKey(e => e.Codigofactura).HasName("factura_pkey");

            entity.ToTable("factura");

            entity.Property(e => e.Codigofactura).HasColumnName("codigofactura");
            entity.Property(e => e.Cafc)
                .HasMaxLength(50)
                .HasColumnName("cafc");
            entity.Property(e => e.Codigodocumentosector).HasColumnName("codigodocumentosector");
            entity.Property(e => e.Codigometodopago).HasColumnName("codigometodopago");
            entity.Property(e => e.Codigomoneda).HasColumnName("codigomoneda");
            entity.Property(e => e.Codigopuntoventa).HasColumnName("codigopuntoventa");
            entity.Property(e => e.Codigorecepcion)
                .HasMaxLength(100)
                .HasColumnName("codigorecepcion");
            entity.Property(e => e.Codigosucursal).HasColumnName("codigosucursal");
            entity.Property(e => e.Codigotelefonocliente).HasColumnName("codigotelefonocliente");
            entity.Property(e => e.Codigotipodocumentoidentidad).HasColumnName("codigotipodocumentoidentidad");
            entity.Property(e => e.Complemento)
                .HasMaxLength(5)
                .HasColumnName("complemento");
            entity.Property(e => e.Cuf)
                .HasMaxLength(100)
                .HasColumnName("cuf");
            entity.Property(e => e.Cufd)
                .HasMaxLength(100)
                .HasColumnName("cufd");
            entity.Property(e => e.Descuentoadicional)
                .HasPrecision(19, 2)
                .HasColumnName("descuentoadicional");
            entity.Property(e => e.Direccion)
                .HasMaxLength(500)
                .HasColumnName("direccion");
            entity.Property(e => e.EstadoFactura)
                .HasMaxLength(10)
                .HasColumnName("estado_factura");
            entity.Property(e => e.Fechaemision)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fechaemision");
            entity.Property(e => e.Leyenda)
                .HasMaxLength(200)
                .HasColumnName("leyenda");
            entity.Property(e => e.Montototal)
                .HasPrecision(19, 2)
                .HasColumnName("montototal");
            entity.Property(e => e.Montototalsujetoiva)
                .HasPrecision(19, 2)
                .HasColumnName("montototalsujetoiva");
            entity.Property(e => e.Municipio)
                .HasMaxLength(25)
                .HasColumnName("municipio");
            entity.Property(e => e.Nitconjunto).HasColumnName("nitconjunto");
            entity.Property(e => e.Nitemisor).HasColumnName("nitemisor");
            entity.Property(e => e.Nombrerazonsocial)
                .HasMaxLength(500)
                .HasColumnName("nombrerazonsocial");
            entity.Property(e => e.Nrotarjeta).HasColumnName("nrotarjeta");
            entity.Property(e => e.Numerodocumento)
                .HasMaxLength(20)
                .HasColumnName("numerodocumento");
            entity.Property(e => e.Numerofactura).HasColumnName("numerofactura");
            entity.Property(e => e.Telefonoemisor).HasColumnName("telefonoemisor");
            entity.Property(e => e.Usuario)
                .HasMaxLength(10)
                .HasColumnName("usuario");

            entity.HasOne(d => d.CodigopuntoventaNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.Codigopuntoventa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("factura_codigopuntoventa_fkey");

            entity.HasOne(d => d.CodigosucursalNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.Codigosucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("factura_codigosucursal_fkey");

            entity.HasOne(d => d.CodigotelefonoclienteNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.Codigotelefonocliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("factura_codigotelefonocliente_fkey");

            entity.HasOne(d => d.UsuarioNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.Usuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("factura_usuario_fkey");
        }
    }
}
