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
        // CAMPO: método de transferencia activo
        // ─────────────────────────────────────────────
        private ITransferenciaArchivo? _transferencia;

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
                ActualizarStatus("Listo");
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
        // Credenciales vienen de config.json (nunca hardcodeadas aquí)
        // Para cambiar a SFTP: comenta FTP y descomenta SFTP
        // ─────────────────────────────────────────────
        private void InicializarTransferencia()
        {
            string url = GlobalData.Config.Credenciales.FtpBaseUrl;
            string user = GlobalData.Config.Credenciales.User;
            string pass = GlobalData.Config.Credenciales.Pass;

            // OPCIÓN A — FTP (activo por defecto)
            _transferencia = new TransferenciaFTP(url, user, pass);

            // OPCIÓN B — SFTP (más seguro, requiere SSH.NET)
            // _transferencia = new TransferenciaSFTP(
            //     host:          "162.241.194.172",
            //     puerto:        22,
            //     usuario:       user,
            //     password:      pass,
            //     carpetaRemota: "/backups/"
            // );
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

            // Archivo origen — pendiente parametrizar desde UI
            string archivoLocal = @"D:\PDF-DOC-E001-10420604979669.pdf";

            if (!File.Exists(archivoLocal))
            {
                MessageBox.Show($"No se encontró el archivo:\n{archivoLocal}",
                    "Archivo no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnEnviar.Enabled = false;
            ActualizarStatus("Verificando conexión...");

            try
            {
                // 1. VERIFICAR CONEXIÓN antes de empezar
                bool conectado = await Task.Run(() => _transferencia.ProbarConexion());
                if (!conectado)
                {
                    MessageBox.Show(
                        "No se puede conectar al servidor.\nVerifica las credenciales y la red.",
                        "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2. EJECUTAR el proceso en hilo secundario
                await Task.Run(() => EjecutarBackup(archivoLocal));

                ActualizarStatus("Completado correctamente");
                RegistrarEnLista($"{DateTime.Now:dd/MM/yyyy HH:mm} — Backup OK: {Path.GetFileName(archivoLocal)}");

                MessageBox.Show("Archivo comprimido y subido con éxito.",
                    "Proceso completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ActualizarStatus("Error en el proceso");
                RegistrarEnLista($"{DateTime.Now:dd/MM/yyyy HH:mm} — ERROR: {ex.Message}");
                MessageBox.Show($"Error durante el proceso:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnEnviar.Enabled = true;
            }
        }

        // ─────────────────────────────────────────────
        // LÓGICA PRINCIPAL DE BACKUP
        // 1. Comprimir → 2. Dividir en chunks → 3. Subir → 4. Limpiar
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
                // Siempre limpiar el ZIP temporal, incluso si hay error
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
        // ACTUALIZAR la barra de estado (sttBarraEstado)
        // Funciona desde hilos secundarios con InvokeRequired
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
        // SYSTEM TRAY — minimizar a bandeja del sistema
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
        // MENÚ CONTEXTUAL del system tray
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