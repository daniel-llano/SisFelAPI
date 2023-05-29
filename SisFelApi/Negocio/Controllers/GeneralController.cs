using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SisFelApi.Negocio.Data;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GeneralController : ControllerBase
{

    public readonly SisfelbdContext _context;

    public GeneralController(SisfelbdContext context)
    {
        _context = context;
    }

    [HttpGet("lista")]
    public async Task<ActionResult<List<General>>> getListaGenerales() 
    {
        return Ok(await _context.Generals.ToListAsync());        
    }

    [HttpGet("id")]
    public async Task<General> getGeneralById(int codigoPuntoVenta)
    {
        General general = await _context.Generals.FindAsync(codigoPuntoVenta);
        return general;
    }

    [HttpPost("agregar")]
    public async Task<ActionResult> agregarGeneral(General general)
    {
        _context.Generals.Add(general);
        await _context.SaveChangesAsync();
        return Created($"/id?codigoPuntoVenta={general.Codigopuntoventa}",general);
    }

    [HttpPut("modificar")]
    public async Task<ActionResult> actualizarGeneral(General general)
    {
        General generalBd = await _context.Generals.FindAsync(general.Codigopuntoventa);
        
        generalBd.Ciudad = general.Ciudad;
        generalBd.Codigoautorizacion = general.Codigoautorizacion;
        generalBd.Codigocontrol = general.Codigocontrol;
        generalBd.Codigosistema = general.Codigosistema;
        generalBd.Cufd = general.Cufd;
        generalBd.Cuis = general.Cuis;
        generalBd.Direccion = general.Direccion;
        generalBd.Fechavigenciacufd = general.Fechavigenciacufd;
        generalBd.Fechavigenciacuis = general.Fechavigenciacuis;
        generalBd.Nit = general.Nit;
        generalBd.Nombreempresa = general.Nombreempresa;
        generalBd.Telefono = general.Telefono;

        _context.Generals.Update(general);
        await _context.SaveChangesAsync();
        return Created($"/id?codigoPuntoVenta={general.Codigopuntoventa}",general);
    }

    [HttpDelete("eliminar")]
    public async Task<ActionResult> eliminarGeneral(int codigoPuntoVenta)
    {
        General general = await _context.Generals.FindAsync(codigoPuntoVenta);

        if(general == null)
        {
            return NotFound();
        }

        _context.Generals.Remove(general);
        await _context.SaveChangesAsync();
        return Ok();
    }


    [HttpGet("datosCertificado")]
    public async Task<ActionResult<Helper.RespuestaCertificado>> getDatosCertificado() 
    {
        return Ok(await Helper.obtenerDatosCertificado());        
    }

}

