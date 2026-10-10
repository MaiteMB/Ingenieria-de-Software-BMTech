using System;
using System.Drawing;
using System.Windows.Forms;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public class mb506FrmBackup : Form
    {
        private DataGridView dgvBackups = new DataGridView();
        private BLLBackup bllBackup = new BLLBackup();

        public mb506FrmBackup()
        {
            IdiomasFormulario.mb506Vincular(this);
            Text = "Copias de seguridad";
            ClientSize = new Size(950, 510);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 10);
            dgvBackups.SetBounds(20, 20, 910, 405);
            dgvBackups.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvBackups.ReadOnly = true;
            dgvBackups.AllowUserToAddRows = false;
            dgvBackups.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            string[] acciones = { "Crear copia", "Restaurar copia", "Actualizar" };
            for (int i = 0; i < acciones.Length; i++)
            {
                Button boton = new Button { Text = acciones[i], BackColor = Color.Teal, ForeColor = Color.White };
                boton.SetBounds(20 + i * 180, 450, 160, 36);
                boton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
                int accion = i;
                boton.Click += delegate { mb506Accion(accion); };
                Controls.Add(boton);
            }
            Controls.Add(dgvBackups);
            Load += delegate { mb506Cargar(); };
        }

        private void mb506Cargar()
        {
            try { dgvBackups.DataSource = bllBackup.mb506ObtenerBackups(); }
            catch (Exception ex) { Mensajes.mb506Mostrar(ex.Message); }
        }

        private void mb506Accion(int accion)
        {
            try
            {
                if (accion == 0)
                {
                    using (FolderBrowserDialog dialogo = new FolderBrowserDialog())
                    {
                        if (dialogo.ShowDialog(this) != DialogResult.OK) return;
                        Mensajes.mb506Mostrar("SQL Server debe tener permiso de escritura en la carpeta. Conserve juntos el .bak y el .key en un lugar privado.");
                        Cursor = Cursors.WaitCursor;
                        string ruta = bllBackup.mb506Crear(dialogo.SelectedPath);
                        Mensajes.mb506Mostrar("Copia creada: " + ruta);
                    }
                }
                else if (accion == 1)
                {
                    using (OpenFileDialog dialogo = new OpenFileDialog { Filter = "BMTech backup (*.bak)|*.bak" })
                    {
                        if (dialogo.ShowDialog(this) != DialogResult.OK) return;
                        if (Mensajes.mb506Mostrar("Restaurar reemplaza los datos actuales por los de esta copia y cierra el sistema. Haga antes una copia actual. Continuar?",
                            "Restaurar copia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                        Cursor = Cursors.WaitCursor;
                        bllBackup.mb506Restaurar(dialogo.FileName);
                        Mensajes.mb506Mostrar("Base restaurada. Abra nuevamente el sistema para verificar integridad e iniciar sesion.");
                        Application.Exit();
                        return;
                    }
                }
                mb506Cargar();
            }
            catch (Exception ex)
            {
                Mensajes.mb506Mostrar(ex.Message);
                if (!mb506.ServiciosBMTech.Seguridad.SessionManager.IsSessionActive) Application.Exit();
            }
            finally { Cursor = Cursors.Default; }
        }
    }
}
