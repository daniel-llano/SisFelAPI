using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SisFelApi.Negocio.Data;
using SisFelApi.Negocio.DTOs;
using SisFelApi.Negocio.Models;
using SisFelApi.Negocio.Helpers;
using SisFelApi.Negocio.Helpers.Models;

namespace SisFelApi.Negocio.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        public readonly SisfelbdContext _context;
        private readonly IMapper _mapper;
        public ClienteController(SisfelbdContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //Lista de clientes en gral
        [HttpGet("lista")]
        public async Task<ActionResult<List<Cliente>>> listaClientes(bool activo)
        {   
            return Ok(await _context.Clientes.Include(cliente=>cliente.Telefonoclientes).Where(c => c.Activo == activo).ToListAsync());
        }


        //Lista de clientes con paginacion en gral
        [HttpGet("listaPaginada")]
        public IActionResult listaClientesPaginada([FromQuery] PostQueryFilter filters, bool activo)
        {
            filters.PageNumber = filters.PageNumber == 0 ? 1 : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? 10 : filters.PageSize;

            var objs = _context.Clientes.Include(cliente => cliente.Telefonoclientes).Where(c => c.Activo == activo).AsEnumerable();
            if (filters.filter != null)
            {
                objs = objs.Where(x => 
                x.Codigocliente.ToLower().Contains(filters.filter.ToLower())||
                x.Datoscliente.ToLower().Contains(filters.filter.ToLower())||
                x.Ci.ToLower().Contains(filters.filter.ToLower())
                ).ToList();
            }

            var pageobjs = PagedList<Cliente>.Create(objs, filters.PageNumber, filters.PageSize);
            var metadata = new MetaData
            {
                TotalCount = pageobjs.TotalCount,
                PageSize = pageobjs.PageSize,
                CurrentPage = pageobjs.CurrentPage,
                TotalPages = pageobjs.TotalPages,
                HasNextPage = pageobjs.HasNextPage,
                HasPreviousPage = pageobjs.HasPreviousPage,
            };
            var response = new ApiResponse<IEnumerable<Cliente>>(pageobjs)
            {
                Meta = metadata
            };
            return Ok(response);
        }

        [HttpGet("id")]
        public async Task<ActionResult<ClienteDto>> cliente(string codigoCliente)
        {
            Cliente obj = await _context.Clientes.FindAsync(codigoCliente);
            var objDto = _mapper.Map<ClienteDto>(obj);
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
        public async Task<ActionResult> agregarCliente(ClienteDto objDto)
        {
            var obj = _mapper.Map<Cliente>(objDto);
            _context.Clientes.Add(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<ClienteDto>(obj);
            return Created($"/id?codigoCliente={objDto.Codigocliente}", objDto);
        }
        [HttpPut("modificar")]
        public async Task<ActionResult> actualizarCliente(ClienteDto objDto)
        {
            var obj = _mapper.Map<Cliente>(objDto);
            _context.Clientes.Update(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<ClienteDto>(obj);
            return Created($"/id?codigoCliente={objDto.Codigocliente}", objDto);
        }

        [HttpPut("cambio de estado")]
        public async Task<ActionResult> estadoCliente(string CodigoCliente, bool estado)
        {
            Cliente obj = await _context.Clientes.FirstOrDefaultAsync(x=> x.Codigocliente.Equals(CodigoCliente));

            if (obj == null)
            {
                return NotFound();
            }

            if (estado == true)
            {
                obj.Activo = true;
                _context.Clientes.Update(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            if (estado == false)
            {
                obj.Activo = false;
                _context.Clientes.Update(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return Ok();
        }
    }
}

