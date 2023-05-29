using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SisFelApi.Negocio.Data;
using SisFelApi.Negocio.DTOs;
using SisFelApi.Negocio.Models;
using SisFelApi.Negocio.Helpers;
using Microsoft.AspNetCore.Authorization;
using SisFelApi.Negocio.Helpers.Models;

namespace SisFelApi.Negocio.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductoController : Controller
{

    public readonly SisfelbdContext _context;
    private readonly IMapper _mapper;

    public ProductoController(SisfelbdContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("lista")]
    public async Task<ActionResult<List<Producto>>> listaProductos()
    {
        ConversionList conversionList= new ConversionList();
        var list = await _context.Productos.Include(product => product.CodigocategoriaNavigation).ToListAsync();
        return Ok(conversionList.conversionListProducto(list));
    }

    //Lista de productos con paginacion en gral
        [HttpGet("listaPaginada")]
        public IActionResult listaProductosPaginada([FromQuery] PostQueryFilter filters, bool activo)
        {
            filters.PageNumber = filters.PageNumber == 0 ? 1 : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? 10 : filters.PageSize;

            var objs = _context.Productos.Include(producto => producto.CodigocategoriaNavigation).Where(c => c.Activo == activo).AsEnumerable();
            if (filters.filter != null)
            {
                objs = objs.Where(x => 
                x.Codigoproducto.ToLower().Contains(filters.filter.ToLower())||
                x.Nombreproducto.ToLower().Contains(filters.filter.ToLower())||
                x.Tipoproducto.ToString().ToLower().Contains(filters.filter.ToLower())||
                x.Codigocategoria.ToString().ToLower().Contains(filters.filter.ToLower())
                ).ToList();
            }

            var pageobjs = PagedList<Producto>.Create(objs, filters.PageNumber, filters.PageSize);
            var metadata = new MetaData
            {
                TotalCount = pageobjs.TotalCount,
                PageSize = pageobjs.PageSize,
                CurrentPage = pageobjs.CurrentPage,
                TotalPages = pageobjs.TotalPages,
                HasNextPage = pageobjs.HasNextPage,
                HasPreviousPage = pageobjs.HasPreviousPage,
            };
            var response = new ApiResponse<IEnumerable<Producto>>(pageobjs)
            {
                Meta = metadata
            };
            return Ok(response);
        }
    
    [HttpGet("id")]
    public async Task<ActionResult<Producto>> producto(string codigoProducto)
    {
        Producto obj = await _context.Productos.FirstOrDefaultAsync(x=>x.Codigoproducto.Equals(codigoProducto));
        var objDto = _mapper.Map<ProductoDto>(obj);
        if (objDto == null)
        {
            return NotFound();
        }else{
            return Ok(objDto);
        }
    }

    [HttpPost("agregar")]
    public async Task<ActionResult> agregarProducto(ProductoDto objDto)
    {
        var obj= _mapper.Map<Producto>(objDto);
        _context.Productos.Add(obj);
        await _context.SaveChangesAsync();
        objDto=_mapper.Map<ProductoDto>(obj);
        return Created($"/id?codigoProducto={objDto.Codigoproducto}",objDto);
    }

    [HttpPut("modificar")]
    public async Task<ActionResult> actualizarProducto(ProductoDto objDto)
    {
        var obj = _mapper.Map<Producto>(objDto);
        _context.Productos.Update(obj);
        await _context.SaveChangesAsync();
        objDto = _mapper.Map<ProductoDto>(obj);
        return Created($"/id?codigoProducto={objDto.Codigoproducto}",objDto);
    }

    [HttpPut("cambio de estado")]
    public async Task<ActionResult> estadoProducto(string Codigoproducto, bool estado)
    {
        Producto producto = await _context.Productos.FirstOrDefaultAsync(x => x.Codigoproducto.Equals(Codigoproducto));

        if(producto == null)
        {
            return NotFound();
        }

        if (estado == true)
        {
            producto.Activo = true;
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
            return Ok();
        }
        if (estado == false)
        {
            producto.Activo = false;
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
            return Ok();
        }
        return Ok();
    }
}
