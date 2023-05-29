using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SisFelApi.Negocio.Data;
using SisFelApi.Negocio.DTOs;
using SisFelApi.Negocio.Helpers;
using SisFelApi.Negocio.Helpers.Models;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        public readonly SisfelbdContext _context;
        private readonly IMapper _mapper;

        public RolController(SisfelbdContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("lista")]
        public async Task<ActionResult<List<Rol>>> listaRol(bool? activo)
        {

            ConversionList conversionList = new ConversionList();
            if (activo != null)
            {
                var list = await _context.Rols.Where(x => x.Activo == activo).ToListAsync();
                return Ok(list);
            }
            if (activo == null)
            {
                var list = await _context.Rols.Include(rol => rol.Codigorol).ToListAsync();
                return Ok(list);
            }
            return Ok();
        }

        //Lista de roles con paginacion en gral
        [HttpGet("listaPaginada")]
        public IActionResult listaRolesPaginada([FromQuery] PostQueryFilter filters, bool activo)
        {
            filters.PageNumber = filters.PageNumber == 0 ? 1 : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? 10 : filters.PageSize;

            var objs = _context.Rols.Where(c => c.Activo == activo).AsEnumerable();
            if (filters.filter != null)
            {
                objs = objs.Where(x => 
                x.Codigorol.ToString().ToLower().Contains(filters.filter.ToLower())||
                x.Nombrerol.ToLower().Contains(filters.filter.ToLower())
                ).ToList();
            }

            var pageobjs = PagedList<Rol>.Create(objs, filters.PageNumber, filters.PageSize);
            var metadata = new MetaData
            {
                TotalCount = pageobjs.TotalCount,
                PageSize = pageobjs.PageSize,
                CurrentPage = pageobjs.CurrentPage,
                TotalPages = pageobjs.TotalPages,
                HasNextPage = pageobjs.HasNextPage,
                HasPreviousPage = pageobjs.HasPreviousPage,
            };
            var response = new ApiResponse<IEnumerable<Rol>>(pageobjs)
            {
                Meta = metadata
            };
            return Ok(response);
        }

        [HttpGet("id")]
        public async Task<ActionResult<Rol>> rol(int codigoRol)
        {
            Rol obj = await _context.Rols.FindAsync(codigoRol);
            var objDto = _mapper.Map<RolDto>(obj);
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
        public async Task<ActionResult> agregarRol(RolDto objDto)
        {
            var obj = _mapper.Map<Rol>(objDto);
            _context.Rols.Add(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<RolDto>(obj);
            return Created($"/id?codigoRol={objDto.Codigorol}", objDto);
        }

        [HttpPut("modificar")]
        public async Task<ActionResult> actualizarRol(RolDto objDto)
        {
            var obj = _mapper.Map<Rol>(objDto);
            _context.Rols.Update(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<RolDto>(obj);
            return Created($"/id?codigoRol={objDto.Codigorol}", objDto);
        }

        [HttpPut("cambio de estado")]
        public async Task<ActionResult> cambiarEstadoRol(int Codigorol, bool estado)
        {
            Rol rol = await _context.Rols.FirstOrDefaultAsync(x => x.Codigorol.Equals(Codigorol));

            if (rol == null)
            {
                return NotFound();
            }
            if (estado == true)
            {
                rol.Activo = true;
                _context.Rols.Update(rol);
                await _context.SaveChangesAsync();
                return Ok();
            }
            if (estado == false)
            {
                rol.Activo = false;
                _context.Rols.Update(rol);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return Ok();
        }
    }
}