﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PagoElectronico.Services;
using PagoElectronico.Entities;

namespace PagoElectronico.ABM_Cuenta
{
    public partial class ConsultaCuenta : Form
    {
        CuentaService cuentaService { get; set; }
        PaisService paisService { get; set; }
        TipoMonedaService tipoMonedaService { get; set; }
        TipoEstadoCuentaService tipoEstadoService { get; set; }
        ClienteService clienteService { get; set; }
        TransaccionService transaccionService { get; set; }
        Usuario usuario = Session.Usuario;

        public ConsultaCuenta()
        {
            cuentaService = new CuentaService();
            paisService = new PaisService();
            tipoMonedaService = new TipoMonedaService();
            tipoEstadoService = new TipoEstadoCuentaService();
            clienteService = new ClienteService();
            transaccionService = new TransaccionService();
            InitializeComponent();
        }


        private void cargarCampos()
        {
            var monedas = new List<TipoMoneda>();
            monedas.Add(new TipoMoneda { codigo = 0, descripcion = "Seleccionar" });
            monedas.AddRange(tipoMonedaService.GetAll());

            var tiposCuenta = new List<TipoCuenta>();
            tiposCuenta.Add(new TipoCuenta { codigo = 0, descripcion = "Seleccionar" });
            tiposCuenta.AddRange(cuentaService.GetTiposCuenta());

            var tiposEstado = new List<TipoEstadoCuenta>();
            tiposEstado.Add(new TipoEstadoCuenta { codigo = 0, descripcion = "Seleccionar" });
            tiposEstado.AddRange(tipoEstadoService.GetAll());

            var paises = new List<Pais>();
            paises.Add(new Pais { codigoPais = 0, descripcionPais = "Seleccione un país" });
            paises.AddRange(paisService.GetAll());

            cmbMoneda.DataSource = monedas;
            cmbTipoCuenta.DataSource = tiposCuenta;
            cmbEstado.DataSource = tiposEstado;
            cmbPaises.DataSource = paises;

        }

        private void ConsultaCuenta_Load(object sender, EventArgs e)
        {
            cargarCampos();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            this.realizarBusqueda();
        }

        public void realizarBusqueda()
        {
            int? moneda = (int)cmbMoneda.SelectedValue <= 0 ? (int?)null : (int)cmbMoneda.SelectedValue;
            long? pais = (long)cmbPaises.SelectedValue <= 0 ? (long?)null : (long)cmbPaises.SelectedValue;
            int? estado = (int)cmbEstado.SelectedValue <= 0 ? (int?)null : (int)cmbEstado.SelectedValue;
            int? tipoCuenta = (int)cmbTipoCuenta.SelectedValue <= 0 ? (int?)null : (int)cmbTipoCuenta.SelectedValue;
            long? nroDocCliente, tipoDocCliente;

            if (Session.Usuario.SelectedRol.Id == (int)Entities.Enums.Roles.Admin)
            {
                nroDocCliente = (long?)null;
                tipoDocCliente = (long?)null;
            }
            else
            {
                Cliente cliente = new Cliente();
                cliente = clienteService.getClienteByUsername(usuario.Username);
                nroDocCliente = cliente.numeroDocumento;
                tipoDocCliente = cliente.tipoDocumento;
            }


            dgvCuentas.AutoGenerateColumns = false;
            dgvCuentas.DataSource = cuentaService.GetCuentas(pais, estado, moneda, tipoCuenta, nroDocCliente, tipoDocCliente);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cmbMoneda.SelectedValue = 0;
            cmbPaises.SelectedValue = (long)(0);
            cmbTipoCuenta.SelectedValue = 0;
            cmbEstado.SelectedValue = 0;
        }

        private void dgvCuentas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)//Editar cuenta
            {
                var row = dgvCuentas.Rows[e.RowIndex];
                var cell = row.Cells["Numero"];
                int estadoCuenta = cuentaService.getEstado(Convert.ToInt64(cell.Value));

                switch (estadoCuenta)
                {
                    case 1: /*Pendiente de activacion*/
                        MessageBox.Show("La cuenta está pendiente de activación, no puede ser editada.", "Atención!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    case 3: /*Inhabilitada*/
                        int cantTrans = transaccionService.GetCountTransaccionesByCuenta(Convert.ToInt64(cell.Value));
                        if (cantTrans > 5)
                        {
                            MessageBox.Show("La cuenta fue inhabilitada por tener más de 5 transacciones sin facturar. La misma no podrá ser editada hasta que no se facturen las transacciones pendientes.", "Atención!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                        break;
                    default:
                        break;
                }
                var form = new AltaCuenta(Convert.ToInt64(cell.Value), this);
                form.Show();
                form.MdiParent = this.MdiParent;
            }
            else if (e.ColumnIndex == 1)//Cerrar cuenta
            {
                if (MessageBox.Show("Desea cerrar la cuenta seleccionada? La misma no podrá volver a activarse.", "Atención!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    var row = dgvCuentas.Rows[e.RowIndex];
                    var cell = row.Cells["Numero"];
                    DateTime fecha = Session.Fecha;

                    try
                    {
                        int resp = cuentaService.CerrarCuenta(Convert.ToInt64(cell.Value), fecha);
                        if (resp == -1)
                        {
                            MessageBox.Show("La cuenta no se podrá cerrar mientras haya transacciones pendientes de pago.", "Atención!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            MessageBox.Show("Cuenta cerrada!", "Atención!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        this.realizarBusqueda();
                    }
                    catch (OperationCanceledException ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error al cerrar la cuenta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } 
            }
        }
    }
}
