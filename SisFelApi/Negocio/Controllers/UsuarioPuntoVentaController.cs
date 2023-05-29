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
    public class UsuarioPuntoVentaController : ControllerBase
    {
        public readonly SisfelbdContext _context;
        private readonly IMapper _mapper;

        public UsuarioPuntoVentaController(SisfelbdContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("lista")]
        public async Task<ActionResult<List<Puntoventum>>> listaPuntosVentaUsuario(string nombreUsuario)
        {
            var puntosVenta = from b in _context.Usuariopuntoventa
                   where b.Nombreusuario == nombreUsuario
                   select b.CodigopuntoventaNavigation;
            return puntosVenta.ToList();
        }

        [HttpGet("id")]
        public async Task<ActionResult<Permiso>> UsuarioPuntoVenta(string nombreUsuario, int codigoPuntoVenta)
        {
            Usuariopuntoventum obj = _context.Usuariopuntoventa.FirstOrDefault(p => p.Nombreusuario == nombreUsuario && p.Codigopuntoventa == codigoPuntoVenta); 
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
        public async Task<ActionResult> agregarPuntoVentaUsuario(UsuarioPuntoVentaDto objDto)
        {
            var obj = _mapper.Map<Usuariopuntoventum>(objDto);
            _context.Usuariopuntoventa.Add(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<UsuarioPuntoVentaDto>(obj);
            return Ok(objDto);
        }


        [HttpPut("modificar")]
        public async Task<ActionResult> actualizarPuntoVentaUsuario(UsuarioPuntoVentaDto objDto)
        {
            var obj = _mapper.Map<Usuariopuntoventum>(objDto);
            _context.Usuariopuntoventa.Update(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<UsuarioPuntoVentaDto>(obj);
            return Ok(objDto);
        }

        [HttpDelete("eliminar")]
        public async Task<ActionResult> eliminarPuntosVentaUsuario(string nombreUsuario)
        {
            List<Usuariopuntoventum> obj = await _context.Usuariopuntoventa.Where(p => p.Nombreusuario == nombreUsuario).ToListAsync();

           foreach (var objDto in obj)
            {
                _context.Usuariopuntoventa.Remove(objDto);
                await _context.SaveChangesAsync();
            }

            List<Usuariopuntoventum> obj2 = await _context.Usuariopuntoventa.Where(p => p.Nombreusuario == nombreUsuario).ToListAsync();

            return Ok();
        }
    }
}