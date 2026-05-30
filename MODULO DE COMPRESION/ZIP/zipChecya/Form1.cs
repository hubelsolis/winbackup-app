using System.IO.Compression;
namespace zipChecya
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void btnEntrada_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Selecciona un archivo o cancela para elegir carpeta";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtEntrada.Text = dlg.FileName;
            }
            else
            {
                FolderBrowserDialog folder = new FolderBrowserDialog();
                folder.Description = "Selecciona una carpeta para comprimir";
                if (folder.ShowDialog() == DialogResult.OK)
                    txtEntrada.Text = folder.SelectedPath;
            }
        }
        private void btnSalida_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Archivo ZIP|*.zip";
            dlg.DefaultExt = "zip";
            dlg.Title = "Guardar ZIP como...";
            if (dlg.ShowDialog() == DialogResult.OK)
                txtSalida.Text = dlg.FileName;
        }
        private void btnComprimir_Click(object sender, EventArgs e)
        {
            if (txtEntrada.Text == "" || txtSalida.Text == "")
            {
                MessageBox.Show("Completa las rutas de entrada y salida.");
                return;
            }
            try
            {
                CompresorZip.Comprimir(txtEntrada.Text, txtSalida.Text);
                lstResultado.Items.Add("Comprimido: " + txtSalida.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al comprimir: " + ex.Message);
            }
        }
        private void btnDescomprimir_Click(object sender, EventArgs e)
        {
            if (txtEntrada.Text == "" || txtSalida.Text == "")
            {
                MessageBox.Show("Completa las rutas.");
                return;
            }
            try
            {
                CompresorZip.Descomprimir(txtEntrada.Text, txtSalida.Text);
                lstResultado.Items.Add("Descomprimido en: " + txtSalida.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al descomprimir: " + ex.Message);
            }
        }
        private void btnVerContenido_Click(object sender, EventArgs e)
        {
            if (txtEntrada.Text == "")
            {
                MessageBox.Show("Selecciona un archivo ZIP en la entrada.");
                return;
            }
            try
            {
                var archivos = CompresorZip.ListarContenido(txtEntrada.Text);
                lstResultado.Items.Clear();
                lstResultado.Items.Add("Contenido de: " + Path.GetFileName(txtEntrada.Text));
                foreach (var a in archivos)
                    lstResultado.Items.Add("   " + a);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void lstResultado_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

    }
}