using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using mb506.BEBMTech.Idiomas;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public class mb506FrmIdiomas : Form
    {
        private ComboBox cmbIdiomas = new ComboBox();
        private TextBox txtCodigo = new TextBox();
        private TextBox txtNombre = new TextBox();
        private DataGridView dgvTextos = new DataGridView();
        private BLLIdioma bllIdioma = new BLLIdioma();
        private bool cargando;

        public mb506FrmIdiomas()
        {
            IdiomasFormulario.mb506Vincular(this);
            Text = "Idiomas y traducciones";
            ClientSize = new Size(950, 660);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10);
            BackColor = Color.White;
            cmbIdiomas.SetBounds(20, 20, 250, 30);
            cmbIdiomas.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIdiomas.SelectedIndexChanged += mb506Seleccionar;
            Controls.Add(new Label { Text = "Codigo de idioma", Left = 300, Top = 24, Width = 140 });
            txtCodigo.SetBounds(440, 20, 65, 30);
            txtCodigo.MaxLength = 2;
            Controls.Add(new Label { Text = "Nombre", Left = 525, Top = 24, Width = 75 });
            txtNombre.SetBounds(605, 20, 320, 30);
            txtNombre.MaxLength = 50;
            dgvTextos.SetBounds(20, 75, 905, 510);
            dgvTextos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvTextos.AllowUserToAddRows = dgvTextos.AllowUserToDeleteRows = false;
            dgvTextos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTextos.SelectionMode = DataGridViewSelectionMode.CellSelect;
            string[] acciones = { "Nuevo", "Guardar", "Eliminar" };
            for (int i = 0; i < acciones.Length; i++)
            {
                Button boton = new Button { Text = acciones[i], BackColor = Color.Teal, ForeColor = Color.White };
                boton.SetBounds(20 + i * 150, 605, 135, 36);
                boton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
                int accion = i;
                boton.Click += delegate { mb506Accion(accion); };
                Controls.Add(boton);
            }
            Controls.AddRange(new Control[] { cmbIdiomas, txtCodigo, txtNombre, dgvTextos });
            Load += delegate { mb506Cargar(); };
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
            dgvTextos.Columns["etiqueta"].ReadOnly = true;
            dgvTextos.Columns["textoTraducido"].ReadOnly = false;
            dgvTextos.Columns["etiqueta"].HeaderText = "Texto original";
            dgvTextos.Columns["textoTraducido"].HeaderText = "Traduccion";
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
    }
}
