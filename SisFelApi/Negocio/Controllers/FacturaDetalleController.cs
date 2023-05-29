using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SisFelApi.Negocio.Data;
using SisFelApi.Negocio.DTOs;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaDetalleController : ControllerBase
    {
        public readonly SisfelbdContext _context;
        private readonly IMapper _mapper;
        public FacturaDetalleController(SisfelbdContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("lista")]
        public async Task<ActionResult<List<Facturadetalle>>> listaFacturaDetalles()
        {
            return Ok(await _context.Facturadetalles.ToListAsync());
        }
        [HttpGet("id")]
        public async Task<ActionResult<Facturadetalle>> facturaDetalle(int codigoFacturaDetalle)
        {
            Facturadetalle obj = await _context.Facturadetalles.FindAsync(codigoFacturaDetalle);
            var objDto = _mapper.Map<FacturaDetalleDto>(obj);
            if (objDto == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(objDto);
            }
        }

        [HttpPost("agregar")]
        public async Task<ActionResult> agregarFacturaDetalle(FacturaDetalleDto objDto)
        {

            var obj = _mapper.Map<Facturadetalle>(objDto);

            if(objDto.Codigofacturadetalle != 0){
                _context.Facturadetalles.Update(obj);
            }else{
                _context.Facturadetalles.Add(obj);
            }

            
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<FacturaDetalleDto>(obj);
            return Created($"/id?codigoFacturaDetalle={objDto.Codigofacturadetalle}", objDto);
        }
        [HttpPut("modificar")]
        public async Task<ActionResult> actualizarFacturaDetalle(FacturaDetalleDto objDto)
        {
            var obj = _mapper.Map<Facturadetalle>(objDto);
            _context.Facturadetalles.Update(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<FacturaDetalleDto>(obj);
            return Created($"/id?codigoFacturaDetalle={objDto.Codigofacturadetalle}", objDto);
        }
    }
}
