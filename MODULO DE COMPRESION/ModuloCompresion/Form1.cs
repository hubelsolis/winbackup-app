using System.Diagnostics;

namespace ModuloCompresion
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            AplicarEstilo();
            CargarOpciones();
        }

        private void AplicarEstilo()
        {
            // ── Ventana ──────────────────────────────────────────────
            this.Text = "Módulo de Compresión";
            this.Size = new Size(420, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(18, 32, 58);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // ── Ícono ZIP (emoji como label) ─────────────────────────
            Label lblIcono = new Label();
            lblIcono.Text = "🗜";
            lblIcono.Font = new Font("Segoe UI Emoji", 40);
            lblIcono.ForeColor = Color.White;
            lblIcono.AutoSize = true;
            lblIcono.Location = new Point(160, 30);
            this.Controls.Add(lblIcono);

            // ── Título ───────────────────────────────────────────────
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.BackColor = Color.Transparent;
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(90, 110);

            // ── Subtítulo ────────────────────────────────────────────
            lblSubtitulo.Font = new Font("Segoe UI", 10);
            lblSubtitulo.ForeColor = Color.FromArgb(150, 180, 220);
            lblSubtitulo.BackColor = Color.Transparent;
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Location = new Point(95, 160);

            // ── ListBox ──────────────────────────────────────────────
            lstAlgoritmo.Font = new Font("Segoe UI", 13);
            lstAlgoritmo.BackColor = Color.FromArgb(25, 45, 80);
            lstAlgoritmo.ForeColor = Color.White;
            lstAlgoritmo.BorderStyle = BorderStyle.None;
            lstAlgoritmo.Size = new Size(280, 100);
            lstAlgoritmo.Location = new Point(65, 195);

            // ── Botón Usar ───────────────────────────────────────────
            btnUsar.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            btnUsar.BackColor = Color.FromArgb(0, 180, 216);
            btnUsar.ForeColor = Color.White;
            btnUsar.FlatStyle = FlatStyle.Flat;
            btnUsar.FlatAppearance.BorderSize = 0;
            btnUsar.Size = new Size(200, 45);
            btnUsar.Location = new Point(105, 330);
            btnUsar.Cursor = Cursors.Hand;
        }

        private void CargarOpciones()
        {
            lstAlgoritmo.Items.Add("ZIP");
            lstAlgoritmo.Items.Add("RAR");
            lstAlgoritmo.Items.Add("LZMA");
            lstAlgoritmo.SelectedIndex = 0;
        }

        private void btnUsar_Click(object sender, EventArgs e)
        {
            if (lstAlgoritmo.SelectedItem == null)
            {
                MessageBox.Show("Selecciona un algoritmo.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string seleccion = lstAlgoritmo.SelectedItem.ToString();

            Form formulario = seleccion switch
            {
                "ZIP" => new zipChecya.Form1(),
                "RAR" => new WinFormsAppRAR.Form1(),
                "LZMA" => new WinFormsLZMA.Form1(),
                _ => null
            };

            if (formulario != null)
            {
                formulario.Show();
            }
        }
    }
}