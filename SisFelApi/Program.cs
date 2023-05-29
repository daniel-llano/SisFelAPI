using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NPOI.OpenXml4Net.OPC;
using SisFelApi.Negocio.Data;
using SisFelApi.Negocio.Helpers.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//ignorar los objetos ciclicos
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//Metodo para usar el AllowAnonymus en los controladores
  //builder.Services.AddControllersWithViews();

  //  builder.Services.AddMvc(config =>
  //  {
  //      var policy = new AuthorizationPolicyBuilder()
  //          .RequireAuthenticatedUser()
  //          .Build();
  //      config.Filters.Add(new AuthorizeFilter(policy));
  //  });


//builders
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false

        };
    });


// Add services to the container.

builder.Services.AddControllers();

//Add CorsPolicy
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// Agregando conexion con BD
builder.Services.AddDbContext<SisfelbdContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("SisfelbdContext")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(mensajeServicio => $"mensajeServicio_{System.Guid.NewGuid()}");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
