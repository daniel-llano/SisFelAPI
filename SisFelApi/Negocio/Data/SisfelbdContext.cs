using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Data;
public partial class SisfelbdContext : DbContext
{
    public SisfelbdContext()
    {
    }

    public SisfelbdContext(DbContextOptions<SisfelbdContext> options)
        : base(options)
    {
    }
    
    public virtual DbSet<Auditoriafactura> Auditoriafacturas { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Datosbase> Datosbases { get; set; }

    public virtual DbSet<Enlace> Enlaces { get; set; }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }
    public virtual DbSet<Facturacionmasiva> Facturacionmasivas { get; set; }

    public virtual DbSet<Facturadetalle> Facturadetalles { get; set; }

    public virtual DbSet<General> Generals { get; set; }

    public virtual DbSet<Operadore> Operadores { get; set; }

    public virtual DbSet<Paqueterecepcioncompra> Paqueterecepcioncompras { get; set; }

    public virtual DbSet<Parametro> Parametros { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Puntoventum> Puntoventa { get; set; }

    public virtual DbSet<Registrocompra> Registrocompras { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Sucursal> Sucursals { get; set; }

    public virtual DbSet<Telefonocliente> Telefonoclientes { get; set; }
    public virtual DbSet<Historialtelefonico> HistorialTelefonico { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<Usuariopuntoventum> Usuariopuntoventa { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Database=SISFELBD;Username=postgres;Password=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auditoriafactura>(entity =>
        {
            entity.HasKey(e => e.Codigomovimiento).HasName("auditoriafactura_pkey");

            entity.ToTable("auditoriafactura");

            entity.Property(e => e.Codigomovimiento).HasColumnName("codigomovimiento");
            entity.Property(e => e.Codigofactura).HasColumnName("codigofactura");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.Usuario)
                .HasMaxLength(10)
                .HasColumnName("usuario");

            entity.HasOne(d => d.CodigofacturaNavigation).WithMany(p => p.Auditoriafacturas)
                .HasForeignKey(d => d.Codigofactura)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("auditoriafactura_codigofactura_fkey");

            entity.HasOne(d => d.UsuarioNavigation).WithMany(p => p.Auditoriafacturas)
                .HasForeignKey(d => d.Usuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("auditoriafactura_usuario_fkey");
        });


        modelBuilder.Entity<Operadore>(entity =>
        {
            entity.HasKey(e => e.Nit).HasName("operadores_pkey");

            entity.ToTable("operadores");

            entity.Property(e => e.Nit)
                .ValueGeneratedNever()
                .HasColumnName("nit");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("true")
                .HasColumnName("activo");
            entity.Property(e => e.Barriozona)
                .HasMaxLength(90)
                .HasColumnName("barriozona");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(30)
                .HasColumnName("ciudad");
            entity.Property(e => e.Direccion)
                .HasMaxLength(50)
                .HasColumnName("direccion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(90)
                .HasColumnName("nombre");
            entity.Property(e => e.Sucursal)
                .HasMaxLength(70)
                .HasColumnName("sucursal");
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .HasColumnName("telefono");
        });


        modelBuilder.Entity<Puntoventum>(entity =>
        {
            entity.HasKey(e => e.Codigopuntoventa).HasName("puntoventa_pkey");

            entity.ToTable("puntoventa");

            entity.Property(e => e.Codigopuntoventa)
                .ValueGeneratedNever()
                .HasColumnName("codigopuntoventa");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("true")
                .HasColumnName("activo");
            entity.Property(e => e.Codigosucursal).HasColumnName("codigosucursal");
            entity.Property(e => e.Nombrepuntoventa)
                .HasMaxLength(60)
                .HasColumnName("nombrepuntoventa");
            entity.Property(e => e.Tipopuntoventa)
                .HasMaxLength(60)
                .HasColumnName("tipopuntoventa");

            entity.HasOne(d => d.CodigosucursalNavigation).WithMany(p => p.Puntoventa)
                .HasForeignKey(d => d.Codigosucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("puntoventa_codigosucursal_fkey");
        });


        modelBuilder.Entity<Sucursal>(entity =>
        {
            entity.HasKey(e => e.Codigosucursal).HasName("sucursal_pkey");

            entity.ToTable("sucursal");

            entity.HasIndex(e => e.Nombresucursal, "unq_sucursal_nombresucursal").IsUnique();

            entity.Property(e => e.Codigosucursal)
                .ValueGeneratedNever()
                .HasColumnName("codigosucursal");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("true")
                .HasColumnName("activo");
            entity.Property(e => e.Barrio)
                .HasMaxLength(60)
                .HasColumnName("barrio");
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .HasColumnName("direccion");
            entity.Property(e => e.Municipio)
                .HasMaxLength(35)
                .HasColumnName("municipio");
            entity.Property(e => e.Nombresucursal)
                .HasMaxLength(60)
                .HasColumnName("nombresucursal");
            entity.Property(e => e.Telefono)
                .HasMaxLength(40)
                .HasColumnName("telefono");
        });


        //OnModelCreatingPartial(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
