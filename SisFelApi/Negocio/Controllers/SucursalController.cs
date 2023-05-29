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
    public class SucursalController : ControllerBase
    {
        public readonly SisfelbdContext _context;
        private readonly IMapper _mapper;
        public SucursalController(SisfelbdContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("lista")]
        public async Task<ActionResult<List<Sucursal>>> listaSucursales()
        {
            return Ok(await _context.Sucursals.ToListAsync());
        }

        //Lista de sucursales con paginacion en gral
        [HttpGet("listaPaginada")]
        public IActionResult listaSucursalPaginada([FromQuery] PostQueryFilter filters, bool activo)
        {
            filters.PageNumber = filters.PageNumber == 0 ? 1 : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? 10 : filters.PageSize;

            var objs = _context.Sucursals.Where(c => c.Activo == activo).AsEnumerable();
            if (filters.filter != null)
            {
                objs = objs.Where(x => 
                x.Codigosucursal.ToString().ToLower().Contains(filters.filter.ToLower())||
                x.Nombresucursal.ToLower().Contains(filters.filter.ToLower())||
                x.Municipio.ToLower().Contains(filters.filter.ToLower())
                ).ToList();
            }

            var pageobjs = PagedList<Sucursal>.Create(objs, filters.PageNumber, filters.PageSize);
            var metadata = new MetaData
            {
                TotalCount = pageobjs.TotalCount,
                PageSize = pageobjs.PageSize,
                CurrentPage = pageobjs.CurrentPage,
                TotalPages = pageobjs.TotalPages,
                HasNextPage = pageobjs.HasNextPage,
                HasPreviousPage = pageobjs.HasPreviousPage,
            };
            var response = new ApiResponse<IEnumerable<Sucursal>>(pageobjs)
            {
                Meta = metadata
            };
            return Ok(response);
        }

        [HttpGet("id")]
        public async Task<ActionResult<Sucursal>> sucursal(int codigoSucursal)
        {
            Sucursal obj = await _context.Sucursals.FindAsync(codigoSucursal);
            var objDto = _mapper.Map<SucursalDto>(obj);
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
        public async Task<ActionResult> agregarSucursal(SucursalDto objDto)
        {
            var obj = _mapper.Map<Sucursal>(objDto);
            _context.Sucursals.Add(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<SucursalDto>(obj);
            return Created($"/id?codigoSucursal={objDto.Codigosucursal}", objDto);
        }
        [HttpPut("modificar")]
        public async Task<ActionResult> actualizarSucursal(SucursalDto objDto)
        {
            var obj = _mapper.Map<Sucursal>(objDto);
            _context.Sucursals.Update(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<SucursalDto>(obj);
            return Created($"/id?codigoSucursal={objDto.Codigosucursal}", objDto);
        }

        [HttpPut("cambio de estado")]
        public async Task<ActionResult> estadoSucursal(int CodigoSucursal, bool estado)
        {
            Sucursal obj = await _context.Sucursals.FindAsync(CodigoSucursal);

            if (obj == null)
            {
                return NotFound();
            }
            if (estado == true)
            {
                obj.Activo = true;
                _context.Sucursals.Update(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            if (estado == false)
            {
                obj.Activo = false;
                _context.Sucursals.Update(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return Ok();
        }
    }
}
