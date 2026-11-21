using System;
using System.Collections.Generic;
using System.Windows.Forms;
using mb506.BLLBMTech;
using mb506.ServiciosBMTech.Seguridad;

namespace mb506.BMTech
{
    public partial class mb506FrmPrincipal : Form
    {
        private readonly BLLRol bllRol;

        public mb506FrmPrincipal()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            IdiomasFormulario.mb506Vincular(this);
            AyudaSistema.mb506Vincular(this);
            bllRol = new BLLRol();
        }

        private void mb506menuClientes_Click(object sender, EventArgs e)
        {
            mb506FrmCliente mb506FrmCliente = new mb506FrmCliente();
            mb506FrmCliente.ShowDialog();
        }

        private void mb506menuProductos_Click(object sender, EventArgs e)
        {
            mb506FrmProducto mb506FrmProducto = new mb506FrmProducto();
            mb506FrmProducto.ShowDialog();
        }

        private void mb506menuVentas_Click(object sender, EventArgs e)
        {
        }

        private void mb506menuServicios_Click(object sender, EventArgs e)
        {
            using (mb506FrmServicioTecnico formulario = new mb506FrmServicioTecnico()) formulario.ShowDialog(this);
        }

        private void mb506menuInformes_Click(object sender, EventArgs e)
        {
            using (mb506FrmInformes formulario = new mb506FrmInformes()) formulario.ShowDialog(this);
        }

        private void mb506menuAyuda_Click(object sender, EventArgs e)
        {
            AyudaSistema.mb506Mostrar(Name);
        }

        private void mb506menuControlCambiosProducto_Click(object sender, EventArgs e)
        {
            mb506FrmControlCambiosProducto mb506FrmControlCambiosProducto = new mb506FrmControlCambiosProducto();
            mb506FrmControlCambiosProducto.ShowDialog();
        }

        private void mb506menuVerificarIntegridad_Click(object sender, EventArgs e)
        {
            try
            {
            BLLIntegridad bllIntegridad = new BLLIntegridad();
            List<string> diferencias = bllIntegridad.mb506VerificarSistema();
            bool correcto = diferencias.Count == 0;

            if (correcto)
            {
                Mensajes.mb506Mostrar("La integridad del sistema es correcta.", "Integridad", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Mensajes.mb506Mostrar("Diferencias en: " + string.Join(", ", diferencias), "Integridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }

        private void mb506menuRegenerarIntegridad_Click(object sender, EventArgs e)
        {
            if (Mensajes.mb506Mostrar("Se recalcularan los digitos con los datos actuales. Continuar solo si reviso que son correctos.",
                "Regenerar integridad", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            BLLIntegridad bllIntegridad = new BLLIntegridad();
            try
            {
                bllIntegridad.mb506RegenerarSistema();
                mb506AplicarPermisos();
                Mensajes.mb506Mostrar("La integridad del sistema fue regenerada.", "Integridad", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }

        private void mb506menuSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mb506FrmPrincipal_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            mb506CargarIdiomas();
            try
            {
                mb506AplicarPermisos();
                List<string> diferencias = new BLLIntegridad().mb506VerificarSistema();
                if (diferencias.Count > 0)
                {
                    menuClientes.Enabled = menuProductos.Enabled = menuVentas.Enabled = menuServicios.Enabled = menuInformes.Enabled = false;
                    Mensajes.mb506Mostrar("Revise integridad antes de operar. Diferencias en: " + string.Join(", ", diferencias));
                }
            }
            catch (Exception ex)
            {
                menuClientes.Enabled = menuProductos.Enabled = menuVentas.Enabled = menuServicios.Enabled = menuInformes.Enabled = menuSeguridad.Enabled = false;
                Mensajes.mb506Mostrar(ex.Message);
            }
        }

        private void mb506AplicarPermisos()
        {
            menuUsuarios.Enabled = menuPermisos.Enabled = menuIdiomas.Enabled = false;
            menuClientes.Enabled = false;
            menuProductos.Enabled = false;
            menuVentas.Enabled = false;
            menuServicios.Enabled = false;
            menuInformes.Enabled = false;
            menuSeguridad.Enabled = false;

            if (SessionManager.getSession.Usuario == null)
            {
                return;
            }

            List<string> patentes = bllRol.mb506ObtenerPatentesPorPerfil(SessionManager.getSession.Usuario.perfil);

            menuClientes.Enabled = patentes.Contains("CLIENTES");
            menuProductos.Enabled = patentes.Contains("PRODUCTOS");
            menuVentas.Enabled = patentes.Contains("VENTAS");
            menuServicios.Enabled = patentes.Contains("SERVICIOS");
            menuInformes.Enabled = patentes.Contains("VENTAS") || patentes.Contains("SERVICIOS");
            menuSeguridad.Enabled = patentes.Contains("SEGURIDAD");
            menuUsuarios.Enabled = menuPermisos.Enabled = menuIdiomas.Enabled = patentes.Contains("SEGURIDAD");
        }

        private void mb506CargarIdiomas()
        {
            try
            {
                var idiomas = new BLLIdioma().mb506ObtenerIdiomas();
                string actual = mb506.ServiciosBMTech.Idiomas.IdiomasStatic.Observer.idiomaActual;
                cmbIdioma.ComboBox.DataSource = idiomas;
                cmbIdioma.SelectedItem = idiomas.Find(i => i.codigo == actual);
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }

        private void mb506IdiomaCambiado(object sender, EventArgs e)
        {
            var idioma = cmbIdioma.SelectedItem as mb506.BEBMTech.Idiomas.Idioma;
            if (idioma != null) mb506.ServiciosBMTech.Idiomas.IdiomasStatic.Observer.mb506CambiarIdioma(idioma.codigo);
        }

        private void mb506menuUsuarios_Click(object sender, EventArgs e)
        {
            using (var formulario = new mb506FrmUsuarios()) formulario.ShowDialog(this);
        }

        private void mb506menuBitacora_Click(object sender, EventArgs e)
        {
            using (var formulario = new mb506FrmBitacora()) formulario.ShowDialog(this);
        }

        private void mb506menuBackup_Click(object sender, EventArgs e)
        {
            using (var formulario = new mb506FrmBackup()) formulario.ShowDialog(this);
        }

        private void mb506menuRegistrarVenta_Click(object sender, EventArgs e)
        {
            using (var formulario = new mb506FrmVenta()) formulario.ShowDialog(this);
        }

        private void mb506menuHistorialVentas_Click(object sender, EventArgs e)
        {
            using (var formulario = new mb506FrmHistorialVentas()) formulario.ShowDialog(this);
        }

        private void mb506menuClave_Click(object sender, EventArgs e)
        {
            using (var formulario = new mb506FrmCambiarPassword()) formulario.ShowDialog(this);
        }

        private void mb506menuPermisos_Click(object sender, EventArgs e)
        {
            using (var formulario = new mb506FrmPermisos()) formulario.ShowDialog(this);
            mb506AplicarPermisos();
        }

        private void mb506menuIdiomas_Click(object sender, EventArgs e)
        {
            using (var formulario = new mb506FrmIdiomas()) formulario.ShowDialog(this);
            mb506CargarIdiomas();
        }

        private void mb506FrmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!SessionManager.IsSessionActive) return;
            try { new BLLLog().mb506RegistrarEvento("Logout", "Acceso", 1); }
            catch (Exception ex) { Mensajes.mb506Mostrar("No se pudo registrar el cierre de sesion. " + ex.Message); }
            finally { SessionManager.mb506Logout(); }
        }
    }
}
