using System;
using System.Windows.Forms;
using SharpCompress.Common;

namespace WinFormsAppRAR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBuscarEntrada_Click(object sender, EventArgs e)
        {
            // Preguntamos al usuario qué quiere seleccionar
            DialogResult opcion = MessageBox.Show(
                "¿Qué deseas seleccionar para comprimir?\n\n[Sí] - Un Archivo suelto\n[No] - Una Carpeta completa",
                "Seleccionar Entrada",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (opcion == DialogResult.Yes)
            {
                // Seleccionar un archivo cualquiera
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = "Selecciona el archivo que deseas comprimir";
                    ofd.Filter = "Todos los archivos (*.*)|*.*";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtRutaEntrada.Text = ofd.FileName;
                    }
                }
            }
            else if (opcion == DialogResult.No)
            {
                // Seleccionar una carpeta
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Selecciona la carpeta que deseas comprimir:";
                    fbd.UseDescriptionForTitle = true;

                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        txtRutaEntrada.Text = fbd.SelectedPath;
                    }
                }
            }
        }
        private void btnComprimir_Click(object sender, EventArgs e)
        {
            try
            {
                EstelaRar.Comprimir(txtRutaEntrada.Text, txtRutaSalida.Text);
                lstResultado.Items.Add("✔ Comprimido correctamente.");
            }
            catch (Exception ex)
            {
                lstResultado.Items.Add("✖ Error: " + ex.Message);
            }
        }


        private void btnDescomprimir_Click(object sender, EventArgs e)
        {
            string entrada = txtRutaEntrada.Text.Trim();
            string salida = txtRutaSalida.Text.Trim();

            if (string.IsNullOrEmpty(entrada) || string.IsNullOrEmpty(salida))
            {
                MessageBox.Show("Por favor, selecciona el archivo .rar de entrada y la carpeta de destino.", "Faltan rutas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!System.IO.File.Exists(entrada))
            {
                MessageBox.Show("El archivo .rar de entrada no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                lstResultado.Items.Add("Iniciando descompresión...");
                EstelaRar.Descomprimir(entrada, salida);
                lstResultado.Items.Add("¡Archivos extraídos con éxito en la carpeta de destino!");
                MessageBox.Show("Descompresión completada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lstResultado.Items.Add($"Error al descomprimir: {ex.Message}");
                MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnBuscarSalida_Click(object sender, EventArgs e)
        {
            string rutaEntrada = txtRutaEntrada.Text.Trim();

            // DETECCIÓN AUTOMÁTICA MEJORADA: 
            // Si la entrada existe y es un archivo que termina en .rar -> El usuario va a DESCOMPRIMIR
            if (!string.IsNullOrEmpty(rutaEntrada) && System.IO.File.Exists(rutaEntrada) && rutaEntrada.EndsWith(".rar", StringComparison.OrdinalIgnoreCase))
            {
                using (FolderBrowserDialog dialogoCarpeta = new FolderBrowserDialog())
                {
                    dialogoCarpeta.Description = "Selecciona la carpeta de destino para extraer los archivos:";
                    dialogoCarpeta.UseDescriptionForTitle = true;

                    if (dialogoCarpeta.ShowDialog() == DialogResult.OK)
                    {
                        txtRutaSalida.Text = dialogoCarpeta.SelectedPath;
                    }
                }
            }
            else
            {
                // En cualquier otro caso (si la entrada es carpeta o archivo normal) -> El usuario va a COMPRIMIR
                using (SaveFileDialog dialogoArchivo = new SaveFileDialog())
                {
                    dialogoArchivo.Filter = "Archivos RAR (*.rar)|*.rar";
                    dialogoArchivo.Title = "Selecciona dónde guardar el archivo RAR comprimido";
                    dialogoArchivo.FileName = "nuevo_comprimido.rar";

                    if (dialogoArchivo.ShowDialog() == DialogResult.OK)
                    {
                        txtRutaSalida.Text = dialogoArchivo.FileName;
                    }
                }
            }
        }
        private void lstResultado_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}   