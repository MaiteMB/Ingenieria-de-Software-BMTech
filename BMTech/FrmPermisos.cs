using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public class mb506FrmPermisos : Form
    {
        private BLLRol bllRol = new BLLRol();
        private ComboBox cmbTipo = new ComboBox();
        private DataGridView dgvElementos = new DataGridView();
        private TextBox txtId = new TextBox();
        private TextBox txtNombre = new TextBox();
        private CheckedListBox lstAsignaciones = new CheckedListBox();
        private List<string> idsAsignaciones = new List<string>();
        private bool nuevo = true;
        private TreeView tvPermisos = new TreeView();

        public mb506FrmPermisos()
        {
            IdiomasFormulario.mb506Vincular(this);
            Text = "Perfiles, familias y patentes";
            ClientSize = new Size(1170, 500);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 10);
            cmbTipo.SetBounds(20, 20, 260, 30);
            cmbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipo.Items.AddRange(new object[] { "Perfil", "Familia", "Patente" });
            dgvElementos.SetBounds(20, 70, 390, 400);
            dgvElementos.ReadOnly = true;
            dgvElementos.AllowUserToAddRows = false;
            dgvElementos.MultiSelect = false;
            dgvElementos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvElementos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvElementos.CellClick += mb506Seleccionar;
            Label id = new Label { Text = "Identificador" };
            id.SetBounds(440, 70, 140, 25);
            txtId.SetBounds(580, 70, 240, 28);
            Label nombre = new Label { Text = "Nombre" };
            nombre.SetBounds(440, 110, 140, 25);
            txtNombre.SetBounds(580, 110, 240, 28);
            Button btnNuevo = mb506CrearBoton("Nuevo", 440, 160);
            btnNuevo.Click += delegate { mb506Limpiar(); };
            Button btnGuardar = mb506CrearBoton("Guardar datos", 630, 160);
            btnGuardar.Click += mb506GuardarDatos;
            lstAsignaciones.SetBounds(440, 220, 380, 190);
            lstAsignaciones.CheckOnClick = true;
            Button btnAsignar = mb506CrearBoton("Guardar asignaciones", 440, 435);
            btnAsignar.Width = 250;
            btnAsignar.Click += mb506GuardarAsignaciones;
            Button btnEliminar = mb506CrearBoton("Eliminar", 20, 20);
            btnEliminar.SetBounds(300, 20, 110, 32);
            btnEliminar.Click += delegate
            {
                try
                {
                    if (nuevo) throw new Exception("Seleccione un elemento.");
                    if (Mensajes.mb506Mostrar("Confirma eliminar " + txtId.Text + "?", "Permisos",
                        MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                    bllRol.mb506EliminarElemento(Tipo, txtId.Text);
                    mb506Cargar();
                }
                catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
            };
            Controls.Add(btnEliminar);
            Label estructura = new Label { Text = "Estructura de permisos", AutoSize = true, Left = 850, Top = 25 };
            tvPermisos.SetBounds(850, 70, 295, 400);
            tvPermisos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            Controls.AddRange(new Control[] { estructura, tvPermisos });
            Controls.AddRange(new Control[] { cmbTipo, dgvElementos, id, txtId, nombre,
                txtNombre, btnNuevo, btnGuardar, lstAsignaciones, btnAsignar });
            cmbTipo.SelectedIndexChanged += delegate
            {
                try { mb506Cargar(); }
                catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
            };
            Load += delegate { cmbTipo.SelectedIndex = 0; };
        }

        private Button mb506CrearBoton(string texto, int x, int y)
        {
            Button boton = new Button { Text = texto, BackColor = Color.Teal, ForeColor = Color.White };
            boton.SetBounds(x, y, 180, 36);
            return boton;
        }

        private string Tipo { get { return Convert.ToString(cmbTipo.SelectedItem); } }

        private void mb506Cargar()
        {
            dgvElementos.DataSource = bllRol.mb506ObtenerElementos(Tipo);
            mb506Limpiar();
            idsAsignaciones.Clear();
            lstAsignaciones.Items.Clear();
            mb506CargarArbol();
            lstAsignaciones.Enabled = Tipo != "Patente";
            if (Tipo == "Patente") return;
            DataTable hijos = bllRol.mb506ObtenerElementos(Tipo == "Perfil" ? "Familia" : "Patente");
            foreach (DataRow fila in hijos.Rows)
            {
                idsAsignaciones.Add(Convert.ToString(fila["id"]));
                lstAsignaciones.Items.Add(fila["id"] + " - " + fila["nombre"]);
            }
        }

        private TreeNode mb506Nodo(mb506.BEBMTech.Roles.Rol rol)
        {
            TreeNode nodo = new TreeNode(rol.idRol);
            var familia = rol as mb506.BEBMTech.Roles.Familia;
            if (familia != null)
                foreach (var hijo in familia.Hijos) nodo.Nodes.Add(mb506Nodo(hijo));
            return nodo;
        }

        private void mb506CargarArbol()
        {
            tvPermisos.Nodes.Clear();
            foreach (DataRow fila in bllRol.mb506ObtenerElementos("Perfil").Rows)
            {
                var perfil = bllRol.mb506ObtenerPerfil(Convert.ToString(fila["id"]));
                TreeNode nodo = new TreeNode(perfil.id_perfil);
                foreach (var rol in perfil.Roles) nodo.Nodes.Add(mb506Nodo(rol));
                tvPermisos.Nodes.Add(nodo);
            }
            tvPermisos.ExpandAll();
        }

        private void mb506Limpiar()
        {
            nuevo = true;
            txtId.Clear(); txtId.ReadOnly = false;
            txtNombre.Clear(); txtNombre.Enabled = Tipo != "Perfil";
            for (int i = 0; i < lstAsignaciones.Items.Count; i++) lstAsignaciones.SetItemChecked(i, false);
            dgvElementos.ClearSelection();
        }

        private void mb506Seleccionar(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            try
            {
                DataRowView fila = dgvElementos.Rows[e.RowIndex].DataBoundItem as DataRowView;
                if (fila == null) return;
                txtId.Text = Convert.ToString(fila["id"]);
                txtNombre.Text = Convert.ToString(fila["nombre"]);
                txtId.ReadOnly = true; nuevo = false;
                if (Tipo == "Patente") return;
                List<string> elegidos = bllRol.mb506ObtenerAsignaciones(Tipo, txtId.Text);
                for (int i = 0; i < idsAsignaciones.Count; i++)
                    lstAsignaciones.SetItemChecked(i, elegidos.Contains(idsAsignaciones[i]));
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }

        private void mb506GuardarDatos(object sender, EventArgs e)
        {
            try
            {
                bllRol.mb506GuardarElemento(Tipo, txtId.Text,
                    Tipo == "Perfil" ? txtId.Text : txtNombre.Text, nuevo);
                mb506Cargar(); Mensajes.mb506Mostrar("Datos guardados.");
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }

        private void mb506GuardarAsignaciones(object sender, EventArgs e)
        {
            try
            {
                if (nuevo || Tipo == "Patente") throw new Exception("Seleccione un perfil o una familia.");
                List<string> elegidos = new List<string>();
                foreach (int indice in lstAsignaciones.CheckedIndices) elegidos.Add(idsAsignaciones[indice]);
                bllRol.mb506GuardarAsignaciones(Tipo, txtId.Text, elegidos);
                mb506CargarArbol();
                Mensajes.mb506Mostrar("Asignaciones guardadas.");
            }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }
    }
}
