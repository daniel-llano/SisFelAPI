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
    public class UsuarioController : ControllerBase
    {
        public readonly SisfelbdContext _context;
        private readonly IMapper _mapper;

        public UsuarioController(SisfelbdContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("lista")]
        public async Task<ActionResult<List<Usuario>>> listaUsuarios(bool? activo)
        {

            ConversionList conversionList = new ConversionList();
            if (activo != null)
            {
                var list = await _context.Usuarios.Include(usuario => usuario.CodigorolNavigation).Where(x => x.Activo == activo).ToListAsync();
                return Ok(conversionList.conversionListUsuario(list));
            }
            if (activo==null) {
                var list = await _context.Usuarios.Include(usuario => usuario.CodigorolNavigation).ToListAsync();
                return Ok(conversionList.conversionListUsuario(list));
            }
            return Ok();
        }

        //Lista de usuarios con paginacion en gral
        [HttpGet("listaPaginada")]
        public IActionResult listaUsuariosPaginada([FromQuery] PostQueryFilter filters, bool activo)
        {
            filters.PageNumber = filters.PageNumber == 0 ? 1 : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? 10 : filters.PageSize;

            var objs = _context.Usuarios.Include(usuario => usuario.CodigorolNavigation).Where(c => c.Activo == activo).AsEnumerable();
            if (filters.filter != null)
            {
                objs = objs.Where(x => 
                x.Nombreusuario.ToLower().Contains(filters.filter.ToLower())||
                x.Ci.ToLower().Contains(filters.filter.ToLower())||
                x.Nombres.ToLower().Contains(filters.filter.ToLower())||
                x.Ap.ToLower().Contains(filters.filter.ToLower())||
                x.Am.ToLower().Contains(filters.filter.ToLower())
                ).ToList();
            }

            var pageobjs = PagedList<Usuario>.Create(objs, filters.PageNumber, filters.PageSize);
            var metadata = new MetaData
            {
                TotalCount = pageobjs.TotalCount,
                PageSize = pageobjs.PageSize,
                CurrentPage = pageobjs.CurrentPage,
                TotalPages = pageobjs.TotalPages,
                HasNextPage = pageobjs.HasNextPage,
                HasPreviousPage = pageobjs.HasPreviousPage,
            };
            var response = new ApiResponse<IEnumerable<Usuario>>(pageobjs)
            {
                Meta = metadata
            };
            return Ok(response);
        }

        [HttpGet("GetUserRol")]
        public async Task<ActionResult<List<Usuario>>> getuserrol(int codigoRol)
        {
            var obj = await _context.Usuarios.Where(usuario => usuario.Codigorol==codigoRol).ToListAsync();
            return Ok(obj);
        }

        [HttpGet("id")]
        public async Task<ActionResult<Usuario>> usuario(string nombreUsuario)
        {
            Usuario obj = await _context.Usuarios.FindAsync(nombreUsuario);
            var objDto = _mapper.Map<UsuarioDto>(obj);
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
        public async Task<ActionResult> agregarUsuario(UsuarioDto objDto)
        {
            // Encriptando contrasenia y obteniendo hash
            objDto.Clave = HelperNegocio.obtenerHash(objDto.Clave);

            var obj = _mapper.Map<Usuario>(objDto);
            _context.Usuarios.Add(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<UsuarioDto>(obj);
            return Created($"/id?nombreUsuario={objDto.Nombreusuario}", objDto);
        }

        [HttpPut("modificar")]
        public async Task<ActionResult> actualizarUsuario(UsuarioDto objDto)
        {
            var obj = _mapper.Map<Usuario>(objDto);
            _context.Usuarios.Update(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<UsuarioDto>(obj);
            return Created($"/id?nombreUsuario={objDto.Nombreusuario}", objDto);
        }

        [HttpPut("cambioContrasenia")]
        public async Task<ActionResult> cambiarContrasenia(string nombreUsuario, string nuevaContrasenia)
        {
            Usuario obj = await _context.Usuarios.FirstOrDefaultAsync(x => x.Nombreusuario.Equals(nombreUsuario));

            // Encriptando contrasenia y obteniendo hash
            obj.Clave = HelperNegocio.obtenerHash(nuevaContrasenia);

            _context.Usuarios.Update(obj);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("cambio de estado")]
        public async Task<ActionResult> estadoUsuario(string Nombreusuario, bool estado)
        {
            Usuario usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Nombreusuario.Equals(Nombreusuario));

            if (usuario == null)
            {
                return NotFound();
            }

            if (estado == true)
            {
                usuario.Activo = true;
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
                return Ok();
            }
            if (estado == false)
            {
                usuario.Activo = false;
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return Ok();
        }

        
    }
}


