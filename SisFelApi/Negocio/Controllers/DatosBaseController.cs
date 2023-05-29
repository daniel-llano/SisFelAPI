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
    public class DatosBaseController : ControllerBase
    {
        public readonly SisfelbdContext _context;
        public DatosBaseController(SisfelbdContext context)
        {
            _context = context;
        }

        [HttpGet("lista")]
        public async Task <Datosbase> listaDatosBases()
        {
            
            Datosbase datosbase = _context.Datosbases.OrderByDescending(x => x.Codigodatobase).FirstOrDefault();

            if(datosbase != null)
            {
                return datosbase;
            }
            else
            {
                datosbase = new Datosbase();
                datosbase.Codigodatobase = '1';
                datosbase.Nrofacturapaquetecv = 1;
                datosbase.Nrofacturapaquetetl = 1;
                datosbase.Nropaquetecv = 1;
                datosbase.Nropaquetetl = 1;

                datosbase.Nrofacturapaquetemasivocv = 1;
                datosbase.Nrofacturapaquetemasivotl = 1;
                datosbase.Nropaquetemasivocv = 1;
                datosbase.Nropaquetemasivotl = 1;

                _context.Datosbases.Add(datosbase);
                await _context.SaveChangesAsync();

                return datosbase;
            }
            
        }

        [HttpPut("modificar")]
        public async Task<Datosbase> actualizarDatosBase(Datosbase datosbase)
        {
            Datosbase datosBaseBd = await _context.Datosbases.FindAsync(datosbase.Codigodatobase);

            datosBaseBd.Nrofacturapaquetecv = datosbase.Nrofacturapaquetecv;
            datosBaseBd.Nrofacturapaquetetl = datosbase.Nrofacturapaquetetl;
            datosBaseBd.Nropaquetecv = datosbase.Nropaquetecv;
            datosBaseBd.Nropaquetetl = datosbase.Nropaquetetl;

            datosBaseBd.Nrofacturapaquetemasivocv = datosbase.Nrofacturapaquetemasivocv;
            datosBaseBd.Nrofacturapaquetemasivotl = datosbase.Nrofacturapaquetemasivotl;
            datosBaseBd.Nropaquetemasivocv = datosbase.Nropaquetemasivocv;
            datosBaseBd.Nropaquetemasivotl = datosbase.Nropaquetemasivotl;


            _context.Datosbases.Update(datosBaseBd);
            await _context.SaveChangesAsync();
            return datosbase;
        }
    }
}
