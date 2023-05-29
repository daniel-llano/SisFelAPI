using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SisFelApi.Negocio.Data;
using SisFelApi.Negocio.DTOs;
using SisFelApi.Negocio.Models;
using ServiceCompraVenta;
using SisFelApi.Negocio.Helpers;
using SisFelApi.Negocio.Helpers.Models;

namespace SisFelApi.Negocio.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        public readonly SisfelbdContext _context;
        private readonly IMapper _mapper;
        public EventoController(SisfelbdContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("lista")]
        public async Task<ActionResult<List<Evento>>> listaEventos(bool? activo)
        {
            ConversionList conversionList = new ConversionList();
            if(activo != null){
                var list = await _context.Eventos.Where(x => x.Activo == activo).ToListAsync();
                return Ok(list);
            }
            if(activo == null){
                var list = await _context.Eventos.Include(evento => evento.Codigoevento).ToListAsync();
                return Ok(list);
            }
            return Ok();
        }

        //Lista de eventos con paginacion en gral
        [HttpGet("listaPaginada")]
        public IActionResult listaEventosPaginada([FromQuery] PostQueryFilter filters, bool activo)
        {
            filters.PageNumber = filters.PageNumber == 0 ? 1 : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? 10 : filters.PageSize;

            var objs = _context.Eventos.Where(c => c.Activo == activo).AsEnumerable();
            if (filters.filter != null)
            {
                objs = objs.Where(x => 
                x.Codigoevento.ToString().ToLower().Contains(filters.filter.ToLower())||
                x.Codigomotivoevento.ToString().ToLower().Contains(filters.filter.ToLower())
                ).ToList();
            }

            var pageobjs = PagedList<Evento>.Create(objs, filters.PageNumber, filters.PageSize);
            var metadata = new MetaData
            {
                TotalCount = pageobjs.TotalCount,
                PageSize = pageobjs.PageSize,
                CurrentPage = pageobjs.CurrentPage,
                TotalPages = pageobjs.TotalPages,
                HasNextPage = pageobjs.HasNextPage,
                HasPreviousPage = pageobjs.HasPreviousPage,
            };
            var response = new ApiResponse<IEnumerable<Evento>>(pageobjs)
            {
                Meta = metadata
            };
            return Ok(response);
        }

        [HttpGet("id")]
        public async Task<ActionResult<Evento>> evento(int codigoEvento)
        {
            Evento evento = await _context.Eventos.FindAsync(codigoEvento);
            var eventoDto = _mapper.Map<EventoDto>(evento);
            if (eventoDto == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(evento);
            }
        }

        [HttpPost("agregar")]
        public async Task<ActionResult> agregarEvento(EventoDto eventoDto)
        {
            await Helper.actualizarCodigos(eventoDto.Codigopuntoventa);

            eventoDto.Cuis = Helper.general.Cuis;

            if(eventoDto.Codigomotivoevento >= 129 && eventoDto.Codigomotivoevento <= 132)
            {
                eventoDto.Cufdevento = Helper.general.Cufd;
                eventoDto.Cafccompraventa = null;
                eventoDto.Cafctelecom = null;
                eventoDto.Fechahorainicioevento = DateTime.Now;
            }
            else if(eventoDto.Codigomotivoevento >= 133 && eventoDto.Codigomotivoevento <= 135){
                eventoDto.Cufdevento = eventoDto.Cufdevento;
                eventoDto.Cafccompraventa = eventoDto.Cafccompraventa;
                eventoDto.Cafctelecom = eventoDto.Cafctelecom;
                eventoDto.Fechahorainicioevento = eventoDto.Fechahorainicioevento;
            }

            var obj = _mapper.Map<Evento>(eventoDto);
            _context.Eventos.Add(obj);
            await _context.SaveChangesAsync();
            eventoDto = _mapper.Map<EventoDto>(obj);
            return Created($"/id?codigoEvento={eventoDto.Codigoevento}", eventoDto);
        }

        [HttpPut("modificar")]
        public async Task<ActionResult> actualizarEvento(Evento evento)
        {
            Evento eventoBd = await _context.Eventos.FindAsync(evento.Codigoevento);
            
            eventoBd.Codigomotivoevento = evento.Codigomotivoevento;
            eventoBd.Codigorecepcionevento = evento.Codigorecepcionevento;
            eventoBd.Cufdevento = evento.Cufdevento;
            eventoBd.Cafccompraventa = evento.Cafccompraventa;
            eventoBd.Cafctelecom = evento.Cafctelecom;
            eventoBd.Activo = evento.Activo;
            eventoBd.Codigopuntoventa = evento.Codigopuntoventa;
            eventoBd.Descripcion = evento.Descripcion;
            eventoBd.Fechahorainicioevento = evento.Fechahorainicioevento;
            eventoBd.Cuis = evento.Cuis;
            eventoBd.Cufd = evento.Cufd;
            eventoBd.Fechahorafinevento = DateTime.Now;

             _context.Eventos.Update(eventoBd);
            await _context.SaveChangesAsync();
            return Created($"/id?codigoEvento={evento.Codigoevento}", evento);
        }

        [HttpPut("eliminar")]
        public async Task<Evento?> eliminarEvento(int codigoEvento, bool estado)
        {
            Evento evento = await _context.Eventos.FindAsync(codigoEvento);
            if (evento == null)
            {
                return null;
            }
            if (estado == true)
            {
                evento.Activo = true;
                _context.Eventos.Update(evento);
                await _context.SaveChangesAsync();
                return evento;
            }
            if (estado == false)
            {
                evento.Activo = false;
                
                _context.Eventos.Update(evento);
                await _context.SaveChangesAsync();

                return evento;
            }
            return evento;
        }

        [HttpPut("cambio de estado")]
        public async Task<ActionResult> cambiarEstado(int codigoEvento, bool estado)
        {
            Evento evento = await _context.Eventos.FirstOrDefaultAsync(x => x.Codigoevento.Equals(codigoEvento));

            if (evento == null)
            {
                return NotFound();
            }
            if (estado == true)
            {
                evento.Activo = true;
                _context.Eventos.Update(evento);
                await _context.SaveChangesAsync();
                return Ok();
            }
            if (estado == false)
            {
                evento.Activo = false;
                _context.Eventos.Update(evento);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return Ok();
        }

        [HttpPut("terminarEvento")]
        public async Task<respuestaRecepcion> terminarEvento(int codigoEvento, bool estado)
        {
            Evento evento = await _context.Eventos.FindAsync(codigoEvento);

            if (estado == true)
            {
                evento.Activo = true;
                _context.Eventos.Update(evento);
                await _context.SaveChangesAsync();
            }else if (estado == false)
            {

                evento.Activo = false;
                _context.Eventos.Update(evento);
                await _context.SaveChangesAsync();

                return new respuestaRecepcion(){
                        codigoEstado = 52,
                        codigoDescripcion = "Alerta: El evento ya fue terminado. Por favor registre un nuevo evento."
                };
            }

            try
            {

                int respuesta = await Helper.enviarFacturasGeneradasPorEvento(evento.Codigopuntoventa, evento.Codigomotivoevento, 1);

                if(respuesta == 1)
                {
                    return new respuestaRecepcion(){
                        codigoEstado = 52,
                        codigoDescripcion = "Exito: Se enviaron todos los paquetes de facturas correctamente. Por favor verifique el estado de las facturas."
                };  
                }
                else if(respuesta == 2)
                {
                    return new respuestaRecepcion(){
                        codigoEstado = 52,
                        codigoDescripcion = "Alerta: No existen facturas a enviar. Por favor genere facturas e intente nuevamente. En caso de no haber generado facturas dentro del evento deshabilite el evento."
                };
                }
                else if(respuesta == -1)
                {
                    return new respuestaRecepcion(){
                        codigoEstado = 52,
                        codigoDescripcion = "Error: No se envio uno o todos los paquetes. Por favor verifique el estado de las facturas."
                };
                }
                else if(respuesta == -2)
                {
                    return new respuestaRecepcion(){
                        codigoEstado = 52,
                        codigoDescripcion = "Error: No se pudo registrar el evento. Por favor verifique que los datos registrados sean correctos e intente de nuevo."
                };
                }
                else 
                {
                    return new respuestaRecepcion(){
                        codigoEstado = 52,
                        codigoDescripcion = ""
                };
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"{e.Message}");
                return new respuestaRecepcion(){
                        codigoEstado = 52,
                        codigoDescripcion = "Error: Ocurrio un error inesperado, por favor registre los datos enviados y comuniquese con el personal de soporte."
                };
            }
        }

        [HttpGet("estadoEvento")]
        public async Task<bool> estadoUltimoEvento(int codigoEvento){
            Evento evento = await _context.Eventos.FindAsync(codigoEvento);
            if(ultimoEvento == null)
            {
                return false;
            }else{
                return (bool)evento.Activo;
            }
        }

        [HttpGet("ultimoEvento")]
        public async Task<Evento> ultimoEvento(){
            
            if(_context.Eventos.Count()==0)
            {
                Evento evento = new Evento();
                evento.Activo = false;
                evento.Cafccompraventa = null;
                evento.Cafctelecom = null;
                evento.Codigoevento = 0;
                evento.Codigomotivoevento = 1;
                evento.Codigopuntoventa = 0;
                evento.Codigorecepcionevento = 0;
                evento.Cufd = "";
                evento.Cufdevento = "";
                evento.Cuis = "";
                evento.Descripcion = "";
                evento.Fechahorainicioevento = new DateTime();

                return evento;
            }
            else
            {
                return _context.Eventos.AsNoTracking().OrderByDescending(x => x.Codigoevento).FirstOrDefault();
            }
        }
    }
}
