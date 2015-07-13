USE [GD1C2015]
GO
/****** Object:  StoredProcedure [QUIEN_BAJO_EL_KERNEL].[InsertaCuenta]    Script Date: 07/11/2015 23:19:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [QUIEN_BAJO_EL_KERNEL].[InsertaCuenta]
@an_cod_pais			NUMERIC(18,0),
@an_moneda_tipo			NUMERIC(1,0),
@an_cuenta_tipo			NUMERIC(1,0),
@an_cliente_doc			NUMERIC(10,0),
@an_cliente_tipo_doc	NUMERIC(18,0)
AS
BEGIN
DECLARE @an_num_cuenta	NUMERIC(18,0)
	SET NOCOUNT ON;
	
	SELECT @an_num_cuenta = MAX(numero) + 1
	  FROM QUIEN_BAJO_EL_KERNEL.CUENTA c

	INSERT INTO QUIEN_BAJO_EL_KERNEL.CUENTA([numero],
											[pais_codigo],
											[moneda_tipo],
											[tipo_cuenta],
											[cliente_numero_doc],
											[cliente_tipo_doc],
											[fecha_creacion],
											[saldo],
											[estado_codigo])
									 VALUES (@an_num_cuenta,
											 @an_cod_pais,
											 @an_moneda_tipo, 
											 @an_cuenta_tipo, 
											 @an_cliente_doc, 
											 @an_cliente_tipo_doc,
											 GETDATE(),
											 0,--Saldo
											 1)--Pendiente de activacion

END
