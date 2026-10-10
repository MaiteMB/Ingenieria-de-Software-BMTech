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
            IdiomasFormulario.mb506Vincular(this);
            InitializeComponent();
            bllRol = new BLLRol();
            ToolStripComboBox cmbIdioma = new ToolStripComboBox();
            cmbIdioma.DropDownStyle = ComboBoxStyle.DropDownList;
            Load += delegate
            {
                try
                {
                    var idiomas = new BLLIdioma().mb506ObtenerIdiomas();
                    string codigoActual = mb506.ServiciosBMTech.Idiomas.IdiomasStatic.Observer.idiomaActual;
                    cmbIdioma.ComboBox.DataSource = idiomas;
                    cmbIdioma.SelectedItem = idiomas.Find(i => i.codigo == codigoActual);
                }
                catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
            };
            cmbIdioma.SelectedIndexChanged += delegate
            {
                var idioma = cmbIdioma.SelectedItem as mb506.BEBMTech.Idiomas.Idioma;
                if (idioma != null) mb506.ServiciosBMTech.Idiomas.IdiomasStatic.Observer.mb506CambiarIdioma(idioma.codigo);
            };
            cmbIdioma.Alignment = ToolStripItemAlignment.Right;
            menuPrincipal.Items.Add(cmbIdioma);
            menuSeguridad.DropDownItems.Add("Usuarios", null, delegate { new mb506FrmUsuarios().ShowDialog(this); });
            menuSeguridad.DropDownItems.Add("Permisos", null, delegate
            {
                new mb506FrmPermisos().ShowDialog(this);
                mb506AplicarPermisos();
            });
            menuSeguridad.DropDownItems.Add("Bitacora", null, delegate { new mb506FrmBitacora().ShowDialog(this); });
            menuSeguridad.DropDownItems.Add("Idiomas", null, delegate
            {
                using (var formulario = new mb506FrmIdiomas()) formulario.ShowDialog(this);
                try
                {
                    string codigoActual = mb506.ServiciosBMTech.Idiomas.IdiomasStatic.Observer.idiomaActual;
                    var idiomas = new BLLIdioma().mb506ObtenerIdiomas();
                    cmbIdioma.ComboBox.DataSource = idiomas;
                    cmbIdioma.SelectedItem = idiomas.Find(i => i.codigo == codigoActual);
                }
                catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
            });
            menuSeguridad.DropDownItems.Add("Copias de seguridad", null, delegate { new mb506FrmBackup().ShowDialog(this); });
            menuVentas.DropDownItems.Add("Registrar venta", null, delegate { new mb506FrmVenta().ShowDialog(this); });
            menuVentas.DropDownItems.Add("Historial de ventas", null, delegate { new mb506FrmHistorialVentas().ShowDialog(this); });
            ToolStripMenuItem menuClave = new ToolStripMenuItem("Cambiar contraseña");
            menuClave.Click += delegate { new mb506FrmCambiarPassword().ShowDialog(this); };
            menuPrincipal.Items.Add(menuClave);
            FormClosed += delegate
            {
                if (SessionManager.IsSessionActive)
                {
                    try { new BLLLog().mb506RegistrarEvento("Logout", "Acceso", 1); }
                    catch (Exception ex) { Mensajes.mb506Mostrar("No se pudo registrar el cierre de sesion. " + ex.Message); }
                    finally { SessionManager.mb506Logout(); }
                }
            };
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
            try
            {
                mb506AplicarPermisos();
                List<string> diferencias = new BLLIntegridad().mb506VerificarSistema();
                if (diferencias.Count > 0)
                {
                    menuClientes.Enabled = menuProductos.Enabled = menuVentas.Enabled = false;
                    Mensajes.mb506Mostrar("Revise integridad antes de operar. Diferencias en: " + string.Join(", ", diferencias));
                }
            }
            catch (Exception ex)
            {
                menuClientes.Enabled = menuProductos.Enabled = menuVentas.Enabled = menuSeguridad.Enabled = false;
                Mensajes.mb506Mostrar(ex.Message);
            }
        }

        private void mb506AplicarPermisos()
        {
            menuClientes.Enabled = false;
            menuProductos.Enabled = false;
            menuVentas.Enabled = false;
            menuSeguridad.Enabled = false;

            if (SessionManager.getSession.Usuario == null)
            {
                return;
            }

            List<string> patentes = bllRol.mb506ObtenerPatentesPorPerfil(SessionManager.getSession.Usuario.perfil);

            menuClientes.Enabled = patentes.Contains("CLIENTES");
            menuProductos.Enabled = patentes.Contains("PRODUCTOS");
            menuVentas.Enabled = patentes.Contains("VENTAS");
            menuSeguridad.Enabled = patentes.Contains("SEGURIDAD");
        }
    }
}


