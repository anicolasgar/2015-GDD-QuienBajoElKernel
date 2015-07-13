USE [GD1C2015]
GO
/****** Object:  StoredProcedure [QUIEN_BAJO_EL_KERNEL].[CerrarCuenta]    Script Date: 07/12/2015 03:01:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [QUIEN_BAJO_EL_KERNEL].[CerrarCuenta]
@an_nro_cuenta	NUMERIC(18,0)
AS
BEGIN
DECLARE @items_a_facturar	NUMERIC(18,0),
		@items_facturados	NUMERIC(18,0)
	--Primero verifico que tenga pago todos los costos de las transacciones
	--Hacer un count de modificaciones de cuenta + activacion + transferencias
	--y si la cantidad es igual a los items de factura, se puede inhabilitar
	SELECT @items_a_facturar = (SELECT COUNT(*)
								  FROM QUIEN_BAJO_EL_KERNEL.TRANSFERENCIA t
								 WHERE t.origen = @an_nro_cuenta)
								+
							   (SELECT COUNT(*)
							      FROM QUIEN_BAJO_EL_KERNEL.CUENTA c
							     WHERE c.numero = @an_nro_cuenta)
							    +
							   (SELECT COUNT(*)
							      FROM QUIEN_BAJO_EL_KERNEL.CUENTA_MODIFICACION cm
							     WHERE cm.cuenta = @an_nro_cuenta)
							     
	SELECT @items_facturados = (SELECT COUNT(*)
								  FROM QUIEN_BAJO_EL_KERNEL.ITEM_FACTURA_ACTIVACION_CUENTA ia
								 WHERE ia.cuenta = @an_nro_cuenta)
								+
							   (SELECT COUNT(*)
								  FROM QUIEN_BAJO_EL_KERNEL.ITEM_FACTURA_MODIFICACION_CUENTA im,
									   QUIEN_BAJO_EL_KERNEL.CUENTA_MODIFICACION c
								 WHERE im.id_modificacion = c.id_modificacion
								   AND c.cuenta = @an_nro_cuenta)
								+
							   (SELECT COUNT(*)
								  FROM QUIEN_BAJO_EL_KERNEL.ITEM_FACTURA_TRANSFERENCIAS it,
									   QUIEN_BAJO_EL_KERNEL.TRANSFERENCIA t
								 WHERE it.transferencia = t.codigo
								   AND t.origen = @an_nro_cuenta)
								   
	IF @items_a_facturar <> @items_facturados
		--No pago todas las transacciones
		RETURN -1
	ELSE
		BEGIN
		--Ya pago todas las transacciones, se puede cerrar la cuenta
		UPDATE QUIEN_BAJO_EL_KERNEL.CUENTA
		   SET fecha_cierre = GETDATE(),
			   estado_codigo = 2
		 WHERE numero = @an_nro_cuenta
		END
	
END