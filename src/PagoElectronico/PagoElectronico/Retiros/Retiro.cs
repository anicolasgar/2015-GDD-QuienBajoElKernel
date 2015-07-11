﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PagoElectronico.Common;
using PagoElectronico.Repositories;
using PagoElectronico.Entities;
using PagoElectronico.Services;

namespace PagoElectronico.Retiros
{
    public partial class Retiro : Form
    {
        CuentaService cuentaService = new CuentaService();
        TipoMonedaService tipoMonedaService = new TipoMonedaService();
        TipoDocumentoService tipoDocumentoService = new TipoDocumentoService();
        List<long> listaCuentas;
        List<TipoMoneda> listaTiposMoneda;
        List<TipoDocumento> listaTiposDocumentos;

        Usuario usuario = Session.Usuario;
        //Para probar hasta q este el login: 10002 and cliente_numero_doc=45622098
        long tipoDocCliente = 10002, nroDocCliente = 45622098;


        public Retiro()
        {
            InitializeComponent();
            cargarComboCuentas();
            cargarComboTipoDoc();
            cargarComboTipoMoneda();
        }

        #region Eventos
        /*************    Metodos de componentes       *************/
        private void btnRetirar_Click(object sender, EventArgs e)
        {
            realizarRetiro();
            actualizarSaldoActual();

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiarDatos();
        }

        private void comboCuentaOrigen_SelectedIndexChanged(object sender, EventArgs e)
        {
            actualizarSaldoActual();
            recalcularSaldoPosterior();
        }

        private void txtImporte_TextChanged(object sender, EventArgs e)
        {
            recalcularSaldoPosterior();
        }

        #endregion

        #region MetodosPrivados
        /*************    Metodos privados       *************/
        private void realizarRetiro()
        {
            //validar que este habilitada
            // tener saldo
            // el importe ingresado debe ser menor o igual al saldo de la cuenta
            // el importe debe estar en dls
            if (Validaciones.validarCampoVacio(txtNroDoc) & Validaciones.validarCampoVacio(txtImporte)  & Validaciones.validarCampoNumericoDouble(txtImporte) & Validaciones.validarCampoNumericoEntero(txtNroDoc))
            {
                try
                {
                    validarCuentaHabilitada();
                    validarSaldoDisponible();
                    validarNumeroDocumento();
                    validarImporteEnDolares();
                }
                catch (OperationCanceledException ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "No se pudo realizar el retiro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void actualizarSaldoActual()
        {
            lblSaldoActual.Text = cuentaService.getSaldo(Convert.ToInt64(comboCuentaOrigen.Text.ToString())).ToString();
        }

        private void recalcularSaldoPosterior()
        {
            try
            {
                double saldoActual, saldoPosterior, costo, importe;

                if (txtImporte.Text.Length == 0)
                {
                    ocultarComponentes();
                }
                else
                {
                    mostrarComponentes();

                    saldoActual = Convert.ToDouble(lblSaldoActual.Text);
                    importe = Convert.ToDouble(txtImporte.Text);

                    saldoPosterior = saldoActual - importe;
                    lblSaldoPosterior.Text = saldoPosterior.ToString();

                }
            }
            catch (Exception) { }
        }

        private void limpiarDatos()
        {
            txtNroDoc.BackColor = System.Drawing.Color.White;
            txtImporte.BackColor = System.Drawing.Color.White;
            txtNroDoc.Text = String.Empty;
            txtImporte.Text = String.Empty;
            comboCuentaOrigen.SelectedIndex = 0;
            comboTipoMoneda.SelectedIndex = 0;
            //ocultarComponentes();
        }

        private void cargarComboCuentas()
        {
            listaCuentas = (List<long>)cuentaService.getByCliente(tipoDocCliente, nroDocCliente);
            if (listaCuentas.Count > 0)
            {
                comboCuentaOrigen.DataSource = listaCuentas;
            }
            else
            {
                MessageBox.Show("No posees cuentas para realizar transferencias", "Atencion !", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void cargarComboTipoDoc()
        {
            listaTiposDocumentos = (List<TipoDocumento>) tipoDocumentoService.GetAll();
            foreach (var item in listaTiposDocumentos)
            {
                comboTipoDoc.Items.Add(item.descripcion);
            }

            comboTipoDoc.SelectedIndex = 0;

        }

        private void cargarComboTipoMoneda()
        {
            listaTiposMoneda = (List<TipoMoneda>)tipoMonedaService.GetTiposMonedaByCuenta(comboCuentaOrigen.Text.ToString());
            foreach (var item in listaTiposMoneda)
            {
                comboTipoMoneda.Items.Add(item.descripcion);
            }

            comboTipoMoneda.SelectedIndex = 0;
        }

        private void ocultarComponentes()
        {
            lblSaldoPosterior.Visible = false;
            lblSaldoPosteriorRO.Visible = false;
        }

        private void mostrarComponentes()
        {
            lblSaldoPosterior.Visible = true;
            lblSaldoPosteriorRO.Visible = true;
        }

        #endregion

        #region Validaciones
        /*************    Validadores privados       *************/
        private void validarCuentaHabilitada()
        {

        }

        private void validarNumeroDocumento()
        {

        }

        private void validarSaldoDisponible()
        {
            if (Int64.Parse(lblSaldoActual.Text) <= 0)
            {
                throw new OperationCanceledException("La cuenta no posee fondos!");
            }

            if (Int64.Parse(txtImporte.Text) > Int64.Parse(lblSaldoActual.Text))
            {
                throw new OperationCanceledException("El saldo actual no es suficiente");
            }
        }

        private void validarImporteEnDolares()
        {

        }

        #endregion


    }
}
