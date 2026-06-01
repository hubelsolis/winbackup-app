using System.Drawing;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Windows.Forms;

namespace winbackup
{
    public class ConfigSistemaForm : Form
    {
        private Label lblTitulo;
        private GroupBox grpFtp;
        private Label lblUsuario;
        private Label lblContrasena;
        private Label lblFtpBaseUrl;
        private TextBox txtUsuario;
        private TextBox txtContrasena;
        private TextBox txtFtpBaseUrl;
        private GroupBox grpGeneral;
        private Label lblRutaLocal;
        private Label lblIntervalo;
        private TextBox txtRutaLocal;
        private ComboBox cmbIntervalo;
        private Button btnGuardar;
        private Button btnProbarConexion;
        private Button btnCancelar;

        public ConfigSistemaForm()
        {
            InitializeComponent();
            Load += ConfigSistemaForm_Load;
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            grpFtp = new GroupBox();
            lblUsuario = new Label();
            txtUsuario = new TextBox();
            lblContrasena = new Label();
            txtContrasena = new TextBox();
            lblFtpBaseUrl = new Label();
            txtFtpBaseUrl = new TextBox();
            grpGeneral = new GroupBox();
            lblRutaLocal = new Label();
            txtRutaLocal = new TextBox();
            lblIntervalo = new Label();
            cmbIntervalo = new ComboBox();
            btnGuardar = new Button();
            btnProbarConexion = new Button();
            btnCancelar = new Button();

            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitulo.Location = new Point(24, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(245, 25);
            lblTitulo.Text = "Configuración del Sistema";
            // 
            // grpFtp
            // 
            grpFtp.Controls.Add(lblUsuario);
            grpFtp.Controls.Add(txtUsuario);
            grpFtp.Controls.Add(lblContrasena);
            grpFtp.Controls.Add(txtContrasena);
            grpFtp.Controls.Add(lblFtpBaseUrl);
            grpFtp.Controls.Add(txtFtpBaseUrl);
            grpFtp.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            grpFtp.Location = new Point(24, 64);
            grpFtp.Name = "grpFtp";
            grpFtp.Size = new Size(520, 200);
            grpFtp.Text = "Credenciales FTP";
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(20, 32);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(52, 15);
            lblUsuario.Text = "Usuario";
            // 
            // txtUsuario
            // 
            txtUsuario.Location = new Point(20, 54);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(470, 23);
            // 
            // lblContrasena
            // 
            lblContrasena.AutoSize = true;
            lblContrasena.Location = new Point(20, 88);
            lblContrasena.Name = "lblContrasena";
            lblContrasena.Size = new Size(67, 15);
            lblContrasena.Text = "Contraseña";
            // 
            // txtContrasena
            // 
            txtContrasena.Location = new Point(20, 110);
            txtContrasena.Name = "txtContrasena";
            txtContrasena.PasswordChar = '●';
            txtContrasena.Size = new Size(470, 23);
            // 
            // lblFtpBaseUrl
            // 
            lblFtpBaseUrl.AutoSize = true;
            lblFtpBaseUrl.Location = new Point(20, 144);
            lblFtpBaseUrl.Name = "lblFtpBaseUrl";
            lblFtpBaseUrl.Size = new Size(88, 15);
            lblFtpBaseUrl.Text = "Servidor FTP";
            // 
            // txtFtpBaseUrl
            // 
            txtFtpBaseUrl.Location = new Point(20, 166);
            txtFtpBaseUrl.Name = "txtFtpBaseUrl";
            txtFtpBaseUrl.Size = new Size(470, 23);
            // 
            // grpGeneral
            // 
            grpGeneral.Controls.Add(lblRutaLocal);
            grpGeneral.Controls.Add(txtRutaLocal);
            grpGeneral.Controls.Add(lblIntervalo);
            grpGeneral.Controls.Add(cmbIntervalo);
            grpGeneral.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            grpGeneral.Location = new Point(24, 274);
            grpGeneral.Name = "grpGeneral";
            grpGeneral.Size = new Size(520, 120);
            grpGeneral.Text = "General";
            // 
            // lblRutaLocal
            // 
            lblRutaLocal.AutoSize = true;
            lblRutaLocal.Location = new Point(20, 32);
            lblRutaLocal.Name = "lblRutaLocal";
            lblRutaLocal.Size = new Size(85, 15);
            lblRutaLocal.Text = "Ruta local";
            // 
            // txtRutaLocal
            // 
            txtRutaLocal.Location = new Point(20, 54);
            txtRutaLocal.Name = "txtRutaLocal";
            txtRutaLocal.Size = new Size(470, 23);
            // 
            // lblIntervalo
            // 
            lblIntervalo.AutoSize = true;
            lblIntervalo.Location = new Point(20, 88);
            lblIntervalo.Name = "lblIntervalo";
            lblIntervalo.Size = new Size(119, 15);
            lblIntervalo.Text = "Intervalo de copias";
            // 
            // cmbIntervalo
            // 
            cmbIntervalo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIntervalo.Items.AddRange(new object[] { "Diario", "Semanal", "Mensual" });
            cmbIntervalo.Location = new Point(160, 84);
            cmbIntervalo.Name = "cmbIntervalo";
            cmbIntervalo.Size = new Size(330, 23);
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(0, 120, 215);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(360, 396);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(92, 32);
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += BtnGuardar_Click;
            // 
            // btnProbarConexion
            // 
            btnProbarConexion.BackColor = Color.FromArgb(240, 240, 240);
            btnProbarConexion.FlatStyle = FlatStyle.Flat;
            btnProbarConexion.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnProbarConexion.FlatAppearance.BorderSize = 1;
            btnProbarConexion.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnProbarConexion.ForeColor = Color.FromArgb(45, 50, 58);
            btnProbarConexion.Location = new Point(236, 396);
            btnProbarConexion.Name = "btnProbarConexion";
            btnProbarConexion.Size = new Size(120, 32);
            btnProbarConexion.Text = "Probar conexión";
            btnProbarConexion.UseVisualStyleBackColor = false;
            btnProbarConexion.Click += BtnProbarConexion_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(240, 240, 240);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnCancelar.FlatAppearance.BorderSize = 1;
            btnCancelar.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnCancelar.ForeColor = Color.FromArgb(45, 50, 58);
            btnCancelar.Location = new Point(468, 396);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(92, 32);
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += (sender, e) => Close();
            // 
            // ConfigSistemaForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(568, 440);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(btnProbarConexion);
            Controls.Add(grpGeneral);
            Controls.Add(grpFtp);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConfigSistemaForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Configuración del Sistema";
            grpFtp.ResumeLayout(false);
            grpFtp.PerformLayout();
            grpGeneral.ResumeLayout(false);
            grpGeneral.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void ConfigSistemaForm_Load(object? sender, EventArgs e)
        {
            CargarValoresConfiguracion();
        }

        private void CargarValoresConfiguracion()
        {
            try
            {
                var config = GlobalData.Config ?? clconfiguracion.Cargar("config.json");
                txtUsuario.Text = config.Credenciales?.User ?? string.Empty;
                txtContrasena.Text = config.Credenciales?.PassEncriptado ?? string.Empty;
                txtFtpBaseUrl.Text = config.Credenciales?.FtpBaseUrl ?? string.Empty;
            }
            catch
            {
                // Si la configuración no está disponible, no rompemos la carga del formulario.
            }
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Text;
            string ftpBaseUrl = txtFtpBaseUrl.Text.Trim();

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena) || string.IsNullOrWhiteSpace(ftpBaseUrl))
            {
                MessageBox.Show(this, "Complete Usuario, Contraseña y Servidor FTP antes de guardar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var nuevaConfig = new clconfiguracion
                {
                    Credenciales = new CredencialesConfig
                    {
                        User = usuario,
                        PassEncriptado = contrasena,
                        FtpBaseUrl = ftpBaseUrl
                    },
                    Backups = GlobalData.Config?.Backups ?? new BackupsConfig { Cantidad = "4" }
                };

                string json = JsonSerializer.Serialize(nuevaConfig, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("config.json", json);
                GlobalData.Config = nuevaConfig;

                MessageBox.Show(this, "Configuración guardada correctamente.", "Configurar FTP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"No se pudo guardar la configuración. {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnProbarConexion_Click(object? sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Text;
            string ftpBaseUrl = txtFtpBaseUrl.Text.Trim();
            string directorio = "/";

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena) || string.IsNullOrWhiteSpace(ftpBaseUrl))
            {
                MessageBox.Show("Complete usuario, contraseña y servidor FTP para probar la conexión.", "Verificar conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool conectado = ProbarConexionFtp(ftpBaseUrl, usuario, contrasena, directorio, out string errorMessage);
            if (conectado)
            {
                MessageBox.Show("Conexión FTP exitosa.", "Verificar conexión", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"No se pudo establecer conexión FTP. Revise las credenciales y el servidor.\n\nDetalle: {errorMessage}", "Verificar conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ProbarConexionFtp(string ftpBaseUrl, string usuario, string contrasena, string directorio, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                string baseUrl = ftpBaseUrl.Trim();
                if (!baseUrl.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase) && !baseUrl.StartsWith("ftps://", StringComparison.OrdinalIgnoreCase))
                {
                    baseUrl = "ftp://" + baseUrl;
                }

                if (!baseUrl.EndsWith("/"))
                    baseUrl += "/";

                string url = baseUrl + directorio.TrimStart('/');
                if (!url.EndsWith("/"))
                    url += "/";

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(usuario, contrasena);
                request.UsePassive = true;
                request.UseBinary = true;
                request.EnableSsl = false;
                request.KeepAlive = false;

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == FtpStatusCode.OpeningData || response.StatusCode == FtpStatusCode.DataAlreadyOpen || response.StatusCode == FtpStatusCode.CommandOK;
                }
            }
            catch (WebException webEx)
            {
                errorMessage = webEx.GetBaseException().Message;
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.GetBaseException().Message;
                return false;
            }
        }
    }
}
