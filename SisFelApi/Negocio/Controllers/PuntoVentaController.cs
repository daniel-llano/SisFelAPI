using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SisFelApi.Negocio.Data;
using SisFelApi.Negocio.DTOs;
using SisFelApi.Negocio.Helpers;
using SisFelApi.Negocio.Helpers.Models;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PuntoVentaController : ControllerBase
    {
        public readonly SisfelbdContext _context;
        private readonly IMapper _mapper;
        public PuntoVentaController(SisfelbdContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("lista")]
        public async Task<ActionResult<List<Puntoventum>>> listaPuntosVenta()
        {
            return Ok(await _context.Puntoventa.ToListAsync());
        }

        //Lista de puntos de venta con paginacion en gral
        [HttpGet("listaPaginada")]
        public IActionResult listaPuntoVentaPaginada([FromQuery] PostQueryFilter filters, bool activo)
        {
            filters.PageNumber = filters.PageNumber == 0 ? 1 : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? 10 : filters.PageSize;

            var objs = _context.Puntoventa.Where(c => c.Activo == activo).AsEnumerable();
            if (filters.filter != null)
            {
                objs = objs.Where(x => 
                x.Codigopuntoventa.ToString().ToLower().Contains(filters.filter.ToLower())||
                x.Nombrepuntoventa.ToLower().Contains(filters.filter.ToLower())||
                x.Tipopuntoventa.ToLower().Contains(filters.filter.ToLower())||
                x.Codigosucursal.ToString().ToLower().Contains(filters.filter.ToLower())
                ).ToList();
            }

            var pageobjs = PagedList<Puntoventum>.Create(objs, filters.PageNumber, filters.PageSize);
            var metadata = new MetaData
            {
                TotalCount = pageobjs.TotalCount,
                PageSize = pageobjs.PageSize,
                CurrentPage = pageobjs.CurrentPage,
                TotalPages = pageobjs.TotalPages,
                HasNextPage = pageobjs.HasNextPage,
                HasPreviousPage = pageobjs.HasPreviousPage,
            };
            var response = new ApiResponse<IEnumerable<Puntoventum>>(pageobjs)
            {
                Meta = metadata
            };
            return Ok(response);
        }

        [HttpGet("id")]
        public async Task<ActionResult<Puntoventum>> puntoVenta(int codigoPuntoVenta)
        {
            Puntoventum obj = await _context.Puntoventa.FindAsync(codigoPuntoVenta);
            var objDto = _mapper.Map<PuntoVentaDto>(obj);
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
        public async Task<ActionResult> agregarPuntoVenta(PuntoVentaDto objDto)
        {
            var obj = _mapper.Map<Puntoventum>(objDto);
            _context.Puntoventa.Add(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<PuntoVentaDto>(obj);
            return Created($"/id?codigoPuntoVenta={objDto.Codigopuntoventa}", objDto);
        }
        [HttpPut("modificar")]
        public async Task<ActionResult> actualizarPuntoVenta(PuntoVentaDto objDto)
        {
            var obj = _mapper.Map<Puntoventum>(objDto);
            _context.Puntoventa.Update(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<PuntoVentaDto>(obj);
            return Created($"/id?codigoPuntoVenta={objDto.Codigopuntoventa}", objDto);
        }

        [HttpPut("cambio de estado")]
        public async Task<ActionResult> estadoPuntoVenta(int CodigoPuntoVenta, bool estado)
        {
            Puntoventum obj = await _context.Puntoventa.FindAsync(CodigoPuntoVenta);

            if (obj == null)
            {
                return NotFound();
            }
            if (estado == true)
            {
                obj.Activo = true;
                _context.Puntoventa.Update(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            if (estado == false)
            {
                obj.Activo = false;
                _context.Puntoventa.Update(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return Ok();
        }
    }
}
