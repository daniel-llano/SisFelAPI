using SisFelApi.Negocio.DTOs;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Helpers
{
    public class ConversionList
    {
        public List<Usuario> conversionListUsuario(List<Usuario> usuarios)
        {
            List<Usuario> lista = new List<Usuario>();
            foreach (var item in usuarios)
            {
                lista.Add(new Usuario
                {
                    Nombreusuario = item.Nombreusuario,
                    Ci = item.Ci,
                    Nombres = item.Nombres,
                    Ap = item.Ap,
                    Am = item.Am,
                    Telefono = item.Telefono,
                    Codigorol = item.Codigorol,
                    Clave=item.Clave,
                    Activo = item.Activo,
                    CodigorolNavigation = new Rol
                    {
                        Codigorol = item.CodigorolNavigation.Codigorol,
                        Nombrerol = item.CodigorolNavigation.Nombrerol,
                        Descripcion = item.CodigorolNavigation.Descripcion,
                        Activo = item.CodigorolNavigation.Activo
                    }
                }
                );
            }
            return lista;
        }
        public List<Producto> conversionListProducto(List<Producto> productos)
        {
            List<Producto> lista = new List<Producto>();
            foreach (var item in productos)
            {
                lista.Add(new Producto
                {
                    Codigoproducto = item.Codigoproducto,
                    Nombreproducto = item.Nombreproducto,
                    Tipoproducto = item.Tipoproducto,
                    Precio = item.Precio,
                    Codigounidadmedida = item.Codigounidadmedida,
                    Codigocategoria = item.Codigocategoria,
                    Activo=item.Activo,
                    CodigocategoriaNavigation = new Categorium
                    {
                        Codigocategoria = item.CodigocategoriaNavigation.Codigocategoria,
                        Codigoactividad = item.CodigocategoriaNavigation.Codigoactividad,
                        Codigoproductosin = item.CodigocategoriaNavigation.Codigoproductosin,
                        Descripcionproducto = item.CodigocategoriaNavigation.Descripcionproducto,
                        Activo = item.CodigocategoriaNavigation.Activo
                    },
                }
                );
            }
            return lista;
        }
        public List<Telefonocliente> conversionListListTelefonoClientes(List<Telefonocliente> telefonoclientes)
        {
            List<Telefonocliente> lista = new List<Telefonocliente>();
            foreach (var item in telefonoclientes)
            {
                lista.Add(new Telefonocliente
                {
                    Codigotelefonocliente = item.Codigotelefonocliente,
                    Codigocliente = item.Codigocliente,
                    Codigotipodocumentoidentidad = item.Codigotipodocumentoidentidad,
                    Nit = item.Nit,
                    Ci = item.Ci,
                    Complemento = item.Complemento,
                    Razonsocial = item.Razonsocial,
                    Email = item.Email,
                    Telefono = item.Telefono,
                    Activo = item.Activo,
                    CodigoclienteNavigation = new Cliente
                    {
                        Codigocliente = item.CodigoclienteNavigation.Codigocliente,
                        Ci = item.CodigoclienteNavigation.Ci,
                        Datoscliente = item.CodigoclienteNavigation.Datoscliente,
                        Tipopersona = item.CodigoclienteNavigation.Tipopersona,
                        Activo = item.CodigoclienteNavigation.Activo
                    }
                }
                );
            }
            return lista;
        }
        public List<Puntoventum> conversionListPuntoVenta(List<Puntoventum> puntosventa)
        {
            List<Puntoventum> lista = new List<Puntoventum>();
            foreach (var item in puntosventa)
            {
                lista.Add(new Puntoventum
                {
                    Codigopuntoventa = item.Codigopuntoventa,
                    Nombrepuntoventa = item.Nombrepuntoventa,
                    Tipopuntoventa = item.Tipopuntoventa,
                    Codigosucursal = item.Codigosucursal,
                    Activo = item.Activo,
                    CodigosucursalNavigation = new Sucursal
                    {
                        Codigosucursal = item.CodigosucursalNavigation.Codigosucursal,
                        Nombresucursal = item.CodigosucursalNavigation.Nombresucursal,
                        Direccion = item.CodigosucursalNavigation.Direccion,
                        Barrio = item.CodigosucursalNavigation.Barrio,
                        Municipio = item.CodigosucursalNavigation.Municipio,
                        Telefono = item.CodigosucursalNavigation.Telefono,
                        Activo = item.CodigosucursalNavigation.Activo
                    }
                });
            }
            return lista;
        }
        public List<Factura> conversionListFactura(List<Factura> facturas)
        {
            List<Factura> lista = new List<Factura>();
            foreach (var item in facturas)
            {
                lista.Add(new Factura
                {
                    Codigofactura = item.Codigofactura,
                    Codigorecepcion = item.Codigorecepcion,
                    Nitemisor = item.Nitemisor,
                    Municipio = item.Codigorecepcion,
                    Telefonoemisor = item.Telefonoemisor,
                    Nitconjunto = item.Nitconjunto,
                    Numerofactura = item.Numerofactura,
                    Cuf = item.Cuf,
                    Cufd = item.Cufd,
                    Codigosucursal = item.Codigosucursal,
                    Direccion = item.Direccion,
                    Codigopuntoventa = item.Codigopuntoventa,
                    Cafc=item.Cafc,
                    Fechaemision = item.Fechaemision,
                    Nombrerazonsocial = item.Nombrerazonsocial,
                    Codigotipodocumentoidentidad = item.Codigotipodocumentoidentidad,
                    Numerodocumento = item.Numerodocumento,
                    Complemento = item.Complemento,
                    Codigotelefonocliente = item.Codigotelefonocliente,
                    Codigometodopago = item.Codigometodopago,
                    Nrotarjeta = item.Nrotarjeta,
                    Montototal = item.Montototal,
                    Montototalsujetoiva = item.Montototalsujetoiva,
                    Codigomoneda = item.Codigomoneda,
                    Leyenda = item.Leyenda,
                    Usuario = item.Usuario,
                    Codigodocumentosector = item.Codigodocumentosector,
                    EstadoFactura = item.EstadoFactura,
                    Descuentoadicional = item.Descuentoadicional,
                    CodigotelefonoclienteNavigation = new Telefonocliente
                    {
                        Codigotelefonocliente = item.CodigotelefonoclienteNavigation.Codigotelefonocliente,
                        Codigocliente = item.CodigotelefonoclienteNavigation.Codigocliente,
                        Codigotipodocumentoidentidad = item.CodigotelefonoclienteNavigation.Codigotipodocumentoidentidad,
                        Nit = item.CodigotelefonoclienteNavigation.Nit,
                        Ci = item.CodigotelefonoclienteNavigation.Ci,
                        Complemento = item.CodigotelefonoclienteNavigation.Complemento,
                        Razonsocial = item.CodigotelefonoclienteNavigation.Razonsocial,
                        Email = item.CodigotelefonoclienteNavigation.Email,
                        Telefono = item.CodigotelefonoclienteNavigation.Telefono,
                        Activo = item.CodigotelefonoclienteNavigation.Activo
                    },
                    Facturadetalles = conversionListFacturaDetalle(item.Facturadetalles.ToList())
                });
            }
            return lista;
        }

        public List<Facturadetalle> conversionListFacturaDetalle(List<Facturadetalle> facturadetalles)
        {
            List<Facturadetalle> lista = new List<Facturadetalle>();
            foreach (var item in facturadetalles)
            {
                lista.Add(new Facturadetalle
                {
                    Codigofacturadetalle=item.Codigofacturadetalle,
                    Codigofactura=item.Codigofactura,
                    Codigoproducto=item.Codigoproducto,
                    Actividadeconomica=item.Actividadeconomica,
                    Codigoproductosin=item.Codigoproductosin,
                    Descripcion=item.Descripcion,
                    Cantidad=item.Cantidad,
                    Unidadmedida=item.Unidadmedida,
                    Preciounitario=item.Preciounitario,
                    Montodescuento=item.Montodescuento,
                    Subtotal=item.Subtotal,
                    Cuenta=item.Cuenta,
                    Numeroserie=item.Numeroserie,
                    Numeroimei=item.Numeroimei,
                    Codigogrupo=item.Codigogrupo
                });
            }
            return lista;
        }
    }
}
