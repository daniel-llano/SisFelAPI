-- Sincronizando tabla cliente
-- Select  a tabla dbo.T_Socio a exportar
SELECT [IdSocio]
      ,[Legal]
      ,[CI]
      ,[TipoPers]
      ,'true'
  FROM [Accion2011].[dbo].[T_Socio]

-- Copiando datos de tabla dbo.T_Socio a tabla cliente
COPY cliente FROM 'D:\Documents\Francisco\SisFel\Documentacion\SociosBDCosett.csv' DELIMITER ',' CSV;

-- Observaciones
-- Es necesario actualizar manualmente el primer codigocliente por un error extraño
update cliente set ci = 'NO REGISTRADO' where ci is null OR ci = '                    ';

-- Agregando clientes con tipodocumento 5 nit
-- Select en tabla dbo.T.kardex
SELECT [NroKardex]
		,[IdSocio]
		,5 as codigotipodocumento
      ,[RUC]
	  ,0 as ci
	  ,'' as complemento
	  ,[NombreFact]
	  ,'' as email
	  ,[NroTelef]
	  ,'true' as activo
  FROM [Accion2011].[dbo].[T_Kardex] where LEN(RUC) > 7

-- Copiando datos de tabla dbo.T_kardex a tabla telfonocliente
COPY telefonocliente FROM 'D:\Documents\Francisco\SisFel\Documentacion\KardexBDCosettNit.csv' DELIMITER ',' CSV;
COPY telefonocliente FROM 'D:\Documents\Francisco\SisFel\Documentacion\KardexBDCosettCi.csv' DELIMITER ',' CSV;
update telefonocliente set complemento = '' where complemento is null;
update telefonocliente set email = '' where email is null;

-- Observaciones
-- No existe el idSocio 28763
-- 36438,28763,1,0,"0                   ",,S/N,,6654415,true
