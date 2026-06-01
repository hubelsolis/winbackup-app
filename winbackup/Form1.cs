using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.IO.Compression; // Necesario para ZipArchive
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winbackup
{
    public partial class Form1 : Form
    {
        private FileSnapshotCache _snapshotCache;
        private IFileScannerService _fileScanner;
        private IBackupScheduler _scheduler;
        private List<IFileSystemMonitor> _monitores;
        private ILogService _log;

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnEnviar_Click(object sender, EventArgs e)
        {

            //Se realizó una refactorización del módulo de acceso para eliminar
            //las credenciales incrustadas(hard - coded).Se implementó un sistema
            //de gestión de configuración mediante las clases GlobalData,
            //clconfiguracion y clseguridad.Esto permite centralizar los
            //parámetros en un archivo JSON externo y proteger la información
            //sensible mediante cifrado AES, mejorando la mantenibilidad y
            //seguridad del sistema.
            string localFilePath = "D:\\" + "PDF-DOC-E001-10420604979669.pdf";
            string zipPath = Path.ChangeExtension(localFilePath, ".zip");
            string ftpUrl = GlobalData.Config.Credenciales.FtpBaseUrl + "PDF-DOC-E001-10420604979669.zip";
            string user = GlobalData.Config.Credenciales.User;
            string pass = GlobalData.Config.Credenciales.Pass;
            string ftpBaseUrl = GlobalData.Config.Credenciales.FtpBaseUrl;

            // 2MB en bytes
            const int CHUNK_SIZE = 2 * 1024 * 1024;

            btnEnviar.Enabled = false;
            // TODO ANEXARLO A LA BARRA DE ESTADO lblStatus.Text = "Iniciando proceso...";

            try
            {
                await Task.Run(() =>
                {
                    // 1. COMPRESIÓN
                    ActualizarStatus("Comprimiendo...");
                    if (File.Exists(zipPath)) File.Delete(zipPath);
                    using (ZipArchive zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
                    {
                        zip.CreateEntryFromFile(localFilePath, Path.GetFileName(localFilePath));
                    }

                    // 2. DIVISIÓN Y SUBIDA
                    using (FileStream fs = new FileStream(zipPath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = new byte[CHUNK_SIZE];
                        int bytesRead;
                        int partNumber = 1;
                        long totalParts = (long)Math.Ceiling((double)fs.Length / CHUNK_SIZE);

                        while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            string remoteFileName = $"{Path.GetFileName(zipPath)}.part{partNumber:D3}";
                            string fullFtpUrl = ftpBaseUrl + remoteFileName;

                            ActualizarStatus($"Subiendo parte {partNumber} de {totalParts}...");

                            UploadChunk(buffer, bytesRead, fullFtpUrl, user, pass);

                            partNumber++;
                        }
                    }

                    // 3. LIMPIEZA
                    if (File.Exists(zipPath)) File.Delete(zipPath);
                });

                MessageBox.Show("Archivo dividido y subido con éxito.", "Finalizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // TODO ANEXARLO A LA BARRA DE ESTADO lblStatus.Text = "Listo";
                btnEnviar.Enabled = true;
            }
        }

        private void UploadChunk(byte[] data, int length, string remoteUrl, string user, string pass)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(remoteUrl);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(user, pass);
            request.UsePassive = true;
            request.UseBinary = true;

            // Importante: especificar el tamaño exacto del fragmento
            request.ContentLength = length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(data, 0, length);
            }

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                // Verificación exitosa
            }
        }

        // Método auxiliar para actualizar el Label desde un hilo secundario
        private void ActualizarStatus(string texto)
        {
            // TODO ANEXARLO A LA BARRA DE ESTADO 
            /*
            if (lblStatus.InvokeRequired)
                lblStatus.Invoke(new Action(() => lblStatus.Text = texto));
            else
                lblStatus.Text = texto;
             */
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

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

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide(); // Oculta la ventana de la barra de tareas principal
                notifyIcon1.Visible = true; // Asegura que el icono sea visible
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                return;
            }

            _scheduler?.Detener();
            foreach (var m in _monitores ?? Enumerable.Empty<IFileSystemMonitor>())
                m.Detener();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                GlobalData.Config = clconfiguracion.Cargar("config.json");

                _log = new LogService(lstRegistro);
                _fileScanner = new FileScannerService(new DbfFileValidatorService(), _log);
                _snapshotCache = new FileSnapshotCache();
                _monitores = new List<IFileSystemMonitor>();

                var archivosIniciales = _fileScanner.EscanearTodo();
                _snapshotCache.ActualizarDesdeLista(archivosIniciales);

                var sincro = GlobalData.Config.Sincronizacion;
                if (sincro != null && sincro.Habilitado)
                {
                    _scheduler = new BackupScheduler();
                    _scheduler.OnTick += Scheduler_OnTick;
                    _scheduler.Iniciar(sincro.IntervaloSegundos);

                    if (GlobalData.Config.Rutas?.Carpetas != null)
                    {
                        foreach (var ruta in GlobalData.Config.Rutas.Carpetas)
                        {
                            var monitor = new FileSystemMonitor(sincro.DebounceMs, _log);
                            monitor.OnCambioDetectado += Monitor_OnCambioDetectado;
                            monitor.Iniciar(ruta.Origen, ruta.Patron, ruta.IncluirSubcarpetas);
                            _monitores.Add(monitor);
                        }
                    }

                    lstRegistro.Items.Insert(0, $"[{DateTime.Now:HH:mm:ss}] Sincronización iniciada cada {sincro.IntervaloSegundos}s");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fatal: No se pudo cargar la configuración. " + ex.Message);
            }
        }

        private void btnEnviar_Click_1(object sender, EventArgs e)
        {

        }

        private void Scheduler_OnTick()
        {
            try
            {
                var archivosActuales = _fileScanner.EscanearTodo();
                var cambios = _snapshotCache.CompararYDiferenciar(archivosActuales);

                if (cambios.HayCambios)
                {
                    foreach (var n in cambios.Nuevos)
                        _log.Info($"+ {n}");
                    foreach (var m in cambios.Modificados)
                        _log.Info($"~ {m}");
                    foreach (var e in cambios.Eliminados)
                        _log.Info($"- {e}");

                    _snapshotCache.ActualizarDesdeLista(archivosActuales);
                }
            }
            catch (Exception ex)
            {
                _log.Error($"Error en sincronización: {ex.Message}");
            }
        }

        private void Monitor_OnCambioDetectado(FileChangeEvent cambio)
        {
            lstRegistro.Invoke(() =>
            {
                var tipo = cambio.Tipo switch
                {
                    TipoCambio.Creado => "+",
                    TipoCambio.Modificado => "~",
                    TipoCambio.Eliminado => "-",
                    TipoCambio.Renombrado => ">",
                    _ => "?"
                };
                lstRegistro.Items.Insert(0, $"[{cambio.Timestamp:HH:mm:ss}] {tipo} {cambio.Nombre}");

                if (cambio.Tipo == TipoCambio.Eliminado)
                    _snapshotCache.Remover(cambio.Ruta);
            });
        }

        private void comprobarCambiosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Scheduler_OnTick();
            MessageBox.Show("Verificación completada.", "Comprobar Cambios", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
