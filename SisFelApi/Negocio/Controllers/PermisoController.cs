using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SisFelApi.Negocio.Data;
using SisFelApi.Negocio.DTOs;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisoController : ControllerBase
    {
        public readonly SisfelbdContext _context;
        private readonly IMapper _mapper;

        public PermisoController(SisfelbdContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("lista")]
        public async Task<ActionResult<List<Permiso>>> listaPermisos()
        {
            return Ok(await _context.Permisos.Where(p => p.Activo.Equals(true)).ToListAsync());
        }

        [HttpGet("id")]
        public async Task<ActionResult<Permiso>> permiso(int codigoRol)
        {
            List<Permiso>  obj = await _context.Permisos.Where(p => p.Codigorol==codigoRol).ToListAsync();
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(obj);
            }
        }

        [HttpPost("agregar")]
        public async Task<ActionResult> agregarPermiso(PermisoDto objDto)
        {
            var obj = _mapper.Map<Permiso>(objDto);
            _context.Permisos.Add(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<PermisoDto>(obj);
            return Created($"/id?codigoRol={objDto.Codigorol}", objDto);
        }

        [HttpPut("modificar")]
        public async Task<ActionResult> actualizarPermiso(PermisoDto objDto)
        {
            var obj = _mapper.Map<Permiso>(objDto);
            _context.Permisos.Update(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<PermisoDto>(obj);
            return Created($"/id?codigoRol={objDto.Codigorol}", objDto);
        }

        [HttpDelete("eliminar")]
        public async Task<ActionResult> eliminarPermiso(int Codigorol)
        {
            List<Permiso> obj = await _context.Permisos.Where(p => p.Codigorol == Codigorol).ToListAsync();

           foreach (var objDto in obj)
            {
                _context.Permisos.Remove(objDto);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}