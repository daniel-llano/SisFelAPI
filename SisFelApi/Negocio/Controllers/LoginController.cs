using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using SisFelApi.Negocio.Data;
using SisFelApi.Negocio.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SymmetricSecurityKey = Microsoft.IdentityModel.Tokens.SymmetricSecurityKey;
using SisFelApi.Negocio.Helpers;

namespace SisFelApi.Negocio.Controllers;
[ApiController]
[Route("api/[controller]")]

public class LoginController : ControllerBase
{

    public readonly SisfelbdContext _context;
    private readonly IMapper _mapper;
    private IConfiguration config;

    public LoginController(SisfelbdContext context, IMapper mapper, IConfiguration config)
    {
        _context = context;
        _mapper = mapper;
        this.config = config;
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Login(LoginDto adminDto)
    {
        // Encriptando contrasenia y obteniendo hash
        adminDto.Clave = HelperNegocio.obtenerHash(adminDto.Clave);

        var admin = await _context.Usuarios.Include(x => x.CodigorolNavigation).FirstOrDefaultAsync(x => x.Nombreusuario == adminDto.Usuario && x.Clave == adminDto.Clave && x.Activo==true);

        if (admin is null)
            return Ok(new { token = "" });

        string jwtToken = GenerateToken(adminDto);
        
        return Ok(new { codigorol=admin.Codigorol, token = jwtToken, });

    }

    private string GenerateToken(LoginDto admin)
    {
        var claims = new[]
        {
        new Claim(ClaimTypes.Name, admin.Usuario),
        new Claim(ClaimTypes.Rsa, admin.Clave)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(1440),
            signingCredentials: creds);

        string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return token;
    }

}
