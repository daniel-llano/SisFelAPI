using SisFelApi.Impuestos.Controllers;
using ServiceCompraVenta;
using Xunit;
using Xunit.Abstractions;

namespace SisFelApiTest;

public class FacturacionCompraVentaControllerTest
{
    private FacturacionCompraVentaController controller;
    private readonly ITestOutputHelper _testOutputHelper;

    public FacturacionCompraVentaControllerTest(ITestOutputHelper testOutputHelper)
    {
        controller = new FacturacionCompraVentaController();
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData(2, 1, 0, 1, 0, 1, 1)]
    [InlineData(2, 1, 0, 1, 1, 1, 1)]

    public async Task TestGetRecepcionFactura(int codAmbiente, int codEmision, int codSucursal, int codModalidad,
    int codPuntoVenta, int tipoFacturaDocumento, int codDocumentoSector)
    {
        for (int i = 0; i < 5000; i++)
        {
            var resp = (respuestaRecepcion)await controller.getRecepcionFactura(new solicitudRecepcionFactura()
            {
                archivo = "No requerido",
                codigoAmbiente = codAmbiente,
                codigoDocumentoSector = codDocumentoSector,
                codigoEmision = codEmision,
                codigoModalidad = codModalidad,
                codigoPuntoVenta = codPuntoVenta,
                codigoPuntoVentaSpecified = true,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                codigoSucursal = codSucursal,
                cufd = "No requerido",
                cuis = "No requerido",
                fechaEnvio = "No requerido",
                hashArchivo = "No requerido",
                nit = 1024061022,
                tipoFacturaDocumento = tipoFacturaDocumento
            }, 1, codPuntoVenta, 2);
        }
    }

    [Theory]
    [InlineData(2, 1, 0, 1, 0, 1, 1)]

    [InlineData(2, 1, 0, 1, 1, 1, 1)]
    public async Task TestGetAnulacionFactura(int codAmbiente, int codEmision, int codSucursal, int codModalidad,
     int codPuntoVenta, int tipoFacturaDocumento, int codDocumentoSector)
    {
        string[] listaCufs = {
            "4611B070DFFFD8727B293B186F88E4EC19E19E06AF8493C4C1B717D74",
            "4611B070DFFFD8727BB3BB5D7F9B0D9817519E06AA8493C4C1B717D74",
            "4611B070DFFFD8727BB955B247F5CDA9A7D79E06AC8493C4C1B717D74"
        };
        for (int i = 0; i < listaCufs.Length; i++)
        {
            var resp = (respuestaRecepcion)await controller.getAnulacionFactura(new solicitudAnulacion()
            {
                codigoAmbiente = codAmbiente,
                codigoDocumentoSector = codDocumentoSector,
                codigoEmision = codEmision,
                codigoModalidad = codModalidad,
                codigoMotivo = 1,
                codigoPuntoVenta = codPuntoVenta,
                codigoPuntoVentaSpecified = true,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                codigoSucursal = codSucursal,
                cufd = "No requerido",
                cuis = "No requerido",
                cuf = listaCufs[i],
                nit = 1024061022,
                tipoFacturaDocumento = tipoFacturaDocumento
            }, codPuntoVenta);

            Thread.Sleep(100);
        }
    }

}