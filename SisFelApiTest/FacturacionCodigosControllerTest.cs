using SisFelApi.Impuestos.Controllers;
using ServiceCodigos;
using Xunit;
using Xunit.Abstractions;

namespace SisFelApiTest;

public class FacturacionCodigosControllerTest
{
    private FacturacionCodigosController controller;
    private readonly ITestOutputHelper _testOutputHelper;
    public List<string> listacufd {get; set;}
    public List<string> listaCuis {get; set;} 
    
    public FacturacionCodigosControllerTest(ITestOutputHelper testOutputHelper) {
        controller = new FacturacionCodigosController();
        listaCuis = new List<string>();
        _testOutputHelper = testOutputHelper;
        listacufd = new List<string>();
    }   
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public async Task TestGetCuis(int puntoVenta)
    {
        //Arrange 

        //Act
        var result= (respuestaCuis)await controller.getCuis(new solicitudCuis() {
            codigoAmbiente = 2,
            codigoPuntoVenta = puntoVenta,
            codigoSucursal = 0,
            codigoModalidad = 1,
            codigoPuntoVentaSpecified=true
        });

        //Assert
        Assert.NotNull(result);
        listaCuis.Add(result.codigo);

        var message = "Cuis: " + listaCuis[0];
        _testOutputHelper.WriteLine(message);
    }

    [Theory]
    [InlineData("C2398B6A",1)]
    [InlineData("690ADC2C",0)]
    public async Task TestGetCufd(string cuis, int puntoVenta)
    {
            for (int i = 0; i < 100; i++) {
                var result = (respuestaCufd)await controller.getCufd(new solicitudCufd() {
                    cuis = cuis,
                    codigoAmbiente = 2,
                    codigoPuntoVenta = puntoVenta,
                    codigoSucursal = 0,
                    codigoModalidad = 1,
                    codigoPuntoVentaSpecified = true
                }); 
                
                Assert.NotNull(result);
                Thread.Sleep(10);
                listacufd.Add(result.codigo);
            }
            var message1 = "Cufd: " + listacufd[0];
            _testOutputHelper.WriteLine(message1);
    }
}