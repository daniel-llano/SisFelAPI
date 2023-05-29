using SisFelApi.Impuestos.Controllers;
using ServiceSincronizacion;
using Xunit;
using Xunit.Abstractions;
using System.Text;

namespace SisFelApiTest;

public class FacturacionSincronizacionControllerTest
{
    private FacturacionSincronizacionController controller;
    private readonly ITestOutputHelper _testOutputHelper;
    private int timeSleep = 200;
    private int numeroPruebas = 1;
    
    public FacturacionSincronizacionControllerTest(ITestOutputHelper testOutputHelper) {
        controller = new FacturacionSincronizacionController();
        _testOutputHelper = testOutputHelper;
    }   

    [Theory]
    //[InlineData("963E3C34",0,0)] // El otro CUIS
    [InlineData("690ADC2C",0)]
    [InlineData("C2398B6A",1)]
    public async Task TestSincronizarActividades(string cuis, int puntoVenta)
    {
        for (int i = 0; i < numeroPruebas; i++) {
            var result = (respuestaListaActividades)await controller.getSincronizarActividades(new solicitudSincronizacion() {
                cuis = cuis,
                codigoAmbiente = 2,
                codigoPuntoVenta = puntoVenta,
                codigoSucursal = 0,
                codigoPuntoVentaSpecified = true
            }); 
            
            Thread.Sleep(timeSleep);

            StringBuilder salida = new StringBuilder(); 
            for (int j = 0; j < result.listaActividades.Length; j++)
            {
                salida.Append("[");
                salida.Append(
                    result.listaActividades[j].codigoCaeb + "|"
                    + result.listaActividades[j].tipoActividad + "|"
                    + result.listaActividades[j].descripcion  + "]");
            }

            _testOutputHelper.WriteLine(salida.ToString());
        }
    }

    [Theory]
    [InlineData("C2398B6A",1)]
    [InlineData("690ADC2C",0)]
    public async Task TestFechaHora(string cuis, int puntoVenta)
    {
        for (int i = 0; i < numeroPruebas; i++) {
            var result = (respuestaFechaHora)await controller.getSincronizarFechaHora(new solicitudSincronizacion() {
                cuis = cuis,
                codigoAmbiente = 2,
                codigoPuntoVenta = puntoVenta,
                codigoSucursal = 0,
                codigoPuntoVentaSpecified = true
            }); 
            
            Thread.Sleep(timeSleep);

            _testOutputHelper.WriteLine(result.fechaHora);
        }
    }

    [Theory]
    [InlineData("690ADC2C",0)]
    [InlineData("C2398B6A",1)]
    public async Task TestSincronizarListaActividadesDocumentoSector(string cuis, int puntoVenta)
    {
        for (int i = 0; i < numeroPruebas; i++) {
            var result = (respuestaListaActividadesDocumentoSector)await controller.getSincronizarListaActividadesDocumentoSector(new solicitudSincronizacion() {
                cuis = cuis,
                codigoAmbiente = 2,
                codigoPuntoVenta = puntoVenta,
                codigoSucursal = 0,
                codigoPuntoVentaSpecified = true
            }); 
            
            Thread.Sleep(timeSleep);

            StringBuilder salida = new StringBuilder(); 
            for (int j = 0; j < result.listaActividadesDocumentoSector.Length; j++)
            {
                salida.Append("[");
                salida.Append(
                    result.listaActividadesDocumentoSector[j].codigoActividad + "|"
                    + result.listaActividadesDocumentoSector[j].codigoDocumentoSector + "|"
                    + result.listaActividadesDocumentoSector[j].codigoDocumentoSectorSpecified + "|"
                    + result.listaActividadesDocumentoSector[j].tipoDocumentoSector + "]");
            }
            _testOutputHelper.WriteLine(salida.ToString());
        }
    }

    [Theory]
    [InlineData("690ADC2C",0)]
    [InlineData("C2398B6A",1)]
    public async Task TestSincronizarListaLeyendasFactura(string cuis, int puntoVenta)
    {
        for (int i = 0; i < numeroPruebas; i++) {
            var result = (respuestaListaParametricasLeyendas)await controller.getSincronizarListaLeyendasFactura(new solicitudSincronizacion() {
                cuis = cuis,
                codigoAmbiente = 2,
                codigoPuntoVenta = puntoVenta,
                codigoSucursal = 0,
                codigoPuntoVentaSpecified = true
            }); 
            
            Thread.Sleep(timeSleep);

            StringBuilder salida = new StringBuilder(); 
            for (int j = 0; j < result.listaLeyendas .Length; j++)
            {
                salida.Append("[");
                salida.Append(
                    result.listaLeyendas[j].codigoActividad + "|"
                    + result.listaLeyendas[j].descripcionLeyenda + "]");
            }
            _testOutputHelper.WriteLine(salida.ToString());
        }
    }

    private string ConstruirSalidaListaParametricas(respuestaListaParametricas result)
    {
        StringBuilder salida = new StringBuilder(); 
        //salida.Append("[");
        for (int j = 0; j < result.listaCodigos.Length; j++)
        {
            salida.Append("[");
            salida.Append(
                result.listaCodigos[j].codigoClasificador + "|"
                + result.listaCodigos[j].codigoClasificadorSpecified + "|"
                + result.listaCodigos[j].descripcion + "]");
        }
        if (result.mensajesList is not null)
            for (int j = 0; j < result.mensajesList.Length; j++)
            {
                salida.Append("[");
                salida.Append(
                    result.mensajesList[j].codigo + "|"
                    + result.mensajesList[j].codigoSpecified + "|"
                    + result.mensajesList[j].descripcion + "]");
            }
        salida.Append("[" + result.transaccion + "]");
        salida.Append("[" + result.transaccionSpecified + "]");
        
        return salida.ToString();
    }

    [Theory]
    [InlineData("690ADC2C",0)]
    [InlineData("C2398B6A",1)]
    public async Task TestSincronizarListaMensajesServicios(string cuis, int puntoVenta)
    {
        for (int i = 0; i < numeroPruebas; i++) {
            var result = (respuestaListaParametricas)await controller.getSincronizarListaMensajesServicios(new solicitudSincronizacion() {
                cuis = cuis,
                codigoAmbiente = 2,
                codigoPuntoVenta = puntoVenta,
                codigoSucursal = 0,
                codigoPuntoVentaSpecified = true
            }); 
            
            Thread.Sleep(timeSleep);
            _testOutputHelper.WriteLine(ConstruirSalidaListaParametricas(result));
        }
    }

    [Theory]
    [InlineData("690ADC2C",0)]
    [InlineData("C2398B6A",1)]
    public async Task TestSincronizarListaProductosServicios(string cuis, int puntoVenta)
    {
        for (int i = 0; i < numeroPruebas; i++) {
            var result = (respuestaListaProductos)await controller.getSincronizarListaProductosServicios(new solicitudSincronizacion() {
                cuis = cuis,
                codigoAmbiente = 2,
                codigoPuntoVenta = puntoVenta,
                codigoSucursal = 0,
                codigoPuntoVentaSpecified = true
            }); 
            
            Thread.Sleep(timeSleep);

            StringBuilder salida = new StringBuilder(); 
            salida.Append("[");
            for (int j = 0; j < result.listaCodigos.Length; j++)
            {
                salida.Append("[");
                salida.Append(
                    result.listaCodigos[j].codigoActividad + "|"
                    + result.listaCodigos[j].codigoProducto + "|"
                    + result.listaCodigos[j].codigoProductoSpecified + "|"
                    + result.listaCodigos[j].descripcionProducto + "|"
                    + result.listaCodigos[j].nandina  + "]");
            }
            if (result.mensajesList is not null)
                for (int j = 0; j < result.mensajesList.Length; j++)
                {
                    salida.Append("[");
                    salida.Append(
                        result.mensajesList[j].codigo + "|"
                        + result.mensajesList[j].codigoSpecified + "|"
                        + result.mensajesList[j].descripcion + "]");
                }
            salida.Append("[" + result.transaccion + "]");
            salida.Append("[" + result.transaccionSpecified + "]");
            
            _testOutputHelper.WriteLine(salida.ToString());
        }
    }

    [Theory]
    [InlineData("690ADC2C",0)]
    [InlineData("C2398B6A",1)]
    public async Task TestSincronizarParametricaEventosSignificativos(string cuis, int puntoVenta)
    {
        for (int i = 0; i < numeroPruebas; i++) {
            var result = (respuestaListaParametricas)await controller.getSincronizarParametricaEventosSignificativos(new solicitudSincronizacion() {
                cuis = cuis,
                codigoAmbiente = 2,
                codigoPuntoVenta = puntoVenta,
                codigoSucursal = 0,
                codigoPuntoVentaSpecified = true
            }); 
            
            Thread.Sleep(timeSleep);
            _testOutputHelper.WriteLine(ConstruirSalidaListaParametricas(result));
        }
    }

    [Theory]
    [InlineData("690ADC2C",0)]
    [InlineData("C2398B6A",1)]
    public async Task TestSincronizarParametricaMotivoAnulacion(string cuis, int puntoVenta)
    {
        for (int i = 0; i < numeroPruebas; i++) {
            var result = (respuestaListaParametricas)await controller.getSincronizarParametricaMotivoAnulacion(new solicitudSincronizacion() {
                cuis = cuis,
                codigoAmbiente = 2,
                codigoPuntoVenta = puntoVenta,
                codigoSucursal = 0,
                codigoPuntoVentaSpecified = true
            }); 
            
            Thread.Sleep(timeSleep);
            _testOutputHelper.WriteLine(ConstruirSalidaListaParametricas(result));
        }
    }

    [Theory]
    [InlineData("690ADC2C",0)]
    [InlineData("C2398B6A",1)]
    public async Task TestSincronizarParametricaPaisOrigen(string cuis, int puntoVenta)
    {
        for (int i = 0; i < numeroPruebas; i++) {
            var result = (respuestaListaParametricas)await controller.getSincronizarParametricaPaisOrigen(new solicitudSincronizacion() {
                cuis = cuis,
                codigoAmbiente = 2,
                codigoPuntoVenta = puntoVenta,
                codigoSucursal = 0,
                codigoPuntoVentaSpecified = true
            }); 
            
            Thread.Sleep(timeSleep);
            _testOutputHelper.WriteLine(ConstruirSalidaListaParametricas(result));
        }
    }

    [Theory]
    [InlineData("690ADC2C",0)]
    [InlineData("C2398B6A",1)]
    public async Task TestSincronizarParametricaTipoDocumentoIdentidad(string cuis, int puntoVenta)
    {
        for (int i = 0; i < numeroPruebas; i++) {
            var result = (respuestaListaParametricas)await controller.getSincronizarParametricaTipoDocumentoIdentidad(new solicitudSincronizacion() {
                cuis = cuis,
                codigoAmbiente = 2,
                codigoPuntoVenta = puntoVenta,
                codigoSucursal = 0,
                codigoPuntoVentaSpecified = true
            }); 
            
            Thread.Sleep(timeSleep);
            _testOutputHelper.WriteLine(ConstruirSalidaListaParametricas(result));
        }
    }

    [Theory]
    [InlineData("690ADC2C",0)]
    [InlineData("C2398B6A",1)]
    public async Task TestSincronizarParametricaTipoDocumentoSector(string cuis, int puntoVenta)
    {
        for (int i = 0; i < numeroPruebas; i++) {
            var result = (respuestaListaParametricas)await controller.getSincronizarParametricaTipoDocumentoSector(new solicitudSincronizacion() {
                cuis = cuis,
                codigoAmbiente = 2,
                codigoPuntoVenta = puntoVenta,
                codigoSucursal = 0,
                codigoPuntoVentaSpecified = true
            }); 
            
            Thread.Sleep(timeSleep);
            _testOutputHelper.WriteLine(ConstruirSalidaListaParametricas(result));
        }
    }

    [Theory]
    [InlineData("690ADC2C",0)]
    [InlineData("C2398B6A",1)]
    public async Task TestSincronizarParametricaTipoEmision(string cuis, int puntoVenta)
    {
        for (int i = 0; i < numeroPruebas; i++) {
            var result = (respuestaListaParametricas)await controller.getSincronizarParametricaTipoEmision(new solicitudSincronizacion() {
                cuis = cuis,
                codigoAmbiente = 2,
                codigoPuntoVenta = puntoVenta,
                codigoSucursal = 0,
                codigoPuntoVentaSpecified = true
            }); 
            
            Thread.Sleep(timeSleep);
            _testOutputHelper.WriteLine(ConstruirSalidaListaParametricas(result));
        }
    }

    [Theory]
    [InlineData("690ADC2C",0)]
    [InlineData("C2398B6A",1)]
    public async Task TestSincronizarParametricaTipoHabitacion(string cuis, int puntoVenta)
    {
        for (int i = 0; i < numeroPruebas; i++) {
            var result = (respuestaListaParametricas)await controller.getSincronizarParametricaTipoHabitacion(new solicitudSincronizacion() {
                cuis = cuis,
                codigoAmbiente = 2,
                codigoPuntoVenta = puntoVenta,
                codigoSucursal = 0,
                codigoPuntoVentaSpecified = true
            }); 
            
            Thread.Sleep(timeSleep);
            _testOutputHelper.WriteLine(ConstruirSalidaListaParametricas(result));
        }
    }

    [Theory]
    [InlineData("690ADC2C",0)]
    [InlineData("C2398B6A",1)]
    public async Task TestSincronizarParametricaTipoMetodoPago(string cuis, int puntoVenta)
    {
        for (int i = 0; i < numeroPruebas; i++) {
            var result = (respuestaListaParametricas)await controller.getSincronizarParametricaTipoMetodoPago(new solicitudSincronizacion() {
                cuis = cuis,
                codigoAmbiente = 2,
                codigoPuntoVenta = puntoVenta,
                codigoSucursal = 0,
                codigoPuntoVentaSpecified = true
            }); 
            
            Thread.Sleep(timeSleep);
            _testOutputHelper.WriteLine(ConstruirSalidaListaParametricas(result));
        }
    }

    [Theory]
    [InlineData("690ADC2C",0)]
    [InlineData("C2398B6A",1)]
    public async Task TestSincronizarParametricaTipoMoneda(string cuis, int puntoVenta)
    {
        for (int i = 0; i < numeroPruebas; i++) {
            var result = (respuestaListaParametricas)await controller.getSincronizarParametricaTipoMoneda(new solicitudSincronizacion() {
                cuis = cuis,
                codigoAmbiente = 2,
                codigoPuntoVenta = puntoVenta,
                codigoSucursal = 0,
                codigoPuntoVentaSpecified = true
            }); 
            
            Thread.Sleep(timeSleep);
            _testOutputHelper.WriteLine(ConstruirSalidaListaParametricas(result));
        }
    }

    [Theory]
    [InlineData("690ADC2C",0)]
    [InlineData("C2398B6A",1)]
    public async Task TestSincronizarParametricaTipoPuntoVenta(string cuis, int puntoVenta)
    {
        for (int i = 0; i < numeroPruebas; i++) {
            var result = (respuestaListaParametricas)await controller.getSincronizarParametricaTipoPuntoVenta(new solicitudSincronizacion() {
                cuis = cuis,
                codigoAmbiente = 2,
                codigoPuntoVenta = puntoVenta,
                codigoSucursal = 0,
                codigoPuntoVentaSpecified = true
            }); 
            
            Thread.Sleep(timeSleep);
            _testOutputHelper.WriteLine(ConstruirSalidaListaParametricas(result));
        }
    }

    [Theory]
    [InlineData("690ADC2C",0)]
    [InlineData("C2398B6A",1)]
    public async Task TestSincronizarParametricaTiposFactura(string cuis, int puntoVenta)
    {
        for (int i = 0; i < numeroPruebas; i++) {
            var result = (respuestaListaParametricas)await controller.getSincronizarParametricaTiposFactura(new solicitudSincronizacion() {
                cuis = cuis,
                codigoAmbiente = 2,
                codigoPuntoVenta = puntoVenta,
                codigoSucursal = 0,
                codigoPuntoVentaSpecified = true
            }); 
            
            Thread.Sleep(timeSleep);
            _testOutputHelper.WriteLine(ConstruirSalidaListaParametricas(result));
        }
    }

    [Theory]
    [InlineData("690ADC2C",0)]
    [InlineData("C2398B6A",1)]
    public async Task TestSincronizarParametricaUnidadMedida(string cuis, int puntoVenta)
    {
        for (int i = 0; i < numeroPruebas; i++) {
            var result = (respuestaListaParametricas)await controller.getSincronizarParametricaUnidadMedida(new solicitudSincronizacion() {
                cuis = cuis,
                codigoAmbiente = 2,
                codigoPuntoVenta = puntoVenta,
                codigoSucursal = 0,
                codigoPuntoVentaSpecified = true
            }); 
            
            Thread.Sleep(timeSleep);
            _testOutputHelper.WriteLine(ConstruirSalidaListaParametricas(result));
        }
    }
}