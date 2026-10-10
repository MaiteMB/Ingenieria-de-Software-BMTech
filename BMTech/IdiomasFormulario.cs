using System;
using System.Collections.Generic;
using System.Windows.Forms;
using mb506.BLLBMTech;
using mb506.ServiciosBMTech.Idiomas;

namespace mb506.BMTech
{
    public class IdiomasFormulario : IObserver
    {
        private Form formulario;
        private Dictionary<string, string> traducciones = new Dictionary<string, string>();
        private Dictionary<object, string> etiquetas = new Dictionary<object, string>();
        private bool preparado;

        private IdiomasFormulario(Form form)
        {
            formulario = form;
            form.Shown += delegate
            {
                preparado = true;
                if (formulario.MinimumSize.IsEmpty)
                    formulario.MinimumSize = formulario.WindowState == FormWindowState.Normal ? formulario.Size : formulario.RestoreBounds.Size;
                mb506VincularGrillas(formulario);
                mb506VincularOpciones(formulario);
                IdiomasStatic.Observer.mb506AgregarObservador(this);
                mb506ActualizarIdioma();
            };
            form.FormClosed += delegate { IdiomasStatic.Observer.mb506EliminarObservador(this); };
            form.Disposed += delegate { IdiomasStatic.Observer.mb506EliminarObservador(this); };
        }

        public static void mb506Vincular(Form formulario)
        {
            new IdiomasFormulario(formulario);
        }

        public void mb506ActualizarIdioma()
        {
            if (!preparado || formulario.IsDisposed) return;
            try
            {
                traducciones = new BLLIdioma().mb506ObtenerTraducciones(IdiomasStatic.Observer.idiomaActual);
                Mensajes.mb506Actualizar();
                formulario.Text = mb506Texto(formulario, formulario.Text);
                mb506RecorrerControles(formulario);
            }
            catch (Exception ex)
            {
                Mensajes.mb506Mostrar("No se pudo cargar el idioma. Revise el script de idiomas. " + ex.Message);
            }
        }

        private string mb506Texto(object elemento, string texto)
        {
            if (!etiquetas.ContainsKey(elemento)) etiquetas.Add(elemento, texto);
            string etiqueta = etiquetas[elemento];
            return traducciones.ContainsKey(etiqueta) ? traducciones[etiqueta] : etiqueta;
        }

        private void mb506VincularGrillas(Control contenedor)
        {
            foreach (Control control in contenedor.Controls)
            {
                DataGridView grilla = control as DataGridView;
                if (grilla != null) grilla.DataBindingComplete += delegate
                {
                    foreach (DataGridViewColumn columna in grilla.Columns)
                        columna.HeaderText = mb506Texto(columna, columna.HeaderText);
                };
                mb506VincularGrillas(control);
            }
        }

        private void mb506VincularOpciones(Control contenedor)
        {
            foreach (Control control in contenedor.Controls)
            {
                ComboBox combo = control as ComboBox;
                if (combo != null && combo.DataSource == null)
                {
                    combo.FormattingEnabled = true;
                    combo.Format += delegate(object sender, ListControlConvertEventArgs e)
                    {
                        if (e.ListItem is string) e.Value = Mensajes.mb506Traducir((string)e.ListItem);
                    };
                }
                mb506VincularOpciones(control);
            }
        }

        private void mb506RecorrerControles(Control contenedor)
        {
            foreach (Control control in contenedor.Controls)
            {
                // No traduce datos ingresados, nombres de clientes ni mensajes variables.
                if (control.Name == "lblMensaje" && control.Tag is string)
                    control.Text = Mensajes.mb506Traducir((string)control.Tag);
                if (control is Button || control is CheckBox || control is TabPage || control is GroupBox ||
                    (control is Label && control.Name != "lblMensaje" &&
                     control.Name != "lblCliente" && control.Name != "lblTotal"))
                {
                    if (control.Tag == null) control.Tag = control.Text;
                    control.Text = mb506Texto(control, Convert.ToString(control.Tag));
                }
                MenuStrip menu = control as MenuStrip;
                if (menu != null) mb506RecorrerMenu(menu.Items);
                DataGridView grilla = control as DataGridView;
                if (grilla != null)
                    foreach (DataGridViewColumn columna in grilla.Columns)
                        columna.HeaderText = mb506Texto(columna, columna.HeaderText);
                mb506RecorrerControles(control);
                ComboBox combo = control as ComboBox;
                if (combo != null && combo.FormattingEnabled)
                {
                    combo.FormattingEnabled = false;
                    combo.FormattingEnabled = true;
                }
            }
        }

        private void mb506RecorrerMenu(ToolStripItemCollection elementos)
        {
            foreach (ToolStripItem elemento in elementos)
            {
                ToolStripMenuItem item = elemento as ToolStripMenuItem;
                if (item == null) continue;
                if (item.Tag == null) item.Tag = item.Text;
                item.Text = mb506Texto(item, Convert.ToString(item.Tag));
                mb506RecorrerMenu(item.DropDownItems);
            }
        }
    }
}
