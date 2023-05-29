using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SisFelApi.Negocio.Data;
using SisFelApi.Negocio.DTOs;
using SisFelApi.Negocio.Models;
using SisFelApi.Negocio.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace SisFelApi.Negocio.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TelefonoClienteController : ControllerBase
    {
        public readonly SisfelbdContext _context;
        private readonly IMapper _mapper;
        public TelefonoClienteController(SisfelbdContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("lista")]
        public async Task<ActionResult<List<TelefonoClienteDto>>> listaTelefonosClientes()
        {
            ConversionList conversionList = new ConversionList();
            var list = await _context.Telefonoclientes.Include(telCliente => telCliente.CodigoclienteNavigation).ToListAsync();
            return Ok(conversionList.conversionListListTelefonoClientes(list));
        }

        [HttpGet("id")]
        public async Task<ActionResult<TelefonoClienteDto>> telefonoCliente(int codigoTelefonoCliente)
        {
            Telefonocliente obj = await _context.Telefonoclientes.FindAsync(codigoTelefonoCliente);
            var objDto = _mapper.Map<TelefonoClienteDto>(obj);
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
        public async Task<ActionResult> agregarTelefonoCliente(TelefonoClienteDto objDto)
        {
            var obj = _mapper.Map<Telefonocliente>(objDto);
            _context.Telefonoclientes.Add(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<TelefonoClienteDto>(obj);
            return Created($"/id?codigoTelefonoCliente={objDto.Codigotelefonocliente}", objDto);
        }
        [HttpPut("modificar")]
        public async Task<ActionResult> actualizarTelefonoCliente(TelefonoClienteDto objDto)
        {
            var obj = _mapper.Map<Telefonocliente>(objDto);
            _context.Telefonoclientes.Update(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<TelefonoClienteDto>(obj);
            return Created($"/id?codigoTelefonoCliente={objDto.Codigotelefonocliente}", objDto);
        }
        //modifica los campos de nit y razon social segun el codigo telefono cliente que se mande y se agrega un nuevo historial telefonico con los datos antiguos antes de modificarlos
        [HttpPut("cambio de estado")]
        public async Task<ActionResult> estadoTelefonoCliente(TelefonoClienteDto objDto)
        {
            Telefonocliente obj = await _context.Telefonoclientes.FindAsync(objDto.Codigotelefonocliente);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                //agregandio un nuevo Historial Telefonico(obj);
                Historialtelefonico historialtelefonico = new Historialtelefonico()
                {
                    Codigohistorialtelefonico = 0,
                    Codigotelefonocliente = obj.Codigotelefonocliente,
                    Codigocliente = obj.Codigocliente,
                    Codigotipodocumentoidentidad = obj.Codigotipodocumentoidentidad,
                    Nit = obj.Nit,
                    Ci = obj.Ci,
                    Complemento = obj.Complemento,
                    Razonsocial = obj.Razonsocial,
                    Email = obj.Email,
                    Telefono = obj.Telefono,
                    Fechacambio = DateOnly.FromDateTime(DateTime.Now),
                    Activo = false
                };
                _context.HistorialTelefonico.Add(historialtelefonico);

                // finalizando la insercion del nuevo Historial Telefonico(obj);
                obj.Telefono = objDto.Telefono;
                obj.Nit = objDto.Nit;
                obj.Razonsocial = objDto.Razonsocial;
                obj.Ci = objDto.Ci;
                obj.Complemento = objDto.Complemento;
                obj.Email = objDto.Email;
                obj.Codigotipodocumentoidentidad = objDto.Codigotipodocumentoidentidad;


                _context.Telefonoclientes.Update(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}
