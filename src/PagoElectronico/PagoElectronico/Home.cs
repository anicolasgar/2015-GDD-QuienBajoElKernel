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

        private void showForm(Form unForm)
        {
            this.Cursor = Cursors.WaitCursor;
            unForm.ShowDialog();
            Application.DoEvents();
            this.Cursor = Cursors.Default;
        }

    }
}
