USE [GD1C2015]
GO
/****** Object:  StoredProcedure [QUIEN_BAJO_EL_KERNEL].[GenerarFactura]    Script Date: 07/16/2015 21:05:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [QUIEN_BAJO_EL_KERNEL].[GenerarFactura]
@fecha datetime,
@cliente_numero_doc numeric(10,0),
@cliente_tipo_doc numeric(10,0)
AS
BEGIN
	SET NOCOUNT ON;
	declare @numero_cuenta numeric(18,0);
	
	SELECT @numero_cuenta = MAX(numero) + 1
	FROM QUIEN_BAJO_EL_KERNEL.FACTURA
	
	INSERT INTO QUIEN_BAJO_EL_KERNEL.FACTURA
	(numero, fecha, cliente_numero_doc, cliente_tipo_doc)
	values
	(@numero_cuenta, @fecha, @cliente_numero_doc, @cliente_tipo_doc)
	
	select @numero_cuenta
END
