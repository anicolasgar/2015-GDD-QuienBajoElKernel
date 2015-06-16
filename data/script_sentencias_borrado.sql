
DECLARE @SqlStatement VARCHAR(MAX)
SELECT @SqlStatement = 
    COALESCE(@SqlStatement, '') + 'DROP TABLE [QUIEN_BAJO_EL_KERNEL].' + QUOTENAME(TABLE_NAME) + ';' + CHAR(13)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'QUIEN_BAJO_EL_KERNEL'

PRINT @SqlStatement

select   'ALTER TABLE QUIEN_BAJO_EL_KERNEL.' + t.name + ' drop constraint ' + 
OBJECT_NAME(d.constraint_object_id)  + ';' + CHAR(13)
from sys.tables t 
    join sys.foreign_key_columns d on d.parent_object_id = t.object_id 
    inner join sys.schemas s on t.schema_id = s.schema_id
where s.name = 'QUIEN_BAJO_EL_KERNEL'
ORDER BY t.name;


select 'DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.'+t.name from sys.procedures t


ALTER TABLE QUIEN_BAJO_EL_KERNEL.CLIENTE DROP CONSTRAINT FK_CLIENTE_USUARIO;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.USUARIO_LOG DROP CONSTRAINT FK_USUARIO_LOG_USUARIO ;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.FUNCIONALIDAD_ROL drop constraint FK_FUNCIONALIDADROL_FUNCIONALIDAD; 

ALTER TABLE QUIEN_BAJO_EL_KERNEL.ITEM_FACTURA DROP CONSTRAINT FK_ITEM_FACTURA_FACTURA;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.ITEM_FACTURA DROP CONSTRAINT FK_ITEM_FACTURA_TRANSACCIONES;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.FACTURA DROP CONSTRAINT FK_FACTURA_CLIENTE;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.CUENTA_MODIFICACION DROP CONSTRAINT FK_CUENTA_MODIFICACION_CUENTA;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.CUENTA_MODIFICACION DROP CONSTRAINT FK_CUENTA_MODIFICACION_TRANSACCIONES;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.FUNCIONALIDAD_ROL DROP CONSTRAINT FK_FUNCIONALIDAD_ROL_ROL;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.USUARIO_ROL DROP CONSTRAINT FK_USUARIO_ROL_ROL;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.USUARIO_ROL DROP CONSTRAINT FK_USUARIO_ROL_USUARIO;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.TRANSACCIONES DROP CONSTRAINT FK_TRANSACCIONES_TIPO_TRANSACCION;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.CHEQUE DROP CONSTRAINT FK_CHEQUE_BANCO;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.CHEQUE DROP CONSTRAINT FK_CHEQUE_TIPO_MONEDA;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.RETIRO DROP CONSTRAINT FK_RETIRO_CHEQUE;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.RETIRO DROP CONSTRAINT FK_RETIRO_CUENTA;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.TRANSFERENCIA DROP CONSTRAINT FK_TRANSFERENCIA_CUENTA;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.TRANSFERENCIA DROP CONSTRAINT FK_TRANSFERENCIA_CUENTA_DESTINO;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.TRANSFERENCIA DROP CONSTRAINT FK_TRANSFERENCIA_TIPO_MONEDA;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.TRANSFERENCIA DROP CONSTRAINT FK_TRANSFERENCIA_TRANSACCIONES;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.DEPOSITO DROP CONSTRAINT FK_DEPOSITO_CUENTA;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.DEPOSITO DROP CONSTRAINT FK_DEPOSITO_TARJETA;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.DEPOSITO DROP CONSTRAINT FK_DEPOSITO_TIPO_MONEDA;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.CLIENTE DROP CONSTRAINT FK_CLIENTE_PAIS;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.CLIENTE DROP CONSTRAINT FK_CLIENTE_TIPO_DOCUMENTO;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.CUENTA DROP CONSTRAINT FK_CUENTA_CLIENTE;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.CUENTA DROP CONSTRAINT FK_CUENTA_PAIS;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.CUENTA DROP CONSTRAINT FK_CUENTA_TIPO_CUENTA;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.CUENTA DROP CONSTRAINT FK_CUENTA_TIPO_ESTADO_CUENTA;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.CUENTA DROP CONSTRAINT FK_CUENTA_TRANSACCIONES;



ALTER TABLE QUIEN_BAJO_EL_KERNEL.ITEM_FACTURA DROP CONSTRAINT PK_ITEM_FACTURA;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.FACTURA DROP CONSTRAINT PK_FACTURA;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.CUENTA_MODIFICACION DROP CONSTRAINT PK_CUENTA_MODIFICACION;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.FUNCIONALIDAD_ROL DROP CONSTRAINT PK_FUNCIONALIDAD_ROL;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.USUARIO_ROL DROP CONSTRAINT PK_USUARIO_ROL;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.ROL DROP CONSTRAINT PK_ROL;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.USUARIO DROP CONSTRAINT PK_USUARIO;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.TIPO_CUENTA DROP CONSTRAINT PK_TIPO_CUENTA;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.TIPO_ESTADO_CUENTA DROP CONSTRAINT PK_TIPO_ESTADO_CUENTA;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.TIPO_TRANSACCION DROP CONSTRAINT PK_TIPO_TRANSACCION;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.TRANSACCIONES DROP CONSTRAINT PK_TRANSACCIONES;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.TIPO_MONEDA DROP CONSTRAINT PK_TIPO_MONEDA;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.TIPO_DOCUMENTO DROP CONSTRAINT PK_TIPO_DOCUMENTO;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.PAIS DROP CONSTRAINT PK_PAIS;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.BANCO DROP CONSTRAINT PK_BANCO;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.CHEQUE DROP CONSTRAINT PK_CHEQUE;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.RETIRO DROP CONSTRAINT PK_RETIRO;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.TRANSFERENCIA DROP CONSTRAINT PK_TRANSFERENCIA;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.TARJETA DROP CONSTRAINT PK_TARJETA;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.DEPOSITO DROP CONSTRAINT PK_DEPOSITO;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.CLIENTE DROP CONSTRAINT PK_CLIENTE;
ALTER TABLE QUIEN_BAJO_EL_KERNEL.CUENTA DROP CONSTRAINT PK_CUENTA;



DROP TABLE QUIEN_BAJO_EL_KERNEL.ITEM_FACTURA ;
DROP TABLE QUIEN_BAJO_EL_KERNEL.FACTURA ;
DROP TABLE QUIEN_BAJO_EL_KERNEL.CUENTA_MODIFICACION;
DROP TABLE QUIEN_BAJO_EL_KERNEL.FUNCIONALIDAD_ROL;
DROP TABLE QUIEN_BAJO_EL_KERNEL.USUARIO_ROL;
DROP TABLE QUIEN_BAJO_EL_KERNEL.ROL;
DROP TABLE QUIEN_BAJO_EL_KERNEL.USUARIO;
DROP TABLE QUIEN_BAJO_EL_KERNEL.TIPO_CUENTA;
DROP TABLE QUIEN_BAJO_EL_KERNEL.TIPO_ESTADO_CUENTA;
DROP TABLE QUIEN_BAJO_EL_KERNEL.TIPO_TRANSACCION;
DROP TABLE QUIEN_BAJO_EL_KERNEL.TRANSACCIONES;
DROP TABLE QUIEN_BAJO_EL_KERNEL.TIPO_MONEDA;
DROP TABLE QUIEN_BAJO_EL_KERNEL.TIPO_DOCUMENTO;
DROP TABLE QUIEN_BAJO_EL_KERNEL.PAIS ;
DROP TABLE QUIEN_BAJO_EL_KERNEL.BANCO ;
DROP TABLE QUIEN_BAJO_EL_KERNEL.CHEQUE;
DROP TABLE QUIEN_BAJO_EL_KERNEL.RETIRO;
DROP TABLE QUIEN_BAJO_EL_KERNEL.TRANSFERENCIA;
DROP TABLE QUIEN_BAJO_EL_KERNEL.TARJETA;
DROP TABLE QUIEN_BAJO_EL_KERNEL.DEPOSITO;
DROP TABLE QUIEN_BAJO_EL_KERNEL.CLIENTE;
DROP TABLE QUIEN_BAJO_EL_KERNEL.CUENTA;
DROP TABLE [QUIEN_BAJO_EL_KERNEL].[FUNCIONALIDAD];DROP TABLE [QUIEN_BAJO_EL_KERNEL].[USUARIO_LOG];


DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.completar_transacciones;
DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.GetUsuarioByUsernameAndPassword;
DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.SELECT_ROL;
DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.INSERT_ROL;
DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.INSERT_ROL_FUNCIONALIDAD;
DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.GetRolesByUsername;
DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.GetFuncionalidadesByRol;
DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.SELECT_FUNCIONALIDAD;
DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.GetMaxNroCuenta;
DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.GetTipoMoneda;
DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.GetPaises;
DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.GetTiposCuenta;

DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.ClientesConCuentasInhabilitadas ;
DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.ClientesConMayorTransacciones;
DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.ClientesComisionesFacturadas ;
DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.PaisesConMayorIngresosEgresos ;
DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.TotalFacturadoPorCuentas ;

DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.getUltimasDiezTransferenciasByCuenta;
DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.getUltimosCincoDepositosByCuenta;
DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.getUltimosCincoRetirosByCuenta;
DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.GetTiposMonedaByCuenta;
DROP PROCEDURE QUIEN_BAJO_EL_KERNEL.insertTransferencia;

DROP VIEW [QUIEN_BAJO_EL_KERNEL].[TransaccionesClientes];DROP VIEW [QUIEN_BAJO_EL_KERNEL].[MovimientosPaises];


DROP SCHEMA QUIEN_BAJO_EL_KERNEL;