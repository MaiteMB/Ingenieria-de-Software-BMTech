using System;
using System.Windows.Forms;
using mb506.BLLBMTech;

namespace mb506.BMTech
{
    public partial class mb506FrmBackup : Form
    {
        private BLLBackup bllBackup = new BLLBackup();

        public mb506FrmBackup()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            IdiomasFormulario.mb506Vincular(this);
            AyudaSistema.mb506Vincular(this);
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

        private void mb506Crear(object sender, EventArgs e)
        {
            mb506Accion(0);
        }

        private void mb506Restaurar(object sender, EventArgs e)
        {
            mb506Accion(1);
        }

        private void mb506Actualizar(object sender, EventArgs e)
        {
            mb506Accion(2);
        }

        private void mb506FrmBackup_Load(object sender, EventArgs e)
        {
            if (!DesignMode) mb506Cargar();
        }
    }
}
