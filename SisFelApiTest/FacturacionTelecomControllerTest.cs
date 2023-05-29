using SisFelApi.Impuestos.Controllers;
using ServiceTelecom;
using SisFelApi.Negocio.Controllers;
using SisFelApi.Negocio.Data;
using Xunit;
using Xunit.Abstractions;
using AutoMapper;

namespace SisFelApiTest;

public class FacturacionTelecomControllerTest
{
    private FacturacionTelecomController controller;
    private readonly ITestOutputHelper _testOutputHelper;
    public static SisfelbdContext context = new SisfelbdContext();
    public static readonly IMapper _mapper;
    private EventoController eventoController = new EventoController(context, _mapper);

    public FacturacionTelecomControllerTest(ITestOutputHelper testOutputHelper)
    {
        controller = new FacturacionTelecomController();
        _testOutputHelper = testOutputHelper;
    }

    [Theory]

    [InlineData(2, 1, 0, 1, 0, 1, 22)]
    [InlineData(2, 1, 0, 1, 1, 1, 22)]

    public async Task TestGetRecepcionFactura(int codAmbiente, int codEmision, int codSucursal, int codModalidad,
    int codPuntoVenta, int tipoFacturaDocumento, int codDocumentoSector)
    {
        for (int i = 0; i < 0; i++)
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
            }, codPuntoVenta, 2, 1);
        }
    }

    [Theory]
    [InlineData(2, 1, 0, 1, 0, 1, 22)]
    [InlineData(2, 1, 0, 1, 1, 1, 22)]
    public async Task TestGetAnulacionFactura(int codAmbiente, int codEmision, int codSucursal, int codModalidad,
    int codPuntoVenta, int tipoFacturaDocumento, int codDocumentoSector)
    {
        string[] listaCufs = { };
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