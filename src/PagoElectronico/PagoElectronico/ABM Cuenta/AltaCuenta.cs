﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PagoElectronico.Services.Interfaces;
using PagoElectronico.Services;
using PagoElectronico.Entities;

namespace PagoElectronico.ABM_Cuenta
{
    public partial class AltaCuenta : Form
    {
        CuentaService cuentaService { get; set; }
        PaisService paisService { get; set; }
        TipoMonedaService tipoMonedaService { get; set; }
        ClienteService clienteService { get; set; }
        TransaccionService transaccionService { get; set; }
        long NroCuenta { get; set; }
        Usuario usuario = Session.Usuario;
        //Para probar hasta q este el login: 10002 and cliente_numero_doc=45622098
        Cliente cliente;
        long tipoDocCliente, nroDocCliente;
        ConsultaCuenta formPadre;
        DateTime Fecha = Session.Fecha;

        public AltaCuenta(long nroCuenta, ConsultaCuenta form)
        {
            formPadre = form;
            cuentaService = new CuentaService();
            paisService = new PaisService();
            tipoMonedaService = new TipoMonedaService();
            clienteService = new ClienteService();
            transaccionService = new TransaccionService();
            cliente = new Cliente();
            InitializeComponent();
            this.NroCuenta = nroCuenta;
        }

        private void AltaCuenta_Load(object sender, EventArgs e)
        {
            cargarCampos();
           
            if (NroCuenta > 0)
            {
                Cuenta cuenta = cuentaService.GetCuentaByNumero(NroCuenta);
                cmbPaises.SelectedValue = cuenta.paisCodigo;
                cmbTiposCuenta.SelectedValue = cuenta.tipoCuenta;
                cmbMonedas.SelectedValue = cuenta.monedaTipo;
                txtCuenta.Text = cuenta.numero.ToString();
                cmbPaises.Enabled = false;
                cmbMonedas.Enabled = false;
                cmbTiposCuenta.Enabled = false;
                this.Text = "Edición de Cuenta";
                int estadoCuenta = cuentaService.getEstado(NroCuenta);

                if (estadoCuenta == 3)
                {
                    btnGuardar.Enabled = false;
                    chkResuscribir.Visible = true;
                }
            }
            else
            {
                if (Session.Usuario.SelectedRol.Id == (int)Entities.Enums.Roles.Admin)
                {
                    lblCliente.Visible = true;
                    cmbClientes.Visible = true;
                    var clientes = clienteService.GetClientes();
                    cmbClientes.DataSource = clientes;
                }
            }
        }

        private void cargarCampos() 
        {
            var paises = paisService.GetAll();
            var monedas = tipoMonedaService.GetAll();
            var tiposCuenta = cuentaService.GetTiposCuenta();

            cmbPaises.DataSource = paises;
            cmbMonedas.DataSource = monedas;
            cmbTiposCuenta.DataSource = tiposCuenta;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            int codPais = Convert.ToInt32(cmbPaises.SelectedValue.ToString());
            int tipoMoneda = Convert.ToInt32(cmbMonedas.SelectedValue.ToString());
            int tipoCuenta = Convert.ToInt32(cmbTiposCuenta.SelectedValue.ToString());
            
            //if (Session.Usuario.SelectedRol.Id == (int)Entities.Enums.Roles.Admin)
            //{
            //    tipoDocCliente = ((Cliente)cmbClientes.SelectedItem).tipoDocumento;
            //    nroDocCliente = ((Cliente)cmbClientes.SelectedItem).numeroDocumento;
            //}
            //else
            //{
            //    cliente = clienteService.getClienteByUsername(usuario.Username);
            //    tipoDocCliente = cliente.tipoDocumento;
            //    nroDocCliente = cliente.numeroDocumento;
            //}

            if (txtCuenta.Text == "")
            {
                if (Session.Usuario.SelectedRol.Id == (int)Entities.Enums.Roles.Admin)
                {
                    tipoDocCliente = ((Cliente)cmbClientes.SelectedItem).tipoDocumento;
                    nroDocCliente = ((Cliente)cmbClientes.SelectedItem).numeroDocumento;
                }
                else
                {
                    cliente = clienteService.getClienteByUsername(usuario.Username);
                    tipoDocCliente = cliente.tipoDocumento;
                    nroDocCliente = cliente.numeroDocumento;
                }

                try
                {
                    cuentaService.InsertaCuenta(codPais, tipoMoneda, tipoCuenta, tipoDocCliente, nroDocCliente, Fecha);
                    MessageBox.Show("Cuenta creada exitosamente. Recuerde que la misma permanecerá pendiente de activación hasta que abone el costo de apertura", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.cargarCampos();
                }
                catch (OperationCanceledException ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "No se pudo crear la cuenta!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception)
                {
                    MessageBox.Show("No se pudo crear la cuenta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                long numCuenta = Convert.ToInt64(txtCuenta.Text);

                try
                {
                    cuentaService.ModificaCuenta(numCuenta, tipoCuenta, Fecha);
                    MessageBox.Show("Cuenta modificada exitosamente!", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    inhabilitarCuentaSiCorresponde();
                    this.Close();
                }
                catch (OperationCanceledException ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "No se pudo modificar la cuenta.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error", "No se pudo modificar la cuenta.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void AltaCuenta_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formPadre != null)
            {
                formPadre.realizarBusqueda();
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkResuscribir_CheckedChanged(object sender, EventArgs e)
        {
            int estadoCuenta = cuentaService.getEstado(NroCuenta);
            if (cmbTiposCuenta.Enabled == false)
            {
                cmbTiposCuenta.Enabled = true;
                btnGuardar.Enabled = true;
            }
            else
            {
                cmbTiposCuenta.Enabled = false;
                btnGuardar.Enabled = false;
            }
        }

        private void inhabilitarCuentaSiCorresponde()
        {
            long cuenta = NroCuenta;
            if (transaccionService.GetCountTransaccionesByCuenta(cuenta) > 5)
            {
                cuentaService.inhabilitarCuenta(cuenta, Fecha);
                MessageBox.Show("La cuenta fue inhabilitada por superar las 5 transacciones sin facturar", "Cuenta inhabilitada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
