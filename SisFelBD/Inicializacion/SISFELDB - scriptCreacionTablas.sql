--NOMBRE BASE DE DATOS: SISFELBD

--SENTENCIAS PARA LA CREACION DE TABLAS
--DROP DATABASE IF EXISTS "SISFELBD"
--create database SISFELBD;
--ALTER USER postgres WITH PASSWORD 'postgres';

--CREACION DE TABLA ROL
create table rol(
    codigoRol serial not null,
	nombreRol varchar(30) not null,
    descripcion varchar(150),
    activo 		BOOLEAN DEFAULT TRUE,
	constraint pk_rol primary key (codigoRol),
	constraint unq_rol_nombre unique (nombreRol)
);
-- CREACION DE TABLA ENLACES
create TABLE enlaces(
	codigoEnlace 	serial not null,
	nombreEnlace	varchar(30) not null,
	ruta		    varchar(50) not null,
	activo			BOOLEAN DEFAULT TRUE,
	constraint pk_enlace primary key (codigoEnlace)
);
--CREACION DE TABLA PERMISOS
create TABLE permisos(
	codigoRol 		integer,
	codigoEnlace 	integer,
	activo 			BOOLEAN DEFAULT TRUE,
    primary key (codigoRol,codigoEnlace),
	foreign key (codigoRol) references rol (codigoRol),
	foreign key (codigoEnlace) references enlaces (codigoEnlace)
);
--CREACION DE TABLA SUCURSAL
create table sucursal(
    codigoSucursal integer not null,
    nombreSucursal varchar(60) not null,
	direccion varchar(100) not null,
	barrio varchar(60) not null,
    municipio varchar (25) not null,
    telefono varchar(40) not null,
    activo boolean default true,
    primary key(codigoSucursal),
	constraint unq_sucursal_nombreSucursal unique (nombreSucursal)
);
--CREACION DE TABLA PUNTOVENTA
create table puntoVenta(
    codigoPuntoVenta integer not null,
    nombrePuntoVenta varchar(60) not null,
    tipoPuntoVenta varchar(60) not null,
    codigoSucursal integer not null,
    activo boolean default true,
    primary key (codigoPuntoVenta),
    foreign key (codigoSucursal) references sucursal(codigoSucursal)
);
--CREACION DE TABLA USUARIO
create table usuario (
    nombreUsuario varchar(30) not null, 
    ci varchar(15) not null,
    nombres varchar(40) not null,
    ap varchar(30),
    am varchar(30),
    telefono varchar(40),
    clave char(64) not null, 
    codigoRol integer not null,
    activo boolean default true, 
    constraint pk_usuario primary key (nombreUsuario),
    constraint fk_rol_usuario foreign key (codigoRol) references rol (codigoRol)
    on update cascade on delete set null
);
--CREACION DE TABLA USUARIO-PUNTOVENTA
create table usuarioPuntoVenta(
    nombreUsuario varchar(30) not null, 
    codigoPuntoVenta integer not null,
    activo boolean default true,
    primary key (nombreUsuario, codigoPuntoVenta),
    foreign key (nombreUsuario) references usuario(nombreUsuario),
    foreign key (codigoPuntoVenta) references puntoVenta(codigoPuntoVenta)
);
--CREACION DE TABLA CATEGORIA
create table categoria(
    codigoCategoria     integer not null,
    codigoActividad     integer not null,
    codigoProductoSin   integer not null,
    descripcionProducto varchar(500),
    activo              boolean default true,
    primary key (codigoCategoria)
);
--CREACION DE TABLA PRODUCTO
create table producto(
    codigoProducto varchar(50) not null,
	nombreProducto varchar(500) not null,
	tipoProducto char(1) not null, --Sacar de tabla parametros
    precio NUMERIC(19,2) not null,
    codigoUnidadMedida integer not null, --Sacar de tabla parametros
    codigoCategoria integer not null,
    activo boolean default true,
    primary key (codigoProducto),
    foreign key (codigoCategoria) references categoria(codigoCategoria)
);

-- Equivalente a tabla dbo.T_Socio
create table cliente(
    codigoCliente varchar(10) not null,
    datosCliente varchar(100),
    ci varchar(50),
    tipoPersona char(1),
    activo boolean default true,
    primary key (codigoCliente)
);

--CREACION DE TABLA TELEFONOCLIENTE
create table telefonoCliente(
    codigoTelefonoCliente varchar(10) not null, 
    codigoCliente varchar(10) not null,
    codigoTipoDocumentoIdentidad integer not null,
    nit varchar(20),
    ci varchar(20),
    complemento varchar(5),
    razonSocial varchar(500) not null,
    email varchar(70),
    telefono integer not null,
    activo boolean default true,
    primary key (codigoTelefonoCliente),
    foreign key (codigoCliente) references cliente(codigoCliente)
);

--CREACION DE TABLA HISTORIAL TELEFONICO
create table historialTelefonico(
    codigoHistorialTelefonico serial not null,
    codigoTelefonoCliente varchar(10) not null, 
    codigoCliente varchar(10) not null,
    codigoTipoDocumentoIdentidad integer not null,
    ci varchar(20),
    nit varchar(20),
    complemento varchar(5),
    razonSocial varchar(500) not null,
    email varchar(70),
    telefono integer not null,
    fechaCambio date not null,
    activo boolean default true,
    primary key (codigoHistorialTelefonico ),
    foreign key (codigoCliente) references cliente(codigoCliente)
);

--CREACION DE TABLA FACTURA / PARTE DE LOS ATRIBUTOS ESTAN EN LA API
create table factura(
    codigoFactura serial not null,
	codigoRecepcion varchar(100),
	nitEmisor bigint,
    municipio varchar(25),
    telefonoEmisor integer,
    nitConjunto varchar(13),
    numeroFactura bigint not null,
    cuf varchar(100) not null,
    cufd varchar(100) not null,
    codigoSucursal integer not null,
    direccion varchar(500) not null,
    codigoPuntoVenta integer not null,
    cafc varchar(50),
    fechaEmision timestamp not null,
    nombreRazonSocial varchar(500) not null,
    codigoTipoDocumentoIdentidad integer not null,--
    numeroDocumento varchar(20) not null,
    complemento varchar(5),
    codigoTelefonoCliente varchar(10) not null, -- Considerar que en impuestos lo manejan como varchar(100)
    codigoMetodoPago integer not null,--Se sacará de la tabla parametros
    nroTarjeta bigint default null,
    montoTotal NUMERIC(19,2) not null,
    montoTotalSujetoIva NUMERIC(19,2) not null,
    codigoMoneda integer not null,-- Se sacará de la tabla parametros
    leyenda varchar(200) not null,
    usuario varchar(10) not null,
    codigoDocumentoSector integer not null,--  Se sacará de la tabla parametros 1 o 22
    estado_factura varchar(10) not null, --  Se sacará de la tabla parametros V , A ,  P
    descuentoAdicional  NUMERIC(19,2) not null,
    primary key(codigoFactura),
    foreign key(codigoSucursal) references sucursal(codigoSucursal),
    foreign key(codigoPuntoVenta) references puntoVenta(codigoPuntoVenta),
    foreign key(codigoTelefonoCliente) references telefonoCliente(codigoTelefonoCliente),
    foreign key(usuario) references usuario(nombreUsuario)
);
--CREACION DE TABLA FACTURADETALLE
create table facturadetalle(
    codigoFacturaDetalle serial not null,
    codigoFactura integer not null,
    codigoProducto varchar(50) not null,
    actividadEconomica varchar(10) not null,-- Se sacara de categoria codigoActividad
    codigoProductoSin integer not null,
    descripcion varchar(500) not null,
    cantidad NUMERIC(19,2) not null,
    unidadMedida integer not null,
    precioUnitario NUMERIC(19,2) not null,
    montoDescuento NUMERIC(19,2) not null,
    subtotal NUMERIC(19,2) not null,
    cuenta varchar(10) not null, --Ver en los datos del detalle factura enviado por cosett
    numeroSerie varchar(1500),
    numeroImei varchar(1500),
    codigoGrupo varchar(3),
    primary key (codigoFacturaDetalle),
    foreign key (codigoFactura) references factura(codigoFactura),
    foreign key (codigoProducto) references producto(codigoProducto)
);
--CREACION DE TABLA FACTURACIONMASIVA
create table facturacionmasiva(
    codigoFacturacionMasiva serial not null,
    cufdMasivo varchar(100) not null,
    fechaInicio timestamp not null,
    fechaFin timestamp not null,
    numeroFacturasEnviadas integer not null,
    numeroFacturaInicio integer not null,
    numeroFacturaFin integer not null,
    estado integer not null,
    codigoRecepcion varchar(100),
    primary key (codigoFacturacionMasiva)
);

--OPERADORES
create table operadores(
    nit bigint not null,
    nombre varchar(90) not null,
    sucursal varchar(70) not null,
    direccion varchar(500) not null,
    barrioZona varchar(90),
    telefono varchar(50) not null,
    ciudad varchar(30) not null,
	activo boolean DEFAULT TRUE,
    primary key(nit)
);

--CREACION DE TABLA GENERAL
create table general(
	codigoPuntoVenta integer not null,
    codigoSistema varchar(40),
    cuis varchar(16),
    fechaVigenciaCuis date,
    cufd varchar(100),
    fechaVigenciaCufd date,
    codigoControl varchar(20), 
    nit varchar(20),
    nombreEmpresa varchar(200),
    direccion varchar(500),
    telefono varchar(40),
    ciudad varchar(35),
    codigoAutorizacion text,
	primary key (codigoPuntoVenta) 
);

--CREACION DE TABLA PARAMETROS
CREATE TABLE parametros(
	codigoParametro varchar(10),
	nombreParametro varchar(70),
	nombreGrupo		varchar(10),
	activo boolean DEFAULT TRUE,
	primary KEY(codigoParametro, nombreGrupo)
);

-- CREACION DE TABLA EVENTO
create table evento(
    codigoEvento serial not null,
    codigoMotivoEvento integer not null,
    codigoRecepcionEvento bigint not null,
    codigoPuntoVenta integer not null,
    cafcCompraVenta varchar(50),
    cafcTelecom varchar(50),
    cuis varchar(16) not null,
    cufd varchar(100), 
    cufdEvento varchar(100) not null,
    descripcion varchar(1500) not null,
    fechaHoraInicioEvento timestamp not null,
    fechaHoraFinEvento timestamp not null,
    activo boolean default true,
    primary key(codigoEvento)
);

-- CREACION DE TABLA DATOSBASE
create table datosBase(
    codigoDatoBase char(1) default 1,
    nroPaqueteCV integer,
    nroFacturaPaqueteCV integer,
    nroPaqueteTL integer,
    nroFacturaPaqueteTL integer,
	nroPaqueteMasivoCV integer,
    nroFacturaPaqueteMasivoCV integer,
    nroPaqueteMasivoTL integer,
    nroFacturaPaqueteMasivoTL integer,
    primary key (codigoDatoBase)
);

-- CREACION DE TABLA PAQUETERECEPCIONCOMPRA 
create table paqueteRecepcionCompra(
    codigoPaqueteRecepcionCompra serial not null,
    codigoRecepcionPaquete varchar(100),
    cantidadFacturas integer,
    numeroFacturaInicio integer not null,
    numeroFacturaFin integer not null,
    fechaEnvio timestamp,
    gestion smallint,
    periodo smallint,
    estado smallint,
    primary key (codigoPaqueteRecepcionCompra)
);

-- CREACION DE TABLA REGISTROCOMPRA
create table registroCompra (
    codigoCompra serial not null,
    codigoPaqueteRecepcionCompra serial not null,
    nroCompra integer not null,
    nitEmisor bigint,
    razonSocialEmisor varchar(500),
    codigoAutorizacion varchar(100),
    numeroFactura varchar(20),
    numeroDuiDim varchar(15),
    fechaEmision timestamp,
    montoTotalCompra numeric(16,2),
    importeIce numeric(16,2),
    importeIehd numeric(16,2),
    importeIpj numeric(16,2),
    tasas numeric(16,2),
    otroNoSujetoCredito numeric(16,2),
    importesExentos numeric(16,2),
    importeTasaCero numeric(16,2),
    subTotal numeric(16,2),
    descuento numeric(16,2),
    montoGiftCard numeric(16,2),
    montoTotalSujetoIva numeric(16,2),
    creditoFiscal numeric(16,2),
    tipoCompra varchar(2),
    codigoControl varchar(17),
    estado integer,
    primary key (codigoCompra),
    foreign key (codigoPaqueteRecepcionCompra) references paqueteRecepcionCompra(codigoPaqueteRecepcionCompra)
);

--CREACION DE TABLA AUDITORIA
create table auditoriaFactura (
    codigoMovimiento serial,
    codigoFactura integer not null,
    usuario varchar(10) not null,
    fecha date,
    primary key (codigoMovimiento),
    foreign key (usuario) references usuario(nombreUsuario),
    foreign key (codigoFactura) references factura(codigoFactura)
);

-- Hacer trigger para modificacion de factura
-- CREACION DE FUNCION TRIGGER PARA TABLA AUDITORIA
CREATE OR REPLACE FUNCTION registroFacturaTrigger()
  RETURNS trigger AS
$$
BEGIN
    INSERT INTO auditoriaFactura ("codigofactura", "usuario","fecha")
        VALUES(NEW."codigofactura",NEW."usuario",current_date);
RETURN NEW;
END;
$$
LANGUAGE 'plpgsql';
-- CREACION DE TRIGGER PARA TABLA AUDITORIA
CREATE TRIGGER registroDatosFacturaTrigger1
  AFTER INSERT
  ON "factura"
  FOR EACH ROW
  EXECUTE PROCEDURE registroFacturaTrigger();

--Insert Data into Table Categoria
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(1,610000,84110, 'SERVICIOS DE OPERADORES', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(2,610000,84120, 'SERVICIOS DE TELEFONÍA FIJA', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(3,610000,84131, 'SERVICIOS DE VOZ MÓVIL', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(4,610000,84132, 'SERVICIOS DE TEXTO MÓVIL', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(5,610000,84133, 'SERVICIOS DE DATOS MÓVILES, EXCEPTO SERVICIOS DE TEXTO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(6,610000,84140, 'SERVICIOS DE REDES PRIVADAS', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(7,610000,84150, 'SERVICIOS DE TRANSMISIÓN DE DATOS', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(8,610000,84190, 'OTROS SERVICIOS DE TELECOMUNICACIONES', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(9,610000,84210, 'SERVICIOS BÁSICOS DE INTERNET', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(10,610000,84221, 'SERVICIOS DE ACCESO A INTERNET DE BANDA ANGOSTA', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(11,610000,84222, 'SERVICIOS DE ACCESO A INTERNET DE BANDA ANCHA', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(12,610000,84290, 'OTROS SERVICIOS DE TELECOMUNICACIONES A TRAVÉS DE INTERNET', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(13,610000,99100, 'OTROS PRODUCTOS O SERVICIOS ALCANZADOS POR EL IVA', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(14,610000,842219, 'SERVICIOS DE ACCESO A INTERNET DE BANDA ANGOSTA IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(15,610000,841109, 'SERVICIOS DE OPERADORES IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(16,610000,841209, 'SERVICIOS DE TELEFONÍA FIJA IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(17,610000,841319, 'SERVICIOS DE VOZ MÓVIL IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(18,610000,841329, 'SERVICIOS DE TEXTO MÓVIL IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(19,610000,841339, 'SERVICIOS DE DATOS MÓVILES, EXCEPTO SERVICIOS DE TEXTO IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(20,610000,841409, 'SERVICIOS DE REDES PRIVADAS IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(21,610000,841509, 'SERVICIOS DE TRANSMISIÓN DE DATOS IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(22,610000,841909, 'OTROS SERVICIOS DE TELECOMUNICACIONES IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(23,610000,842109, 'SERVICIOS BÁSICOS DE INTERNET IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(24,610000,842229, 'SERVICIOS DE ACCESO A INTERNET DE BANDA ANCHA IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(25,610000,842909, 'OTROS SERVICIOS DE TELECOMUNICACIONES A TRAVÉS DE INTERNET IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(26,610000,991009, 'OTROS PRODUCTOS O SERVICIOS ALCANZADOS POR EL IVA IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(27,610000,99804, 'DISTRIBUCIÓN DE SEÑALES', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(28,461021,13000, 'MINERALES Y CONCENTRADOS DE URANIO Y TORIO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(29,461021,14100, 'MINERALES Y CONCENTRADOS DE HIERRO, EXCEPTO PIRITA DE HIERRO TOSTADAS', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(30,461021,14210, 'MINERALES DE COBRE Y SUS CONCENTRADOS', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(31,461021,14220, 'MINERALES Y CONCENTRADOS DE NÍQUEL', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(32,461021,14230, 'MINERALES Y CONCENTRADOS DE ALUMINIO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(33,461021,14290, 'OTROS MINERALES Y METALES NO FERROSOS Y SUS CONCENTRADOS (EXCEPTO MINERALES Y CONCENTRADOS DE URANIO O TORIO)', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(34,461021,16120, 'PIRITAS DE HIERRO SIN TOSTAR', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(35,461021,34530, 'PIRITAS DE HIERRO TOSTADAS', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(36,461021,34110, 'HIDROCARBUROS Y SUS DERIVADOS HALOGENADOS, SULFONADOS, NITRADOS O NITROSADOS', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(37,461021,34120, 'ÁCIDOS GRASOS MONOCARBOXÍLICOS INDUSTRIALES; ACEITES ÁCIDOS PROCEDENTES DE LA REFINACIÓN', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(38,461021,34131, 'ALCOHOL ETÍLICO Y OTROS ALCOHOLES, DESNATURALIZADOS DE CUALQUIER GRADUACIÓN', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(39,461021,34139, 'OTROS ALCOHOLES, FENOLES, ALCOHOLES-FENOL, Y SUS DERIVADOS HALOGENADOS, SULFONADOS, NITRADOS O NITROSADOS; ALCOHOLES GRASOS INDUSTRIALES', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(40,461021,34140, 'ÁCIDOS CARBOXÍLICOS Y SUS ANHÍDRIDOS, HALUROS, PERÓXIDOS Y PEROXIÁCIDOS Y SUS DERIVADOS HALOGENADOS, SULFONADOS, NITRADOS O NITROSADOS, EXCEPTO ÁCIDO SALICÍLICO Y SUS SALES Y ÉSTERES Y SUS SALES', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(41,461021,34150, 'COMPUESTOS DE FUNCIÓN AMINO; COMPUESTOS AMINO CON FUNCIÓN OXIGENADA, EXCEPTO LISINA Y SUS ÉSTERES Y SALES DE ESTOS Y ÁCIDO GLUTÁMICO Y SUS SALES; UREÍNAS Y SUS DERIVADOS Y SALES DE ESTOS; COMPUESTOS CON FUNCIÓN CARBOXAMIDA; COMPUESTO IMINA, COMPUESTO FUNCIÓN NITRILO; COMPUESTOS, -DIAZO, -AZO O AZOXI; DERIVADOS ORGÁNICOS DE LA HIDRACINA O DE LA HIDROXILAMINA; COMPUESTOS CON OTRAS FUNCIONES NITROGENADAS', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(42,461021,34170, 'ÉTERES, PERÓXIDOS DE ALCOHOLES, PERÓXIDOS DE ÉTERES, EPÓXIDOS, ACETALES Y HEMIACETALES, Y SUS DERIVADOS HALOGENADOS, SULFONADOS, NITRADOS O NITROSADOS; COMPUESTOS DE FUNCIÓN ALDEHÍDO; COMPUESTOS DE FUNCIÓN CETONA Y COMPUESTOS DE FUNCIÓN QUINONA; ENZIMAS, ENZIMAS PREPARADAS N.C.P., COMPUESTOS ORGÁNICOS N.C.P.', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(43,461021,34180, 'ÉSTERES FOSFÓRICOS Y SUS SALES O ÉSTERES DE OTROS ÁCIDOS INORGÁNICOS (EXCLUYENDO ÉSTERES DE HALUROS DE HIDRÓGENO) Y SUS SALES; Y SUS DERIVADOS HALOGENADOS, SULFONADOS, NITRADOS Y NITROSADOS', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(44,461021,34231, 'ELEMENTOS QUÍMICOS N.C.P.; ÁCIDOS INORGÁNICOS EXCEPTO ÁCIDO FOSFÓRICO Y ÁCIDO SULFONÍTRICO; COMPUESTOS OXIGENADOS INORGÁNICOS DEL BORO, SILICIO Y CARBÓN; COMPUESTOS HALOGENADOS O SULFUROSOS DE METALES; HIDRÓXIDO DE SODIO Y PERÓXIDO DE MAGNESIO; ÓXIDOS, HIDRÓXIDOS Y PERÓXIDOS DE ESTRONCIO O DE BARIO; HIDRÓXIDO DE ALUMINIO; HYDROZINE E HIDROXILAMINA Y SUS SALES INORGÁNICAS', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(45,461021,34232, 'ÁCIDO FOSFÓRICO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(46,461021,34233, 'ÁCIDO NÍTRICO, ÁCIDOS SULFONÍTRICOS', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(47,461021,34250, 'SALES DE ÁCIDOS OXOMETÁLICOS O PEROXOMETÁLICOS; METALES PRECIOSOS EN FORMA COLOIDAL Y SUS COMPUESTOS; COMPUESTOS ORGÁNICOS E INORGÁNICOS DE MERCURIO; OTROS QUÍMICOS INORGÁNICOS N.C.P.; AIRE COMPRIMIDO; AMALGAMAS', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(48,461021,34260, 'ISÓTOPOS N.C.P. Y SUS COMPUESTOS (INCLUSO AGUA PESADA)', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(49,461021,34270, 'CIANUROS, OXICIANUROS Y CIANUROS COMPLEJOS; FULMINATOS, CIANATOS Y TIOCIANATOS; SILICATOS; BORATOS; PERBORATOS', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(50,461021,34280, 'PERÓXIDO DE HIDRÓGENO; FOSFUROS; CARBUROS; HIDRUROS, NITRUROS, AZIDAS, SILICIUROS Y BORUROS', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(51,461021,34290, 'COMPUESTOS DE METALES DE TIERRAS RARAS, DE ITRIO O DE ESCANDIO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(52,461021,34400, 'PRODUCTOS MINERALES NATURALES ACTIVADOS; NEGRO ANIMAL; ACEITE DE RESINA; ACEITES TERPÉNICOS PRODUCIDOS POR TRATAMIENTO DE MADERA DE CONÍFERAS; DIPENTENO CRUDO; PARACIMENO CRUDO; ACEITE DE PINO; COLOFONIA Y ÁCIDOS RESÍNICOS, Y SUS DERIVADOS; ESENCIAS DE COLOFONIA Y ACEITES DE COLOFONIA, ALQUITRÁN DE MADERA; ACEITES DE ALQUITRÁN DE MADERA, CREOSOTA DE MADERA, NAFTA DE MADERA, RESINA VEGETAL', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(53,461021,34540, 'ACEITES Y OTROS PRODUCTOS DE LA DESTILACIÓN DE ALQUITRÁN DE HULLA A ALTA TEMPERATURA, Y PRODUCTOS SIMILARES; BREA Y COQUE DE BREA OBTENIDOS DE ALQUITRANES MINERALES', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(54,461021,35210, 'ÁCIDO SALICÍLICO Y SUS SALES Y ÉSTERES', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(55,461021,35220, 'LISINA Y SUS ÉSTERES Y SALES DE ESTOS COMPUESTOS; ÁCIDO GLUTÁMICO Y SUS SALES; SALES E HIDRÓXIDOS DE AMONIO CUATERNARIO; LECITINAS Y OTROS FOSFOAMINOLÍPIDOS; AMIDAS ACÍCLICAS Y SUS DERIVADOS Y SALES DE ESTOS COMPUESTOS; AMIDAS CÍCLICAS (EXCEPTO UREÍNAS) Y SUS DERIVADOS Y SALES', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(56,461021,35230, 'LACTONAS N.C.P., COMPUESTOS HETEROCÍCLICOS QUE SÓLO CONTENGAN HETEROÁTOMOS DE NITRÓGENO, CON UN ANILLO DE PIRAZOL NO FUSIONADO, UN ANILLO DE PIRIMIDINA, UN ANILLO DE PIPERAZINA, UN ANILLO DE TRIAZINA NO FUSIONADO O UN SISTEMA DE ANILLOS DE FENOTIAZINA SIN OTRO TIPO DE FUSIÓN, HIDANTOÍNA Y SUS DERIVADOS; SULFONAMIDAS', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(57,461021,35240, 'AZÚCARES QUÍMICAMENTE PUROS N.C.P.; ÉTERES Y ÉSTERES DE AZÚCARES Y SUS SALES N.C.P.', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(58,461021,35250, 'PROVITAMINAS, VITAMINAS Y HORMONAS; GLUCÓSIDOS Y ALCALOIDES VEGETALES Y SUS SALES, ÉTERES, ÉSTERES Y OTROS DERIVADOS; ANTIBIÓTICOS', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(59,461021,35260, 'MEDICAMENTOS PARA USOS TERAPÉUTICOS O PROFILÁCTICOS', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(60,461021,35270, 'OTROS PRODUCTOS FARMACÉUTICOS', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(61,461021,35290, 'OTROS ARTÍCULOS PARA FINES MÉDICOS O QUIRÚRGICOS', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(62,461021,35470, 'ELEMENTOS Y COMPUESTOS QUÍMICOS CON ADITIVOS PARA SU USO EN ELECTRÓNICA', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(63,461021,35491, 'BIODIESEL', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(64,461021,35499, 'OTROS PRODUCTOS QUÍMICOS N.C.P.', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(65,461021,61271, 'SERVICIOS DE COMERCIO AL POR MAYOR DE PRODUCTOS QUÍMICOS INDUSTRIALES BÁSICOS Y RESINAS SINTÉTICAS, PRESTADOS A COMISIÓN O POR CONTRATO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(66,461021,61272, 'SERVICIOS DE COMERCIO AL POR MAYOR DE PRODUCTOS FERTILIZANTES Y AGROQUÍMICOS, PRESTADOS A COMISIÓN O POR CONTRATO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(67,461021,61273, 'SERVICIOS DE COMERCIO AL POR MAYOR DE PRODUCTOS FARMACÉUTICOS, PRESTADOS A COMISIÓN O POR CONTRATO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(68,461021,61292, 'SERVICIOS DE COMERCIO AL POR MAYOR DE MINERALES METALÍFEROS Y METALES EN FORMAS PRIMARIAS, PRESTADOS A COMISIÓN O POR CONTRATO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(69,461021,61299, 'SERVICIOS DE COMERCIO AL POR MAYOR DE MINERALES NO METÁLICOS Y OTROS PRODUCTOS N.C.P., PRESTADOS A COMISIÓN O POR CONTRATO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(70,461021,99100, 'OTROS PRODUCTOS O SERVICIOS ALCANZADOS POR EL IVA', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(71,461021,130009, 'MINERALES Y CONCENTRADOS DE URANIO Y TORIO IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(72,461021,141009, 'MINERALES Y CONCENTRADOS DE HIERRO, EXCEPTO PIRITA DE HIERRO TOSTADAS IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(73,461021,142109, 'MINERALES DE COBRE Y SUS CONCENTRADOS IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(74,461021,142209, 'MINERALES Y CONCENTRADOS DE NÍQUEL IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(75,461021,142309, 'MINERALES Y CONCENTRADOS DE ALUMINIO IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(76,461021,142909, 'OTROS MINERALES Y METALES NO FERROSOS Y SUS CONCENTRADOS (EXCEPTO MINERALES Y CONCENTRADOS DE URANIO O TORIO) IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(77,461021,161209, 'PIRITAS DE HIERRO SIN TOSTAR IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(78,461021,341109, 'HIDROCARBUROS Y SUS DERIVADOS HALOGENADOS, SULFONADOS, NITRADOS O NITROSADOS IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(79,461021,341209, 'ÁCIDOS GRASOS MONOCARBOXÍLICOS INDUSTRIALES; ACEITES ÁCIDOS PROCEDENTES DE LA REFINACIÓN IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(80,461021,341319, 'ALCOHOL ETÍLICO Y OTROS ALCOHOLES, DESNATURALIZADOS DE CUALQUIER GRADUACIÓN IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(81,461021,341399, 'OTROS ALCOHOLES, FENOLES, ALCOHOLES-FENOL, Y SUS DERIVADOS HALOGENADOS, SULFONADOS, NITRADOS O NITROSADOS; ALCOHOLES GRASOS INDUSTRIALES IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(82,461021,341409, 'ÁCIDOS CARBOXÍLICOS Y SUS ANHÍDRIDOS, HALUROS, PERÓXIDOS Y PEROXIÁCIDOS Y SUS DERIVADOS HALOGENADOS, SULFONADOS, NITRADOS O NITROSADOS, EXCEPTO ÁCIDO SALICÍLICO Y SUS SALES Y ÉSTERES Y SUS SALES IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(83,461021,342339, 'ÁCIDO NÍTRICO, ÁCIDOS SULFONÍTRICOS IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(84,461021,341509, 'COMPUESTOS DE FUNCIÓN AMINO; COMPUESTOS AMINO CON FUNCIÓN OXIGENADA, EXCEPTO LISINA Y SUS ÉSTERES Y SALES DE ESTOS Y ÁCIDO GLUTÁMICO Y SUS SALES; UREÍNAS Y SUS DERIVADOS Y SALES DE ESTOS; COMPUESTOS CON FUNCIÓN CARBOXAMIDA; COMPUESTO IMINA, COMPUESTO FUNCIÓN NITRILO; COMPUESTOS, -DIAZO, -AZO O AZOXI; DERIVADOS ORGÁNICOS DE LA HIDRACINA O DE LA HIDROXILAMINA; COMPUESTOS CON OTRAS FUNCIONES NITROGENADAS IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(85,461021,341709, 'ÉTERES, PERÓXIDOS DE ALCOHOLES, PERÓXIDOS DE ÉTERES, EPÓXIDOS, ACETALES Y HEMIACETALES, Y SUS DERIVADOS HALOGENADOS, SULFONADOS, NITRADOS O NITROSADOS; COMPUESTOS DE FUNCIÓN ALDEHÍDO; COMPUESTOS DE FUNCIÓN CETONA Y COMPUESTOS DE FUNCIÓN QUINONA; ENZIMAS, ENZIMAS PREPARADAS N.C.P., COMPUESTOS ORGÁNICOS N.C.P. IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(86,461021,341809, 'ÉSTERES FOSFÓRICOS Y SUS SALES O ÉSTERES DE OTROS ÁCIDOS INORGÁNICOS (EXCLUYENDO ÉSTERES DE HALUROS DE HIDRÓGENO) Y SUS SALES; Y SUS DERIVADOS HALOGENADOS, SULFONADOS, NITRADOS Y NITROSADOS IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(87,461021,342319, 'ELEMENTOS QUÍMICOS N.C.P.; ÁCIDOS INORGÁNICOS EXCEPTO ÁCIDO FOSFÓRICO Y ÁCIDO SULFONÍTRICO; COMPUESTOS OXIGENADOS INORGÁNICOS DEL BORO, SILICIO Y CARBÓN; COMPUESTOS HALOGENADOS O SULFUROSOS DE METALES; HIDRÓXIDO DE SODIO Y PERÓXIDO DE MAGNESIO; ÓXIDOS, HIDRÓXIDOS Y PERÓXIDOS DE ESTRONCIO O DE BARIO; HIDRÓXIDO DE ALUMINIO; HYDROZINE E HIDROXILAMINA Y SUS SALES INORGÁNICAS IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(88,461021,342329, 'ÁCIDO FOSFÓRICO IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(89,461021,342509, 'SALES DE ÁCIDOS OXOMETÁLICOS O PEROXOMETÁLICOS; METALES PRECIOSOS EN FORMA COLOIDAL Y SUS COMPUESTOS; COMPUESTOS ORGÁNICOS E INORGÁNICOS DE MERCURIO; OTROS QUÍMICOS INORGÁNICOS N.C.P.; AIRE COMPRIMIDO; AMALGAMAS IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(90,461021,342609, 'ISÓTOPOS N.C.P. Y SUS COMPUESTOS (INCLUSO AGUA PESADA) IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(91,461021,342709, 'CIANUROS, OXICIANUROS Y CIANUROS COMPLEJOS; FULMINATOS, CIANATOS Y TIOCIANATOS; SILICATOS; BORATOS; PERBORATOS IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(92,461021,342809, 'PERÓXIDO DE HIDRÓGENO; FOSFUROS; CARBUROS; HIDRUROS, NITRUROS, AZIDAS, SILICIUROS Y BORUROS IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(93,461021,342909, 'COMPUESTOS DE METALES DE TIERRAS RARAS, DE ITRIO O DE ESCANDIO IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(94,461021,344009, 'PRODUCTOS MINERALES NATURALES ACTIVADOS; NEGRO ANIMAL; ACEITE DE RESINA; ACEITES TERPÉNICOS PRODUCIDOS POR TRATAMIENTO DE MADERA DE CONÍFERAS; DIPENTENO CRUDO; PARACIMENO CRUDO; ACEITE DE PINO; COLOFONIA Y ÁCIDOS RESÍNICOS, Y SUS DERIVADOS; ESENCIAS DE COLOFONIA Y ACEITES DE COLOFONIA, ALQUITRÁN DE MADERA; ACEITES DE ALQUITRÁN DE MADERA, CREOSOTA DE MADERA, NAFTA DE MADERA, RESINA VEGETAL IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(95,461021,345309, 'PIRITAS DE HIERRO TOSTADAS IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(96,461021,345409, 'ACEITES Y OTROS PRODUCTOS DE LA DESTILACIÓN DE ALQUITRÁN DE HULLA A ALTA TEMPERATURA, Y PRODUCTOS SIMILARES; BREA Y COQUE DE BREA OBTENIDOS DE ALQUITRANES MINERALES IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(97,461021,352109, 'ÁCIDO SALICÍLICO Y SUS SALES Y ÉSTERES IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(98,461021,352209, 'LISINA Y SUS ÉSTERES Y SALES DE ESTOS COMPUESTOS; ÁCIDO GLUTÁMICO Y SUS SALES; SALES E HIDRÓXIDOS DE AMONIO CUATERNARIO; LECITINAS Y OTROS FOSFOAMINOLÍPIDOS; AMIDAS ACÍCLICAS Y SUS DERIVADOS Y SALES DE ESTOS COMPUESTOS; AMIDAS CÍCLICAS (EXCEPTO UREÍNAS) Y SUS DERIVADOS Y SALES IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(99,461021,352309, 'LACTONAS N.C.P., COMPUESTOS HETEROCÍCLICOS QUE SÓLO CONTENGAN HETEROÁTOMOS DE NITRÓGENO, CON UN ANILLO DE PIRAZOL NO FUSIONADO, UN ANILLO DE PIRIMIDINA, UN ANILLO DE PIPERAZINA, UN ANILLO DE TRIAZINA NO FUSIONADO O UN SISTEMA DE ANILLOS DE FENOTIAZINA SIN OTRO TIPO DE FUSIÓN, HIDANTOÍNA Y SUS DERIVADOS; SULFONAMIDAS IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(100,461021,352409, 'AZÚCARES QUÍMICAMENTE PUROS N.C.P.; ÉTERES Y ÉSTERES DE AZÚCARES Y SUS SALES N.C.P. IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(101,461021,352509, 'PROVITAMINAS, VITAMINAS Y HORMONAS; GLUCÓSIDOS Y ALCALOIDES VEGETALES Y SUS SALES, ÉTERES, ÉSTERES Y OTROS DERIVADOS; ANTIBIÓTICOS IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(102,461021,352609, 'MEDICAMENTOS PARA USOS TERAPÉUTICOS O PROFILÁCTICOS IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(103,461021,352709, 'OTROS PRODUCTOS FARMACÉUTICOS IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(104,461021,352909, 'OTROS ARTÍCULOS PARA FINES MÉDICOS O QUIRÚRGICOS IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(105,461021,354709, 'ELEMENTOS Y COMPUESTOS QUÍMICOS CON ADITIVOS PARA SU USO EN ELECTRÓNICA IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(106,461021,354919, 'BIODIESEL IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(107,461021,354999, 'OTROS PRODUCTOS QUÍMICOS N.C.P. IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(108,461021,612719, 'SERVICIOS DE COMERCIO AL POR MAYOR DE PRODUCTOS QUÍMICOS INDUSTRIALES BÁSICOS Y RESINAS SINTÉTICAS, PRESTADOS A COMISIÓN O POR CONTRATO IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(109,461021,612729, 'SERVICIOS DE COMERCIO AL POR MAYOR DE PRODUCTOS FERTILIZANTES Y AGROQUÍMICOS, PRESTADOS A COMISIÓN O POR CONTRATO IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(110,461021,612739, 'SERVICIOS DE COMERCIO AL POR MAYOR DE PRODUCTOS FARMACÉUTICOS, PRESTADOS A COMISIÓN O POR CONTRATO IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(111,461021,612929, 'SERVICIOS DE COMERCIO AL POR MAYOR DE MINERALES METALÍFEROS Y METALES EN FORMAS PRIMARIAS, PRESTADOS A COMISIÓN O POR CONTRATO IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(112,461021,612999, 'SERVICIOS DE COMERCIO AL POR MAYOR DE MINERALES NO METÁLICOS Y OTROS PRODUCTOS N.C.P., PRESTADOS A COMISIÓN O POR CONTRATO IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(113,461021,991009, 'OTROS PRODUCTOS O SERVICIOS ALCANZADOS POR EL IVA IMPORTADO', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(114,461021,99866, 'VENTA DE BIENES A TRAVÉS DE COMISIONISTAS O CONSIGNATARIOS', true);
insert into categoria (codigocategoria, codigoactividad, codigoproductosin,descripcionproducto,activo) values(115,461021,99867, 'PRESTACIÓN DE SERVICIOS A TRAVÉS DE COMISIONISTAS O CONSIGNATARIOS', true);

--Insert Data into Table Producto
insert into producto (codigoproducto,nombreproducto,tipoproducto,precio,codigounidadmedida,codigocategoria,activo) values
('1', 'Paquete 1','P',500.00,43,1,true);
insert into producto (codigoproducto, nombreproducto,tipoproducto,precio,codigounidadmedida,codigocategoria,activo) values
('2', 'Paquete 2','P',600.00,43,1,true);
insert into producto (codigoproducto, nombreproducto,tipoproducto,precio,codigounidadmedida,codigocategoria,activo) values
('3', 'Paquete 3','P',200.00,43,1,true);

--Insert Data into Table Parametros
-- UNIDADESMEDIDA
insert into parametros values ('1','BOBINAS','UNIDADES',true);
insert into parametros values ('2','BALDE','UNIDADES',true);
insert into parametros values ('3','BARRILES','UNIDADES',true);
insert into parametros values ('4','BOLSA','UNIDADES',true);
insert into parametros values ('5','BOTELLAS','UNIDADES',true);
insert into parametros values ('6','CAJA','UNIDADES',true);
insert into parametros values ('7','CARTONES','UNIDADES',true);
insert into parametros values ('8','CENTIMETRO CUADRADO','UNIDADES',true);
insert into parametros values ('9','CENTIMETRO CUBICO','UNIDADES',true);
insert into parametros values ('10','CENTIMETRO LINEAL','UNIDADES',true);
insert into parametros values ('11','CIENTO DE UNIDADES','UNIDADES',true);
insert into parametros values ('12','CILINDRO','UNIDADES',true);
insert into parametros values ('13','CONOS','UNIDADES',true);
insert into parametros values ('14','DOCENA','UNIDADES',true);
insert into parametros values ('15','FARDO','UNIDADES',true);
insert into parametros values ('16','GALON INGLES','UNIDADES',true);
insert into parametros values ('17','GRAMO','UNIDADES',true);
insert into parametros values ('18','GRUESA','UNIDADES',true);
insert into parametros values ('19','HECTOLITRO','UNIDADES',true);
insert into parametros values ('20','HOJA','UNIDADES',true);
insert into parametros values ('21','JUEGO','UNIDADES',true);
insert into parametros values ('22','KILOGRAMO','UNIDADES',true);
insert into parametros values ('23','KILOMETRO','UNIDADES',true);
insert into parametros values ('24','KILOVATIO HORA','UNIDADES',true);
insert into parametros values ('25','KIT','UNIDADES',true);
insert into parametros values ('26','LATAS','UNIDADES',true);
insert into parametros values ('27','LIBRAS','UNIDADES',true);
insert into parametros values ('28','LITRO','UNIDADES',true);
insert into parametros values ('29','MEGAWATT HORA','UNIDADES',true);
insert into parametros values ('30','METRO','UNIDADES',true);
insert into parametros values ('31','METRO CUADRADO','UNIDADES',true);
insert into parametros values ('32','METRO CUBICO','UNIDADES',true);
insert into parametros values ('33','MILIGRAMOS','UNIDADES',true);
insert into parametros values ('34','MILILITRO','UNIDADES',true);
insert into parametros values ('35','MILIMETRO','UNIDADES',true);
insert into parametros values ('36','MILIMETRO CUADRADO','UNIDADES',true);
insert into parametros values ('37','MILIMETRO CUBICO','UNIDADES',true);
insert into parametros values ('38','MILLARES','UNIDADES',true);
insert into parametros values ('39','MILLON DE UNIDADES','UNIDADES',true);
insert into parametros values ('40','ONZAS','UNIDADES',true);
insert into parametros values ('41','PALETAS','UNIDADES',true);
insert into parametros values ('42','PAQUETE','UNIDADES',true);
insert into parametros values ('43','PAR','UNIDADES',true);
insert into parametros values ('44','PIES','UNIDADES',true);
insert into parametros values ('45','PIES CUADRADOS','UNIDADES',true);
insert into parametros values ('46','PIES CUBOS','UNIDADES',true);
insert into parametros values ('47','PIEZAS','UNIDADES',true);
insert into parametros values ('48','PLACAS','UNIDADES',true);
insert into parametros values ('49','PLIEGO','UNIDADES',true);
insert into parametros values ('50','PULGADAS','UNIDADES',true);
insert into parametros values ('51','RESMA','UNIDADES',true);
insert into parametros values ('52','TAMBOR','UNIDADES',true);
insert into parametros values ('53','TONELADA CORTA','UNIDADES',true);
insert into parametros values ('54','TONELADA LARGA','UNIDADES',true);
insert into parametros values ('55','TONELADAS','UNIDADES',true);
insert into parametros values ('56','TUBOS','UNIDADES',true);
insert into parametros values ('57','UNIDAD (BIENES)','UNIDADES',true);
insert into parametros values ('58','UNIDAD (SERVICIOS)','UNIDADES',true);
insert into parametros values ('59','US GALON','UNIDADES',true);
insert into parametros values ('60','YARDA','UNIDADES',true);
insert into parametros values ('61','YARDA CUADRADA','UNIDADES',true);
insert into parametros values ('62','OTRO','UNIDADES',true);
insert into parametros values ('63','ONZA TROY','UNIDADES',true);
insert into parametros values ('64','LIBRA FINA','UNIDADES',true);
insert into parametros values ('65','DISPLAY','UNIDADES',true);
insert into parametros values ('66','BULTO','UNIDADES',true);
insert into parametros values ('67','DIAS','UNIDADES',true);
insert into parametros values ('68','MESES','UNIDADES',true);
insert into parametros values ('69','QUINTAL','UNIDADES',true);
insert into parametros values ('70','ROLLO','UNIDADES',true);
insert into parametros values ('71','HORAS','UNIDADES',true);
insert into parametros values ('72','AGUJA','UNIDADES',true);
insert into parametros values ('73','AMPOLLA','UNIDADES',true);
insert into parametros values ('74','BIDÓN','UNIDADES',true);
insert into parametros values ('75','BOLSA','UNIDADES',true);
insert into parametros values ('76','CAPSULA','UNIDADES',true);
insert into parametros values ('77','CARTUCHO','UNIDADES',true);
insert into parametros values ('78','COMPRIMIDO','UNIDADES',true);
insert into parametros values ('79','ESTUCHE','UNIDADES',true);
insert into parametros values ('80','FRASCO','UNIDADES',true);
insert into parametros values ('81','JERINGA','UNIDADES',true);
insert into parametros values ('82','MINI BOTELLA','UNIDADES',true);
insert into parametros values ('83','SACHET','UNIDADES',true);
insert into parametros values ('84','TABLETA','UNIDADES',true);
insert into parametros values ('85','TERMO','UNIDADES',true);
insert into parametros values ('86','TUBO','UNIDADES',true);
insert into parametros values ('87','BARRIL (EEUU) 60 F','UNIDADES',true);
insert into parametros values ('88','BARRIL [42 GALONES(EEUU)]','UNIDADES',true);
insert into parametros values ('89','METRO CUBICO 68F VOL','UNIDADES',true);
insert into parametros values ('90','MIL PIES CUBICOS 14696 PSI','UNIDADES',true);
insert into parametros values ('91','MIL PIES CUBICOS 14696 PSI 68FAH','UNIDADES',true);
insert into parametros values ('92','MILLAR DE PIES CUBICOS (1000 PC)','UNIDADES',true);
insert into parametros values ('93','MILLONES DE PIES CUBICOS (1000000 PC)','UNIDADES',true);
insert into parametros values ('94','MILLONES DE BTU (1000000 BTU)','UNIDADES',true);
insert into parametros values ('95','UNIDAD TERMICA BRITANICA (TI)','UNIDADES',true);
insert into parametros values ('96','POMO','UNIDADES',true);
insert into parametros values ('97','VASO','UNIDADES',true);
insert into parametros values ('98','TETRAPACK','UNIDADES',true);
insert into parametros values ('99','CARTOLA','UNIDADES',true);
insert into parametros values ('100','JABA','UNIDADES',true);
insert into parametros values ('101','YARDA','UNIDADES',true);
insert into parametros values ('102','BANDEJA','UNIDADES',true);
insert into parametros values ('103','TURRIL','UNIDADES',true);
insert into parametros values ('104','BLISTER','UNIDADES',true);
insert into parametros values ('105','TIRA','UNIDADES',true);
insert into parametros values ('106','MEGAWATT','UNIDADES',true);
insert into parametros values ('107','KILOWATT','UNIDADES',true);
insert into parametros values ('108','AMORTIZACION','UNIDADES',true);
insert into parametros values ('109','OVULOS','UNIDADES',true);
insert into parametros values ('110','SUPOSITORIOS','UNIDADES',true);
insert into parametros values ('111','SOBRES','UNIDADES',true);
insert into parametros values ('112','VIAL','UNIDADES',true);
insert into parametros values ('113','HECTAREAS','UNIDADES',true);
insert into parametros values ('114','ARROBA','UNIDADES',true);
insert into parametros values ('115','AEROSOL','UNIDADES',true);
insert into parametros values ('116','BARRA','UNIDADES',true);
insert into parametros values ('117','CONJUNTO','UNIDADES',true);
insert into parametros values ('118','FANEGA','UNIDADES',true);
insert into parametros values ('119','PACK','UNIDADES',true);
insert into parametros values ('120','PIPETA','UNIDADES',true);
insert into parametros values ('121','POTE','UNIDADES',true);
insert into parametros values ('122','PASTILLA','UNIDADES',true);
insert into parametros values ('123','TONELADA METRICA','UNIDADES',true);
insert into parametros values ('124','EQUIPOS','UNIDADES',true);

-- MOTIVOSANULACION
insert into parametros values ('1','FACTURA MAL EMITIDA','MOTIVOS',true);
insert into parametros values ('2','NOTA DE CREDITO-DEBITO MAL EMITIDA','MOTIVOS',true);
insert into parametros values ('3','DATOS DE EMISION INCORRECTOS','MOTIVOS',true);
insert into parametros values ('4','FACTURA O NOTA DE CREDITO-DEBITO DEVUELTA','MOTIVOS',true);
 
-- EVENTOSSIGNIFICATIVOS
insert into parametros values ('1','CORTE DEL SERVICIO DE INTERNET','EVENTOS',true);
insert into parametros values ('2','INACCESIBILIDAD AL SERVICIO WEB DE LA ADMINISTRACIÓN TRIBUTARIA','EVENTOS',true);
insert into parametros values ('3','INGRESO A ZONAS SIN INTERNET POR DESPLIEGUE DE PUNTOS DE VENTA','EVENTOS',true);
insert into parametros values ('4','VENTA EN LUGARES SIN INTERNET','EVENTOS',true);
insert into parametros values ('5','VIRUS INFORMÁTICO O FALLA DE SOFTWARE','EVENTOS',true);
insert into parametros values ('6','CAMBIO DE INFRAESTRUCTURA DE SISTEMA O FALLA DE HARDWARE','EVENTOS',true);
insert into parametros values ('7','CORTE DE SUMINISTRO DE ENERGÍA ELÉCTRICA','EVENTOS',true);

--Insert Data into Table Sucursal
insert into sucursal (codigosucursal, nombresucursal, direccion,barrio,municipio,telefono,activo) values
(0, 'Casa Matriz', 'Calle Suipacha N 484', 'Barrio Las Panosas','Cercado','46643010',true);
insert into sucursal (codigosucursal, nombresucursal, direccion,barrio,municipio,telefono,activo) values
(1, 'Oficina La Loma', 'Calle Ecuador #43', 'La Loma','Cercado','46698203',true);
insert into sucursal (codigosucursal, nombresucursal, direccion,barrio,municipio,telefono,activo) values
(2, 'Oficina Campesino', 'Av. Panamericana #18', 'Defensores del Chaco','Cercado','46682717',true);

--Insert Data into Table PuntoVenta
insert into puntoventa (codigopuntoventa, nombrepuntoventa, tipopuntoventa,codigosucursal,activo) values
(0, 'Plataforma', '1', 0,true);
insert into puntoventa (codigopuntoventa, nombrepuntoventa, tipopuntoventa,codigosucursal,activo) values
(1, 'Dpto. Sistemas', '1', 0,true);

--Insert Data into Table Rol
insert into rol (nombrerol, descripcion,activo) values
('Administrador', 'ADM',true);
insert into rol (nombrerol, descripcion,activo) values
('Cajero', 'CAJ',true);

--Insert Data into Table Usuario
insert into usuario (nombreusuario, ci, nombres,ap,am,telefono,clave,codigorol,activo) values
('admin', '7120750', 'Francisco', 'Lopez','Perez','78243122','G/q3OTn0B2W3Hy7CTmgBOIwt3Ua9CRAhEbYYlsswYs4=                    ',1,true);

--Insert Data into Tabla UsuarioPuntoVenta
insert into usuariopuntoventa (nombreusuario, codigopuntoventa, activo) values ('admin', 0, true); 

--Insert Data into Table Enlace
insert into enlaces (nombreenlace, ruta,activo) values
('Usuarios', '/administrar/usuarios',true);
insert into enlaces (nombreenlace, ruta,activo) values
('Roles', '/administrar/roles',true);
insert into enlaces (nombreenlace, ruta,activo) values
('Unidades', '/general/unidades',true);
insert into enlaces ( nombreenlace, ruta,activo) values
('Categorias', '/general/categorias',true);
insert into enlaces ( nombreenlace, ruta,activo) values
('Datos de la empresa', '/general/empresa',true);
insert into enlaces (nombreenlace, ruta,activo) values
('Productos', '/productos/gestion',true);
insert into enlaces (nombreenlace, ruta,activo) values
('Clientes', '/clientes/gestion',true);
insert into enlaces (nombreenlace, ruta,activo) values
('Gestion de facturas', '/facturacion/gestion',true);
insert into enlaces (nombreenlace, ruta,activo) values
('Facturas Pendientes', '/facturacion/pendientes',true);
insert into enlaces (nombreenlace, ruta,activo) values
('Individual', '/facturacion/individual',true);
insert into enlaces (nombreenlace, ruta,activo) values
('Masiva', '/facturacion/masiva',true);
insert into enlaces (nombreenlace, ruta,activo) values
('Eventos significativos', '/eventos/gestion',true);
insert into enlaces (nombreenlace, ruta,activo) values
('Certificado', '/general/certificado',true);
insert into enlaces (nombreenlace, ruta,activo) values
('Sucursales', '/general/sucursales',true);
insert into enlaces (nombreenlace, ruta,activo) values
('Puntos de Venta', '/general/puntos_de_venta',true);

--Insert Data into Table Permisos
insert into permisos (codigorol, codigoenlace,activo) values (1, 1,true);
insert into permisos (codigorol, codigoenlace,activo) values (1, 2,true);
insert into permisos (codigorol, codigoenlace,activo) values (1, 3,true);
insert into permisos (codigorol, codigoenlace,activo) values (1, 4,true);
insert into permisos (codigorol, codigoenlace,activo) values (1, 5,true);
insert into permisos (codigorol, codigoenlace,activo) values (1, 6,true);
insert into permisos (codigorol, codigoenlace,activo) values (1, 7,true);
insert into permisos (codigorol, codigoenlace,activo) values (1, 8,true);
insert into permisos (codigorol, codigoenlace,activo) values (1, 9,true);
insert into permisos (codigorol, codigoenlace,activo) values (1, 10,true);
insert into permisos (codigorol, codigoenlace,activo) values (1, 11,true);
insert into permisos (codigorol, codigoenlace,activo) values (1, 12,true);
insert into permisos (codigorol, codigoenlace,activo) values (1, 13,true);
insert into permisos (codigorol, codigoenlace,activo) values (1, 14,true);
insert into permisos (codigorol, codigoenlace,activo) values (1, 15,true);

--Insert Data into Table General
insert into general (codigopuntoventa, codigosistema, nit, nombreempresa, direccion, telefono, ciudad, codigoautorizacion) values 
(0, '723755F6B4AE27F5D61A57E', '1024061022', 'COOPERATIVA DE SERVICIOS PUBLICOS DE TELECOMUNICACIONES DE TARIJA R.L. COSETT R. L.', 'Calle Suipacha N 484 Barrio Las Panosas', 46643010, 'Tarija', '4611B070E006877193424714631EFC63CF5C9E06A1844DA7F0FB2FD74');

-- SINCRONIZACION CLIENTES Y TELEFONOS CLIENTES
-- Actualizar la direccion a la ubicacion, el archivo se encuentra en el proyecto
COPY cliente FROM 'SisFelBD\SincronizacionClientes\Archivos\SociosBDCosett.csv' DELIMITER ',' CSV;
update cliente set ci = 'NO REGISTRADO' where ci is null OR ci = '                    ';

-- Es necesario actualizar manualmente a 1 el primer codigocliente por un error extraño 

-- Actualizar la direccion a la ubicacion, el archivo se encuentra en el proyecto
COPY telefonocliente FROM 'D:\Documents\Francisco\SisFel\Documentacion\KardexBDCosettNit.csv' DELIMITER ',' CSV;
COPY telefonocliente FROM 'D:\Documents\Francisco\SisFel\Documentacion\KardexBDCosettCi.csv' DELIMITER ',' CSV;
update telefonocliente set complemento = '' where complemento is null;
update telefonocliente set email = '' where email is null;



