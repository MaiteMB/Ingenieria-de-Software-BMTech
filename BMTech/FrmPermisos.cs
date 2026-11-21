using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public partial class mb506FrmPermisos : Form
    {
        private BLLRol bllRol = new BLLRol();

        private List<string> idsAsignaciones = new List<string>();
        private bool nuevo = true;

        public mb506FrmPermisos()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            IdiomasFormulario.mb506Vincular(this);
            AyudaSistema.mb506Vincular(this);
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
                idsAsignaciones.Add((Tipo == "Perfil" ? "F:" : "") + Convert.ToString(fila["id"]));
                lstAsignaciones.Items.Add((Tipo == "Perfil" ? "Familia: " : "") + fila["id"] + " - " + fila["nombre"]);
            }
            if (Tipo == "Perfil")
            {
                foreach (DataRow fila in bllRol.mb506ObtenerElementos("Perfil").Rows)
                {
                    idsAsignaciones.Add("P:" + Convert.ToString(fila["id"]));
                    lstAsignaciones.Items.Add("Perfil: " + fila["id"]);
                }
                foreach (DataRow fila in bllRol.mb506ObtenerElementos("Patente").Rows)
                {
                    idsAsignaciones.Add("T:" + Convert.ToString(fila["id"]));
                    lstAsignaciones.Items.Add("Permiso: " + fila["nombre"]);
                }
            }
        }

        private TreeNode mb506Nodo(mb506.BEBMTech.Roles.Rol rol)
        {
            TreeNode nodo = new TreeNode(rol.idRol);
            var familia = rol as mb506.BEBMTech.Roles.Familia;
            if (familia != null)
                foreach (var hijo in familia.Hijos) nodo.Nodes.Add(mb506Nodo(hijo));
            var perfil = rol as mb506.BEBMTech.Roles.Perfil;
            if (perfil != null)
                foreach (var hijo in perfil.Roles) nodo.Nodes.Add(mb506Nodo(hijo));
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

        private void mb506Nuevo(object sender, EventArgs e)
        {
            mb506Limpiar();
        }

        private void mb506TipoCambiado(object sender, EventArgs e)
        {
            if (DesignMode) return;
            try { mb506Cargar(); }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }

        private void mb506FrmPermisos_Load(object sender, EventArgs e)
        {
            if (!DesignMode) cmbTipo.SelectedIndex = 0;
        }

        private void mb506Eliminar(object sender, EventArgs e)
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
        }
    }
}
