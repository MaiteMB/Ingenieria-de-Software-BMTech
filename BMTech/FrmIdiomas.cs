using System;
using System.Data;
using System.Windows.Forms;
using mb506.BEBMTech.Idiomas;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public partial class mb506FrmIdiomas : Form
    {
        private BLLIdioma bllIdioma = new BLLIdioma();
        private bool cargando;

        public mb506FrmIdiomas()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            IdiomasFormulario.mb506Vincular(this);
            AyudaSistema.mb506Vincular(this);
        }

        private void mb506Cargar()
        {
            try
            {
                cargando = true;
                cmbIdiomas.DataSource = bllIdioma.mb506ObtenerIdiomas();
                cargando = false;
                mb506Seleccionar(this, EventArgs.Empty);
            }
            catch (Exception ex) { cargando = false; Mensajes.mb506Mostrar(ex.Message); }
        }

        private void mb506Seleccionar(object sender, EventArgs e)
        {
            if (cargando) return;
            try
            {
                Idioma idioma = cmbIdiomas.SelectedItem as Idioma;
                if (idioma == null) return;
                txtCodigo.Text = idioma.codigo;
                txtCodigo.ReadOnly = true;
                txtNombre.Text = idioma.nombre;
                mb506Textos(idioma.codigo);
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }

        private void mb506Textos(string codigo)
        {
            dgvTextos.DataSource = bllIdioma.mb506ObtenerTextos(codigo);
        }

        private void mb506Accion(int accion)
        {
            try
            {
                if (accion == 0)
                {
                    cmbIdiomas.SelectedIndex = -1;
                    txtCodigo.ReadOnly = false;
                    txtCodigo.Clear(); txtNombre.Clear();
                    mb506Textos("es");
                    txtCodigo.Focus();
                    return;
                }
                if (accion == 1)
                {
                    dgvTextos.EndEdit();
                    BindingContext[dgvTextos.DataSource].EndCurrentEdit();
                    bllIdioma.mb506GuardarIdioma(txtCodigo.Text.Trim(), txtNombre.Text.Trim(), (DataTable)dgvTextos.DataSource);
                }
                else if (Mensajes.mb506Mostrar("Eliminar este idioma?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    bllIdioma.mb506EliminarIdioma(txtCodigo.Text.Trim());
                mb506Cargar();
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }

        private void mb506Nuevo(object sender, EventArgs e)
        {
            mb506Accion(0);
        }

        private void mb506Guardar(object sender, EventArgs e)
        {
            mb506Accion(1);
        }

        private void mb506Eliminar(object sender, EventArgs e)
        {
            mb506Accion(2);
        }

        private void mb506FrmIdiomas_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            if (!DesignMode) mb506Cargar();
        }
    }
}
