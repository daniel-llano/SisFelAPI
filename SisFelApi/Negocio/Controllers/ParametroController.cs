using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SisFelApi.Negocio.Data;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ParametroController : ControllerBase
{

    public readonly SisfelbdContext _context;

    public ParametroController(SisfelbdContext context)
    {
        _context = context;
    }

    [HttpGet("listaUnidadesMedida")]
    public List<Parametro> getListaUnidadesMedida() 
    {
        var unidadesMedida = from b in _context.Parametros
                   where b.Nombregrupo == "UNIDADES"
                   select b;
        return unidadesMedida.ToList();
    }

    [HttpGet("listaMotivoEvento")]
    public List<Parametro> getListaMotivoEvento() 
    {
        var motivosEvento = from b in _context.Parametros
                   where b.Nombregrupo == "EVENTOS"
                   select b;
        return motivosEvento.ToList();
    }

    [HttpGet("listaMotivosAnulacion")]
    public List<Parametro> getListaMotivosAnulacion() 
    {
        var motivos = from b in _context.Parametros
                   where b.Nombregrupo == "MOTIVOS"
                   select b;
        return motivos.ToList();
    }
}

