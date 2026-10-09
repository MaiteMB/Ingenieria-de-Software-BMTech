using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BLLBMTech;
using ServiciosBMTech.Seguridad;

namespace BMTech
{
    public partial class FrmPrincipal : Form
    {
        private readonly BLLRol bllRol;

        public FrmPrincipal()
        {
            InitializeComponent();
            bllRol = new BLLRol();
        }

        private void menuClientes_Click(object sender, EventArgs e)
        {
            FrmCliente frmCliente = new FrmCliente();
            frmCliente.ShowDialog();
        }

        private void menuProductos_Click(object sender, EventArgs e)
        {
            FrmProducto frmProducto = new FrmProducto();
            frmProducto.ShowDialog();
        }

        private void menuVentas_Click(object sender, EventArgs e)
        {
            FrmVenta frmVenta = new FrmVenta();
            frmVenta.ShowDialog();
        }

        private void menuControlCambiosProducto_Click(object sender, EventArgs e)
        {
            FrmControlCambiosProducto frmControlCambiosProducto = new FrmControlCambiosProducto();
            frmControlCambiosProducto.ShowDialog();
        }


        private void menuVerificarIntegridad_Click(object sender, EventArgs e)
        {
            BLLIntegridad bllIntegridad = new BLLIntegridad();
            bool correcto = bllIntegridad.VerificarIntegridadProducto()
                && bllIntegridad.VerificarIntegridadCliente();

            if (correcto)
            {
                MessageBox.Show("La integridad de clientes y productos es correcta.", "Integridad", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("La integridad de clientes o productos presenta diferencias.", "Integridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void menuRegenerarIntegridad_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Se recalcularan los digitos con los datos actuales. Continuar solo si reviso que son correctos.",
                "Regenerar integridad", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            BLLIntegridad bllIntegridad = new BLLIntegridad();
            bllIntegridad.RegenerarIntegridadProducto();
            bllIntegridad.RegenerarIntegridadCliente();
            MessageBox.Show("La integridad de clientes y productos fue regenerada.", "Integridad", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void menuSalir_Click(object sender, EventArgs e)
        {
            BLLLog bllLog = new BLLLog();
            bllLog.RegistrarEvento("Logout", "Acceso", 1);
            SessionManager.Logout();
            Application.Exit();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            AplicarPermisos();
        }

        private void AplicarPermisos()
        {
            menuClientes.Enabled = false;
            menuProductos.Enabled = false;
            menuVentas.Enabled = false;
            menuSeguridad.Enabled = false;

            if (SessionManager.getSession.Usuario == null)
            {
                return;
            }

            List<string> patentes = bllRol.ObtenerPatentesPorPerfil(SessionManager.getSession.Usuario.perfil);

            menuClientes.Enabled = patentes.Contains("CLIENTES");
            menuProductos.Enabled = patentes.Contains("PRODUCTOS");
            menuVentas.Enabled = patentes.Contains("VENTAS");
            menuSeguridad.Enabled = patentes.Contains("SEGURIDAD");
        }
    }
}


