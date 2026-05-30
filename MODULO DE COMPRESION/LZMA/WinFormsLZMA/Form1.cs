namespace WinFormsLZMA;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    // ─── COMPRIMIR ────────────────────────────────────────────────────────────

    private void btnExaminarEntrada_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title  = "Selecciona el archivo a comprimir",
            Filter = "Todos los archivos|*.*"
        };
        if (dlg.ShowDialog() == DialogResult.OK)
            txtArchivoEntrada.Text = dlg.FileName;
    }

    private void btnComprimir_Click(object sender, EventArgs e)
    {
        if (!ValidarArchivo(txtArchivoEntrada.Text)) return;

        using var dlgGuardar = new SaveFileDialog
        {
            Title    = "Guardar archivo comprimido",
            Filter   = "Archivo LZMA 7z|*.7z",
            FileName = Path.GetFileNameWithoutExtension(txtArchivoEntrada.Text) + ".7z"
        };
        if (dlgGuardar.ShowDialog() != DialogResult.OK) return;

        IniciarOperacion("Comprimiendo con LZMA...");

        Task.Run(() =>
        {
            try
            {
                var (orig, comp, pct) = RomeroLzma.Comprimir(
                    txtArchivoEntrada.Text, dlgGuardar.FileName);

                Invoke(() => MostrarExito(
                    "✔  Compresión completada",
                    $"> Archivo  : {Path.GetFileName(dlgGuardar.FileName)}\n" +
                    $"> Original : {RomeroLzma.FormatBytes(orig)}\n" +
                    $"> Comprimido: {RomeroLzma.FormatBytes(comp)}\n" +
                    $"> Reducción : {pct:F2} %\n" +
                    $"> Guardado en: {dlgGuardar.FileName}"));
            }
            catch (Exception ex)
            {
                Invoke(() => MostrarError("✘  Error al comprimir", ex.Message));
            }
        });
    }

    // ─── DESCOMPRIMIR ─────────────────────────────────────────────────────────

    private void btnExaminar7z_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title  = "Selecciona el archivo .7z",
            Filter = "Archivo 7z|*.7z|Todos|*.*"
        };
        if (dlg.ShowDialog() == DialogResult.OK)
            txtArchivo7z.Text = dlg.FileName;
    }

    private void btnExaminarCarpeta_Click(object sender, EventArgs e)
    {
        using var dlg = new FolderBrowserDialog
        {
            Description = "Selecciona la carpeta de destino"
        };
        if (dlg.ShowDialog() == DialogResult.OK)
            txtCarpetaDestino.Text = dlg.SelectedPath;
    }

    private void btnDescomprimir_Click(object sender, EventArgs e)
    {
        if (!ValidarArchivo(txtArchivo7z.Text)) return;
        if (string.IsNullOrWhiteSpace(txtCarpetaDestino.Text))
        {
            MessageBox.Show("Selecciona una carpeta de destino.", "Aviso",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        IniciarOperacion("Descomprimiendo...");

        Task.Run(() =>
        {
            try
            {
                RomeroLzma.Descomprimir(txtArchivo7z.Text, txtCarpetaDestino.Text);

                Invoke(() => MostrarExito(
                    "✔  Descompresión completada",
                    $"> Archivo  : {Path.GetFileName(txtArchivo7z.Text)}\n" +
                    $"> Extraído en: {txtCarpetaDestino.Text}"));
            }
            catch (Exception ex)
            {
                Invoke(() => MostrarError("✘  Error al descomprimir", ex.Message));
            }
        });
    }

    // ─── UI helpers ───────────────────────────────────────────────────────────

    private void IniciarOperacion(string mensaje)
    {
        btnComprimir.Enabled    = false;
        btnDescomprimir.Enabled = false;
        progressBar.Visible     = true;
        lblEstadoIcon.Text      = "⏳";
        lblEstadoTexto.Text     = mensaje;
        lblEstadoTexto.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
        txtResultado.Text       = "> Procesando...";
        txtResultado.ForeColor  = System.Drawing.Color.FromArgb(200, 200, 100);
    }

    private void MostrarExito(string estado, string detalle)
    {
        progressBar.Visible     = false;
        btnComprimir.Enabled    = true;
        btnDescomprimir.Enabled = true;
        lblEstadoIcon.Text      = "✅";
        lblEstadoTexto.Text     = estado;
        lblEstadoTexto.ForeColor = System.Drawing.Color.FromArgb(20, 120, 70);
        txtResultado.Text       = detalle;
        txtResultado.ForeColor  = System.Drawing.Color.FromArgb(140, 220, 140);
    }

    private void MostrarError(string estado, string mensaje)
    {
        progressBar.Visible     = false;
        btnComprimir.Enabled    = true;
        btnDescomprimir.Enabled = true;
        lblEstadoIcon.Text      = "❌";
        lblEstadoTexto.Text     = estado;
        lblEstadoTexto.ForeColor = System.Drawing.Color.FromArgb(180, 40, 40);
        txtResultado.Text       = $"> ERROR: {mensaje}";
        txtResultado.ForeColor  = System.Drawing.Color.FromArgb(255, 100, 100);
    }

    private static bool ValidarArchivo(string ruta)
    {
        if (string.IsNullOrWhiteSpace(ruta) || !File.Exists(ruta))
        {
            MessageBox.Show("Selecciona un archivo válido primero.", "Aviso",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }
}
