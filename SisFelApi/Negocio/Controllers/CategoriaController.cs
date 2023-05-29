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
    public class CategoriaController : ControllerBase
    {
        public readonly SisfelbdContext _context;
        private readonly IMapper _mapper;
        public CategoriaController(SisfelbdContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //trae la lista de categorias en gral.
        [HttpGet("lista")]
        public async Task<ActionResult<List<Categorium>>> listaCategorias()
        {
            return Ok(await _context.Categoria.ToListAsync());
        }
        //trae una categoria en específica mandando el codigo de la misma
        [HttpGet("id")]
        public async Task<ActionResult<Categorium>> categoria(int codigoCategoria)
        {
            Categorium obj = await _context.Categoria.FindAsync(codigoCategoria);
            var objDto = _mapper.Map<CategoriaDto>(obj);
            if (objDto == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(objDto);
            }
        }
        //agrega una nueva categoria mandando el objeto correspondiente de tipo CategoriaDto
        [HttpPost("agregar")]
        public async Task<ActionResult> agregarCategoria(CategoriaDto objDto)
        {
            var obj = _mapper.Map<Categorium>(objDto);
            _context.Categoria.Add(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<CategoriaDto>(obj);
            return Created($"/id?codigoCategoria={objDto.Codigocategoria}", objDto);
        }
        //modifica una nueva categoria mandando el objeto correspondiente de tipo CategoriaDto
        [HttpPut("modificar")]
        public async Task<ActionResult> actualizarCategoria(CategoriaDto objDto)
        {
            var obj = _mapper.Map<Categorium>(objDto);
            _context.Categoria.Update(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<CategoriaDto>(obj);
            return Created($"/id?codigoCategoria={objDto.Codigocategoria}", objDto);
        }
        //realiza el cambio de estado segun mandando el codigo de la misma y el tipo de estado a cambiar
        [HttpPut("cambio de estado")]
        public async Task<ActionResult> estadoCategoria(int CodigoCategoria, bool estado)
        {
            Categorium obj = await _context.Categoria.FindAsync(CodigoCategoria);

            if (obj == null)
            {
                return NotFound();
            }
            if (estado == true)
            {
                obj.Activo = true;
                _context.Categoria.Update(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            if (estado == false)
            {
                obj.Activo = false;
                _context.Categoria.Update(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return Ok();
        }
    }
}
