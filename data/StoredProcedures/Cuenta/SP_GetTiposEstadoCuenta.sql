CREATE PROCEDURE [QUIEN_BAJO_EL_KERNEL].[GetTiposEstadoCuenta]
AS
BEGIN
	SELECT * FROM QUIEN_BAJO_EL_KERNEL.TIPO_ESTADO_CUENTA t ORDER BY t.codigo ASC;
END
GO