﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PagoElectronico.Entities;
using PagoElectronico.Common;
using PagoElectronico.Services;

namespace PagoElectronico.Depositos
{
    public partial class DepositoForm : Form
    {

        Cliente clienteLogueado;
        Usuario usuarioLogueado = Session.Usuario;

        CuentaService cuentaService = new CuentaService();
        TipoMonedaService tipoMonedaService = new TipoMonedaService();
        TarjetaService tarjetaService = new TarjetaService();
        DepositoService depositoService = new DepositoService();

        List<long> listaCuentas;
        List<TipoMoneda> listaTiposMoneda;
        List<Tarjeta> listaTC;

        public DepositoForm()
        {
            InitializeComponent();
        }

        #region Eventos
        /*************    Metodos de componentes       *************/

        private void DepositoForm_Load(object sender, EventArgs e)
        {
            try
            {
                clienteLogueado = Utils.obtenerCliente(usuarioLogueado);
                dateTimePicker1.Value = DateTime.Today.AddDays(-1);
                cargarComboCuentas();
                cargarComboTipoMoneda();
                cargarComboTC();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDepositar_Click(object sender, EventArgs e)
        {
            realizarDeposito();
            actualizarSaldoActual();
            recalcularSaldoPosterior();
        }

        private void comboCuentaOrigen_SelectedIndexChanged(object sender, EventArgs e)
        {
            actualizarSaldoActual();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiarDatos();
        }

        private void txtImporte_TextChanged(object sender, EventArgs e)
        {
            recalcularSaldoPosterior();
        }


        #endregion

        #region MetodosPrivados
        /*************    Metodos privados       *************/

        private void realizarDeposito()
        {
            Boolean validador = Validaciones.validarCampoVacio(txtImporte) & Validaciones.validarCampoNumericoDouble(txtImporte);
            if (validador)
            {
                try
                {
                    validarImportePositivo();
                    validarCuentaHabilitada();

                    Deposito deposito = new Deposito();
                    deposito.fecha = DateTime.Now;
                    deposito.importe = Int64.Parse(txtImporte.Text);
                    deposito.cuentaNumero = Int64.Parse(comboCuentaOrigen.Text);
                    deposito.monedaTipo = ((TipoMoneda)comboTipoMoneda.SelectedItem).codigo;
                    deposito.tarjetaNumero = ((Tarjeta)comboTarjeta.SelectedItem).tarjetaNumero;

                    depositoService.Insert(deposito);
                    MessageBox.Show("Se ha realizado el deposito de saldo. ", "Deposito realizado satisfactoriamente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiarDatos();

                }
                catch (Exception)
                {

                }
            }

        }

        private void recalcularSaldoPosterior()
        {
            try
            {
                double saldoActual, saldoPosterior, importe;

                if (txtImporte.Text.Length == 0)
                {
                    mostrarComponentes(false);
                }
                else
                {
                    mostrarComponentes(true);

                    saldoActual = Convert.ToDouble(lblSaldoActual.Text);
                    importe = Convert.ToDouble(txtImporte.Text);

                    saldoPosterior = saldoActual - importe;
                    lblSaldoPosterior.Text = saldoPosterior.ToString();

                }
            }
            catch (Exception) { }
        }

        private void mostrarComponentes(Boolean validate)
        {
            this.lblSaldoPosterior.Visible = validate;
            this.lblSaldoPosteriorRO.Visible = validate;
        }

        private void actualizarSaldoActual()
        {
            lblSaldoActual.Text = cuentaService.getSaldo(Convert.ToInt64(comboCuentaOrigen.Text.ToString())).ToString();
        }

        private void limpiarDatos()
        {
            txtImporte.BackColor = System.Drawing.Color.White;
            txtImporte.Text = String.Empty;
            comboCuentaOrigen.SelectedIndex = 0;
            comboTipoMoneda.SelectedIndex = 0;
            comboTarjeta.SelectedIndex = 0;
        }


        #endregion

        #region CargarCombos


        private void cargarComboCuentas()
        {
            listaCuentas = (List<long>)cuentaService.getByCliente(clienteLogueado.tipoDocumento, clienteLogueado.numeroDocumento);
            if (listaCuentas.Count > 0)
            {
                comboCuentaOrigen.DataSource = listaCuentas;
            }
            else
            {
                throw new Exception("No posees cuentas para realizar retiros");

            }

        }

        private void cargarComboTipoMoneda()
        {
            listaTiposMoneda = (List<TipoMoneda>)tipoMonedaService.GetTiposMonedaByCuenta(comboCuentaOrigen.Text.ToString());
            comboTipoMoneda.DataSource = listaTiposMoneda;
            comboTipoMoneda.SelectedIndex = 0;
        }

        private void cargarComboTC()
        {
            listaTC = (List<Tarjeta>)tarjetaService.getAllByCliente(clienteLogueado.tipoDocumento, clienteLogueado.numeroDocumento);
            comboTarjeta.DataSource = listaTC;
            comboTarjeta.SelectedIndex = 0;
        }


        #endregion

        #region Validaciones

        private void validarImportePositivo()
        {
            if (Int64.Parse(txtImporte.Text) < 1)
            {
                throw new Exception("El importe debe ser mayor o igual a 1");
            }
        }

        private void validarCuentaHabilitada()
        {
            if (cuentaService.getEstado(Int64.Parse(comboCuentaOrigen.Text)) != (Int32)Entities.Enums.EstadosCuenta.Habilitada)
            {

                throw new OperationCanceledException("La cuenta no se encuentra habilitada");
            }
        }
       
        #endregion


    }
}
