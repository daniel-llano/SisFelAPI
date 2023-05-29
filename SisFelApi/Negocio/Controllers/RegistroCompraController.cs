using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class RegistroCompraController : ControllerBase
    {
        public readonly SisfelbdContext _context;
        private readonly IMapper _mapper;

        public RegistroCompraController(SisfelbdContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //trae la lista de categorias en gral.
        [HttpGet("lista")]
        public async Task<ActionResult<List<RegistroCompra>>> listaRegistrosCompra()
        {
            return Ok(await _context.Registrocompras.ToListAsync());
        }
        //trae una categoria en específica mandando el codigo de la misma
        [HttpGet("id")]
        public async Task<ActionResult<RegistroCompra>> registroCompra(int codigoCompra)
        {
            Registrocompra obj = await _context.Registrocompras.FindAsync(codigoCompra);
            var objDto = _mapper.Map<RegistroCompraDto>(obj);
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
        public async Task<ActionResult> agregarRegistroCompra(RegistroCompraDto objDto)
        {
            // Corrigiendo formato de fecha
            objDto.Fechaemision = objDto.Fechaemision.Value.ToLocalTime();

            // Cargando registro compra en BD
            var obj = _mapper.Map<Registrocompra>(objDto);
            _context.Registrocompras.Add(obj);
            await _context.SaveChangesAsync();

            // Actualizando estado de paquete al cual pertenece el registro compra
            PaqueteRecepcionCompraController paqueteRecepcionCompraController = new PaqueteRecepcionCompraController(_context, _mapper);
            
            try{
                await paqueteRecepcionCompraController.actualizarEstadoPaqueteRecepcionCompra(objDto.Codigopaqueterecepcioncompra, 3);
            }catch(Exception e){
                Console.WriteLine($"{e.Message}");
                return NotFound();
            }

            return Created($"/id?codigoCompra={obj.Codigocompra}", obj);
        }
        //modifica una nueva categoria mandando el objeto correspondiente de tipo CategoriaDto
        [HttpPut("modificar")]
        public async Task<ActionResult> actualizarRegistroCompra(RegistroCompraDto objDto)
        {
            var obj = _mapper.Map<Registrocompra>(objDto);
            _context.Registrocompras.Update(obj);
            await _context.SaveChangesAsync();
            return Created($"/id?codigoCompra={obj.Codigocompra}", obj);
        }
        //realiza el cambio de estado segun mandando el codigo de la misma y el tipo de estado a cambiar
        [HttpPut("cambio de estado")]
        public async Task<ActionResult> estadoRegistroCompra(int codigoCompra, int estado)
        {
            Registrocompra obj = await _context.Registrocompras.FindAsync(codigoCompra);

            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                obj.Estado = estado;
                _context.Registrocompras.Update(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}
