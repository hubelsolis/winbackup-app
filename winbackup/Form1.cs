using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;
using winbackup.Core.Transferencia;

namespace winbackup
{
    public partial class Form1 : Form
    {
        // ─────────────────────────────────────────────
        // CAMPOS
        // ─────────────────────────────────────────────
        private ITransferenciaArchivo? _transferencia;
        private TransferenciaSSH? _ssh;
        private string _archivoSeleccionado = string.Empty;

        // Tamaño de cada chunk: 2 MB
        private const int CHUNK_SIZE = 2 * 1024 * 1024;

        // ─────────────────────────────────────────────
        // CONSTRUCTOR
        // ─────────────────────────────────────────────
        public Form1()
        {
            InitializeComponent();
        }

        // ─────────────────────────────────────────────
        // EVENTO: Form cargado
        // ─────────────────────────────────────────────
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                GlobalData.Config = clconfiguracion.Cargar("config.json");
                InicializarTransferencia();
                ActualizarStatus("Listo — Seleccione un archivo para comenzar");
                btnEnviar.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error fatal: No se pudo cargar la configuración.\n" + ex.Message,
                    "Error de inicio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ─────────────────────────────────────────────
        // INICIALIZAR el método de transferencia
        // Todas las credenciales vienen de config.json
        // ─────────────────────────────────────────────
        private void InicializarTransferencia()
        {
            string ftpUrl = GlobalData.Config.Credenciales.FtpBaseUrl;
            string ftpUser = GlobalData.Config.Credenciales.User;
            string ftpPass = GlobalData.Config.Credenciales.Pass;

            // Credenciales locales desde config.json
            string localUser = GlobalData.Config.CredencialesLocal!.User;
            string localPass = GlobalData.Config.CredencialesLocal!.Pass;
            string localHost = GlobalData.Config.CredencialesLocal!.SftpHost;
            string localFolder = GlobalData.Config.CredencialesLocal!.SftpCarpeta;

            // OPCIÓN A — FTP docente (comentado)
             _transferencia = new TransferenciaFTP(ftpUrl, ftpUser, ftpPass);

            // OPCIÓN B — SFTP local ✅ ACTIVO
        /*    _transferencia = new TransferenciaSFTP(
                host: localHost,
                puerto: 22,
                usuario: localUser,
                password: localPass,
                carpetaRemota: localFolder
            );

            // SSH — para verificar archivos después de subir
            _ssh = new TransferenciaSSH(
                host: localHost,
                puerto: 22,
                usuario: localUser,
                password: localPass
            );
       */ }

        // ─────────────────────────────────────────────
        // EVENTO: Botón Seleccionar Archivo
        // ─────────────────────────────────────────────
        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dialogo = new OpenFileDialog
            {
                Title = "Seleccionar archivo para backup",
                Filter = "Todos los archivos (*.*)|*.*|PDF (*.pdf)|*.pdf|Base de datos (*.fdb)|*.fdb|Texto (*.txt)|*.txt",
                FilterIndex = 1,
                Multiselect = false
            };

            if (dialogo.ShowDialog() == DialogResult.OK)
            {
                _archivoSeleccionado = dialogo.FileName;
                lblArchivo.Text = _archivoSeleccionado;
                lblArchivo.ForeColor = System.Drawing.Color.Black;
                btnEnviar.Enabled = true;

                RegistrarEnLista($"{DateTime.Now:dd/MM/yyyy HH:mm} — Archivo seleccionado: {Path.GetFileName(_archivoSeleccionado)}");
                ActualizarStatus($"Archivo listo: {Path.GetFileName(_archivoSeleccionado)}");
            }
        }

        // ─────────────────────────────────────────────
        // EVENTO: Botón Enviar
        // ─────────────────────────────────────────────
        private async void btnEnviar_Click(object sender, EventArgs e)
        {
            if (_transferencia is null)
            {
                MessageBox.Show("El método de transferencia no está inicializado.",
                    "Error de configuración", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(_archivoSeleccionado) || !File.Exists(_archivoSeleccionado))
            {
                MessageBox.Show("Selecciona un archivo válido antes de enviar.",
                    "Archivo no seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnEnviar.Enabled = false;
            btnSeleccionar.Enabled = false;
            ActualizarStatus("Verificando conexión...");

            try
            {
                // 1. VERIFICAR CONEXIÓN
                bool conectado = await Task.Run(() => _transferencia.ProbarConexion());
                if (!conectado)
                {
                    MessageBox.Show(
                        "No se puede conectar al servidor.\n" +
                        "Verifica que el servidor esté activo y las credenciales sean correctas.",
                        "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2. EJECUTAR BACKUP
                await Task.Run(() => EjecutarBackup(_archivoSeleccionado));

                ActualizarStatus("Completado correctamente");
                RegistrarEnLista($"{DateTime.Now:dd/MM/yyyy HH:mm} — Backup OK: {Path.GetFileName(_archivoSeleccionado)}");

                // 3. VERIFICAR CON SSH que los archivos llegaron
                if (_ssh is not null)
                    await Task.Run(() => VerificarConSSH());

                // 4. LIMPIAR selección
                _archivoSeleccionado = string.Empty;
                lblArchivo.Text = "Ningún archivo seleccionado";
                lblArchivo.ForeColor = System.Drawing.Color.Gray;

                MessageBox.Show("Archivo comprimido y subido con éxito.",
                    "Proceso completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ActualizarStatus("Error en el proceso");

                string detalle = ex.Message;
                if (ex.InnerException != null)
                    detalle += $"\nDetalle: {ex.InnerException.Message}";

                RegistrarEnLista($"{DateTime.Now:dd/MM/yyyy HH:mm} — ERROR: {detalle}");
                MessageBox.Show($"Error:\n{detalle}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSeleccionar.Enabled = true;
                btnEnviar.Enabled = !string.IsNullOrEmpty(_archivoSeleccionado);
            }
        }

        // ─────────────────────────────────────────────
        // VERIFICAR con SSH que los archivos llegaron
        // Usa comando Windows: dir
        // ─────────────────────────────────────────────
        private void VerificarConSSH()
        {
            if (_ssh is null) return;

            try
            {
                // Comando Windows para listar la carpeta destino
                string lista = _ssh.EjecutarComando(
                    @"dir C:\Users\WINDOWS\Documents\servidor_STP"
                );

                RegistrarEnLista("── Archivos verificados en servidor (SSH) ──");

                if (string.IsNullOrWhiteSpace(lista))
                {
                    RegistrarEnLista("  Sin respuesta del servidor.");
                    return;
                }

                foreach (string linea in lista.Split('\n'))
                {
                    if (!string.IsNullOrWhiteSpace(linea))
                        RegistrarEnLista("  " + linea.Trim());
                }
            }
            catch (Exception ex)
            {
                RegistrarEnLista($"SSH error: {ex.Message}");
            }
        }

        // ─────────────────────────────────────────────
        // LÓGICA PRINCIPAL DE BACKUP
        // 1. Comprimir → 2. Dividir chunks → 3. Subir → 4. Limpiar
        // ─────────────────────────────────────────────
        private void EjecutarBackup(string archivoLocal)
        {
            string zipPath = Path.ChangeExtension(archivoLocal, ".zip");

            try
            {
                ActualizarStatus("Comprimiendo archivo...");
                Comprimir(archivoLocal, zipPath);
                SubirEnChunks(zipPath);
            }
            finally
            {
                if (File.Exists(zipPath))
                    File.Delete(zipPath);
            }
        }

        // ─────────────────────────────────────────────
        // COMPRIMIR el archivo en un ZIP
        // ─────────────────────────────────────────────
        private static void Comprimir(string archivoOrigen, string zipDestino)
        {
            if (File.Exists(zipDestino))
                File.Delete(zipDestino);

            using ZipArchive zip = ZipFile.Open(zipDestino, ZipArchiveMode.Create);
            zip.CreateEntryFromFile(archivoOrigen, Path.GetFileName(archivoOrigen));
        }

        // ─────────────────────────────────────────────
        // DIVIDIR el ZIP en chunks y subirlos uno a uno
        // ─────────────────────────────────────────────
        private void SubirEnChunks(string zipPath)
        {
            using FileStream fs = new(zipPath, FileMode.Open, FileAccess.Read);

            byte[] buffer = new byte[CHUNK_SIZE];
            int partNumber = 1;
            long totalParts = (long)Math.Ceiling((double)fs.Length / CHUNK_SIZE);
            int bytesRead;

            while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
            {
                string nombreRemoto = $"{Path.GetFileName(zipPath)}.part{partNumber:D3}";
                ActualizarStatus($"Subiendo parte {partNumber} de {totalParts}...");
                _transferencia!.SubirChunk(buffer, bytesRead, nombreRemoto);
                partNumber++;
            }
        }

        // ─────────────────────────────────────────────
        // ACTUALIZAR barra de estado desde cualquier hilo
        // ─────────────────────────────────────────────
        private void ActualizarStatus(string texto)
        {
            void Actualizar()
            {
                sttBarraEstado.Items.Clear();
                sttBarraEstado.Items.Add(new ToolStripStatusLabel(texto));
            }

            if (sttBarraEstado.InvokeRequired)
                sttBarraEstado.Invoke((Action)Actualizar);
            else
                Actualizar();
        }

        // ─────────────────────────────────────────────
        // AGREGAR entrada al log visual (lstRegistro)
        // ─────────────────────────────────────────────
        private void RegistrarEnLista(string mensaje)
        {
            void Agregar() => lstRegistro.Items.Add(mensaje);

            if (lstRegistro.InvokeRequired)
                lstRegistro.Invoke((Action)Agregar);
            else
                Agregar();
        }

        // ─────────────────────────────────────────────
        // SYSTEM TRAY
        // ─────────────────────────────────────────────
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        // ─────────────────────────────────────────────
        // MENÚ CONTEXTUAL
        // ─────────────────────────────────────────────
        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e) { }

        private void resturarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}