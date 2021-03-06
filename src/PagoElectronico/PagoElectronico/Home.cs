﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PagoElectronico.Consulta_Saldos;
using PagoElectronico.Listados;
using PagoElectronico.Transferencias;
using PagoElectronico.ABM_Rol;
using PagoElectronico.Entities;
using PagoElectronico.ABM_Cuenta;
using PagoElectronico.Depositos;
using PagoElectronico.Facturacion;
using PagoElectronico.Login;
using PagoElectronico.Entities.Enums;

namespace PagoElectronico
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Está seguro que desea salir?", "Pago Electrónico",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Information) == DialogResult.OK)
                    Environment.Exit(1);
                else
                    e.Cancel = true; // to don't close form is user change his mind
            }
        }

        private void consultarSaldoDeCuentaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showForm(new ConsultaSaldos());
        }

        private void listadoEstadisticoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showForm(new ListadoEstadistico());
        }


        private void transferenciasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showForm(new TransferenciasCuentas());
        }

        private void crearNuevoRolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showForm(new Alta(0));
        }

        private void showForm(Form unForm)
        {
            this.Cursor = Cursors.WaitCursor;
            unForm.Location = this.Location;
            unForm.StartPosition = FormStartPosition.CenterScreen;
            unForm.MdiParent = this;
            unForm.Show();
            this.Cursor = Cursors.Default;
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Session.Usuario = null;
            var form = new Login.Login();
            form.Location = this.Location;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Show();
            this.Hide();
        }

        private void buscarRolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rolBusquedaForm = new ConsultaRol();
            rolBusquedaForm.MdiParent = this;
            showForm(rolBusquedaForm);

        }

        private void altaDeCuenta_Click(object sender, EventArgs e)
        {
            showForm(new AltaCuenta((long)0, null));
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void listadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showForm(new ABM_Cliente.ConsultaCliente());
        }

        private void retiroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showForm(new Retiros.Retiro());
        }

        private void altaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showForm(new ABM_Cliente.AltaCliente());
        }

        private void listadoCuentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showForm(new ConsultaCuenta());
        }

        private void depositoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showForm(new DepositoForm());
        }

        private void facturarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showForm(new FacturacionForm());
        }

        private void cambiarContraseñaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showForm(new CambiarPassword());
        }

        private void Home_Load(object sender, EventArgs e)
        {
            foreach (var func in Session.Usuario.SelectedRol.Funcionalidades)
            {
                switch ((FuncionalidadesEnum)func.Id)
                {
                    case FuncionalidadesEnum.ABM_CLIENTE:
                        clientesToolStripMenuItem.Visible = true;
                        break;
                    case FuncionalidadesEnum.ABM_CUENTA:
                        cuentasToolStripMenuItem.Visible = true;
                        break;
                    case FuncionalidadesEnum.ABM_ROL:
                        rolesToolStripMenuItem.Visible = true;
                        break;
                    case FuncionalidadesEnum.ASOCIAR_DESASOCIAR_TC:
                        break;
                    case FuncionalidadesEnum.CONSULTA_SALDO:
                        consultarSaldoDeCuentaToolStripMenuItem.Visible = true;
                        break;
                    case FuncionalidadesEnum.DEPOSITO:
                        depositoToolStripMenuItem.Visible = true;
                        break;
                    case FuncionalidadesEnum.FACTURACION:
                        facturarToolStripMenuItem.Visible = true;
                        break;
                    case FuncionalidadesEnum.LISTADO_ESTADISTICO:
                        listadoEstadisticoToolStripMenuItem.Visible = true;
                        break;
                    case FuncionalidadesEnum.RETIRO:
                        retiroToolStripMenuItem.Visible = true;
                        break;
                    case FuncionalidadesEnum.TRANSFERENCIA:
                        transferenciasToolStripMenuItem.Visible = true;
                        break;
                }
            }

            if (Session.Usuario.SelectedRol.Id == (int)Entities.Enums.Roles.Admin)
            {
                selecciónClienteToolStripMenuItem.Visible = true;
            }
        }

        private void selecciónClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showForm(new SeleccionCliente());
        }
    }
}