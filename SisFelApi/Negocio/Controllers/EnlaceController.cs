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
    public class EnlaceController : ControllerBase
    {
        public readonly SisfelbdContext _context;
        private readonly IMapper _mapper;
        public EnlaceController(SisfelbdContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("lista")]
        public async Task<ActionResult<List<Enlace>>> listaEnlaces()
        {
            return Ok(await _context.Enlaces.Where(p => p.Activo.Equals(true)).ToListAsync());
        }
        [HttpGet("id")]
        public async Task<ActionResult<Enlace>> enlace(int codigoEnlace)
        {
            Enlace obj = await _context.Enlaces.FindAsync(codigoEnlace);
            var objDto = _mapper.Map<EnlaceDto>(obj);
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
        public async Task<ActionResult> agregarEnlace(EnlaceDto objDto)
        {
            var obj = _mapper.Map<Enlace>(objDto);
            _context.Enlaces.Add(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<EnlaceDto>(obj);
            return Created($"/id?codigoEnlace={objDto.Codigoenlace}", objDto);
        }
        [HttpPut("modificar")]
        public async Task<ActionResult> actualizarEnlace(EnlaceDto objDto)
        {
            var obj = _mapper.Map<Enlace>(objDto);
            _context.Enlaces.Update(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<EnlaceDto>(obj);
            return Created($"/id?codigoEnlace={objDto.Codigoenlace}", objDto);
        }

        [HttpDelete("eliminar")]
        public async Task<ActionResult> eliminarEnlace(int CodigoEnlace, bool estado)
        {
            Enlace obj = await _context.Enlaces.FindAsync(CodigoEnlace);

            if (obj == null)
            {
                return NotFound();
            }

            if (estado == true)
            {
                obj.Activo = true;
                _context.Enlaces.Update(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            if (estado == false)
            {
                obj.Activo = false;
                _context.Enlaces.Update(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return Ok();
        }
    }
}