namespace WinFormsAppRAR
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtRutaEntrada;
        private System.Windows.Forms.TextBox txtRutaSalida;
        private System.Windows.Forms.Button btnBuscarEntrada;
        private System.Windows.Forms.Button btnComprimir;
        private System.Windows.Forms.Button btnDescomprimir;
        private System.Windows.Forms.Button btnBuscarSalida; // <-- AGREGADO AQUÍ
        private System.Windows.Forms.ListBox lstResultado;
        private System.Windows.Forms.Label lblEntrada;
        private System.Windows.Forms.Label lblSalida;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblResultados;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtRutaEntrada = new TextBox();
            txtRutaSalida = new TextBox();
            btnBuscarEntrada = new Button();
            btnComprimir = new Button();
            btnDescomprimir = new Button();
            btnBuscarSalida = new Button();
            lstResultado = new ListBox();
            lblEntrada = new Label();
            lblSalida = new Label();
            lblTitulo = new Label();
            lblResultados = new Label();
            SuspendLayout();
            // 
            // txtRutaEntrada
            // 
            txtRutaEntrada.Font = new Font("Segoe UI", 10F);
            txtRutaEntrada.Location = new Point(20, 100);
            txtRutaEntrada.Name = "txtRutaEntrada";
            txtRutaEntrada.Size = new Size(380, 30);
            txtRutaEntrada.TabIndex = 0;
            // 
            // txtRutaSalida
            // 
            txtRutaSalida.Font = new Font("Segoe UI", 10F);
            txtRutaSalida.Location = new Point(20, 170);
            txtRutaSalida.Name = "txtRutaSalida";
            txtRutaSalida.Size = new Size(380, 30);
            txtRutaSalida.TabIndex = 2;
            // 
            // btnBuscarEntrada
            // 
            btnBuscarEntrada.BackColor = Color.SteelBlue;
            btnBuscarEntrada.FlatStyle = FlatStyle.Flat;
            btnBuscarEntrada.Font = new Font("Segoe UI", 9F);
            btnBuscarEntrada.ForeColor = Color.White;
            btnBuscarEntrada.Location = new Point(410, 99);
            btnBuscarEntrada.Name = "btnBuscarEntrada";
            btnBuscarEntrada.Size = new Size(80, 29);
            btnBuscarEntrada.TabIndex = 1;
            btnBuscarEntrada.Text = "📂 Buscar";
            btnBuscarEntrada.UseVisualStyleBackColor = false;
            btnBuscarEntrada.Click += btnBuscarEntrada_Click;
            // 
            // btnComprimir
            // 
            btnComprimir.BackColor = Color.SeaGreen;
            btnComprimir.FlatStyle = FlatStyle.Flat;
            btnComprimir.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnComprimir.ForeColor = Color.White;
            btnComprimir.Location = new Point(20, 220);
            btnComprimir.Name = "btnComprimir";
            btnComprimir.Size = new Size(235, 35);
            btnComprimir.TabIndex = 3;
            btnComprimir.Text = "🗜 Comprimir RAR";
            btnComprimir.UseVisualStyleBackColor = false;
            btnComprimir.Click += btnComprimir_Click;
            // 
            // btnDescomprimir
            // 
            btnDescomprimir.BackColor = Color.DarkOrange;
            btnDescomprimir.FlatStyle = FlatStyle.Flat;
            btnDescomprimir.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDescomprimir.ForeColor = Color.White;
            btnDescomprimir.Location = new Point(281, 220);
            btnDescomprimir.Name = "btnDescomprimir";
            btnDescomprimir.Size = new Size(233, 35);
            btnDescomprimir.TabIndex = 4;
            btnDescomprimir.Text = "📦 Descomprimir RAR";
            btnDescomprimir.UseVisualStyleBackColor = false;
            btnDescomprimir.Click += btnDescomprimir_Click;
            // 
            // btnBuscarSalida
            // 
            btnBuscarSalida.BackColor = Color.SteelBlue;
            btnBuscarSalida.FlatStyle = FlatStyle.Flat;
            btnBuscarSalida.Font = new Font("Segoe UI", 9F);
            btnBuscarSalida.ForeColor = Color.White;
            btnBuscarSalida.Location = new Point(410, 170);
            btnBuscarSalida.Name = "btnBuscarSalida";
            btnBuscarSalida.Size = new Size(80, 29);
            btnBuscarSalida.TabIndex = 6;
            btnBuscarSalida.Text = "📂 Buscar";
            btnBuscarSalida.UseVisualStyleBackColor = false;
            btnBuscarSalida.Click += btnBuscarSalida_Click;
            // 
            // lstResultado
            // 
            lstResultado.Font = new Font("Segoe UI", 10F);
            lstResultado.Location = new Point(20, 300);
            lstResultado.Name = "lstResultado";
            lstResultado.Size = new Size(560, 119);
            lstResultado.TabIndex = 5;
            // 
            // lblEntrada
            // 
            lblEntrada.Font = new Font("Segoe UI", 10F);
            lblEntrada.Location = new Point(20, 75);
            lblEntrada.Name = "lblEntrada";
            lblEntrada.Size = new Size(235, 25);
            lblEntrada.TabIndex = 1;
            lblEntrada.Text = "Archivo de entrada:";
            // 
            // lblSalida
            // 
            lblSalida.Font = new Font("Segoe UI", 10F);
            lblSalida.Location = new Point(20, 145);
            lblSalida.Name = "lblSalida";
            lblSalida.Size = new Size(320, 25);
            lblSalida.TabIndex = 2;
            lblSalida.Text = "Carpeta o archivo de salida:";
            // 
            // lblTitulo
            // 
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.DarkBlue;
            lblTitulo.Location = new Point(20, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(300, 35);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "🗜 Compresor RAR";
            // 
            // lblResultados
            // 
            lblResultados.Font = new Font("Segoe UI", 10F);
            lblResultados.Location = new Point(20, 275);
            lblResultados.Name = "lblResultados";
            lblResultados.Size = new Size(137, 25);
            lblResultados.TabIndex = 5;
            lblResultados.Text = "Resultados:";
            // 
            // Form1
            // 
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(620, 450);
            Controls.Add(lblTitulo);
            Controls.Add(lblEntrada);
            Controls.Add(txtRutaEntrada);
            Controls.Add(btnBuscarEntrada);
            Controls.Add(lblSalida);
            Controls.Add(txtRutaSalida);
            Controls.Add(btnComprimir);
            Controls.Add(btnDescomprimir);
            Controls.Add(btnBuscarSalida);
            Controls.Add(lblResultados);
            Controls.Add(lstResultado);
            Name = "Form1";
            Text = "Compresor RAR - WinForms";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}