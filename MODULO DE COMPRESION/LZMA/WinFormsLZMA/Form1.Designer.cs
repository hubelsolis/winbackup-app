namespace WinFormsLZMA
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.panelCuerpo = new System.Windows.Forms.Panel();
            this.grpComprimir = new System.Windows.Forms.GroupBox();
            this.lblArchivoEntrada = new System.Windows.Forms.Label();
            this.txtArchivoEntrada = new System.Windows.Forms.TextBox();
            this.btnExaminarEntrada = new System.Windows.Forms.Button();
            this.btnComprimir = new System.Windows.Forms.Button();
            this.grpDescomprimir = new System.Windows.Forms.GroupBox();
            this.lblArchivo7z = new System.Windows.Forms.Label();
            this.txtArchivo7z = new System.Windows.Forms.TextBox();
            this.btnExaminar7z = new System.Windows.Forms.Button();
            this.lblCarpetaDestino = new System.Windows.Forms.Label();
            this.txtCarpetaDestino = new System.Windows.Forms.TextBox();
            this.btnExaminarCarpeta = new System.Windows.Forms.Button();
            this.btnDescomprimir = new System.Windows.Forms.Button();
            this.panelResultado = new System.Windows.Forms.Panel();
            this.lblEstadoIcon = new System.Windows.Forms.Label();
            this.lblEstadoTexto = new System.Windows.Forms.Label();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.panelHeader.SuspendLayout();
            this.panelCuerpo.SuspendLayout();
            this.grpComprimir.SuspendLayout();
            this.grpDescomprimir.SuspendLayout();
            this.panelResultado.SuspendLayout();
            this.SuspendLayout();

            // ── panelHeader ──────────────────────────────────────────────────
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(20, 60, 140);
            this.panelHeader.Controls.Add(this.lblSubtitulo);
            this.panelHeader.Controls.Add(this.lblTitulo);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(600, 72);
            this.panelHeader.TabIndex = 0;

            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(16, 10);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(200, 30);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "🗜  Compresor LZMA";

            // lblSubtitulo
            this.lblSubtitulo.AutoSize = true;
            this.lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(180, 210, 255);
            this.lblSubtitulo.Location = new System.Drawing.Point(18, 44);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new System.Drawing.Size(200, 15);
            this.lblSubtitulo.TabIndex = 1;
            this.lblSubtitulo.Text = "Módulo SharpCompress  |  Romero";

            // ── panelCuerpo ──────────────────────────────────────────────────
            this.panelCuerpo.BackColor = System.Drawing.Color.FromArgb(245, 247, 252);
            this.panelCuerpo.Controls.Add(this.grpDescomprimir);
            this.panelCuerpo.Controls.Add(this.grpComprimir);
            this.panelCuerpo.Location = new System.Drawing.Point(0, 72);
            this.panelCuerpo.Name = "panelCuerpo";
            this.panelCuerpo.Size = new System.Drawing.Size(600, 310);
            this.panelCuerpo.TabIndex = 1;

            // ── grpComprimir ─────────────────────────────────────────────────
            this.grpComprimir.BackColor = System.Drawing.Color.White;
            this.grpComprimir.Controls.Add(this.btnComprimir);
            this.grpComprimir.Controls.Add(this.btnExaminarEntrada);
            this.grpComprimir.Controls.Add(this.txtArchivoEntrada);
            this.grpComprimir.Controls.Add(this.lblArchivoEntrada);
            this.grpComprimir.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpComprimir.ForeColor = System.Drawing.Color.FromArgb(20, 60, 140);
            this.grpComprimir.Location = new System.Drawing.Point(12, 12);
            this.grpComprimir.Name = "grpComprimir";
            this.grpComprimir.Size = new System.Drawing.Size(576, 110);
            this.grpComprimir.TabIndex = 0;
            this.grpComprimir.TabStop = false;
            this.grpComprimir.Text = "  COMPRIMIR";

            // lblArchivoEntrada
            this.lblArchivoEntrada.AutoSize = true;
            this.lblArchivoEntrada.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblArchivoEntrada.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblArchivoEntrada.Location = new System.Drawing.Point(10, 26);
            this.lblArchivoEntrada.Name = "lblArchivoEntrada";
            this.lblArchivoEntrada.Size = new System.Drawing.Size(130, 15);
            this.lblArchivoEntrada.TabIndex = 0;
            this.lblArchivoEntrada.Text = "Archivo a comprimir:";

            // txtArchivoEntrada
            this.txtArchivoEntrada.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtArchivoEntrada.Location = new System.Drawing.Point(10, 44);
            this.txtArchivoEntrada.Name = "txtArchivoEntrada";
            this.txtArchivoEntrada.ReadOnly = true;
            this.txtArchivoEntrada.Size = new System.Drawing.Size(460, 23);
            this.txtArchivoEntrada.TabIndex = 1;

            // btnExaminarEntrada
            this.btnExaminarEntrada.BackColor = System.Drawing.Color.FromArgb(220, 230, 255);
            this.btnExaminarEntrada.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExaminarEntrada.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnExaminarEntrada.ForeColor = System.Drawing.Color.FromArgb(20, 60, 140);
            this.btnExaminarEntrada.Location = new System.Drawing.Point(478, 43);
            this.btnExaminarEntrada.Name = "btnExaminarEntrada";
            this.btnExaminarEntrada.Size = new System.Drawing.Size(86, 25);
            this.btnExaminarEntrada.TabIndex = 2;
            this.btnExaminarEntrada.Text = "📂 Buscar";
            this.btnExaminarEntrada.UseVisualStyleBackColor = false;
            this.btnExaminarEntrada.Click += new System.EventHandler(this.btnExaminarEntrada_Click);

            // btnComprimir
            this.btnComprimir.BackColor = System.Drawing.Color.FromArgb(20, 60, 140);
            this.btnComprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComprimir.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnComprimir.ForeColor = System.Drawing.Color.White;
            this.btnComprimir.Location = new System.Drawing.Point(10, 76);
            this.btnComprimir.Name = "btnComprimir";
            this.btnComprimir.Size = new System.Drawing.Size(554, 28);
            this.btnComprimir.TabIndex = 3;
            this.btnComprimir.Text = "▶  COMPRIMIR con LZMA  →  .7z";
            this.btnComprimir.UseVisualStyleBackColor = false;
            this.btnComprimir.Click += new System.EventHandler(this.btnComprimir_Click);

            // ── grpDescomprimir ──────────────────────────────────────────────
            this.grpDescomprimir.BackColor = System.Drawing.Color.White;
            this.grpDescomprimir.Controls.Add(this.btnDescomprimir);
            this.grpDescomprimir.Controls.Add(this.btnExaminarCarpeta);
            this.grpDescomprimir.Controls.Add(this.txtCarpetaDestino);
            this.grpDescomprimir.Controls.Add(this.lblCarpetaDestino);
            this.grpDescomprimir.Controls.Add(this.btnExaminar7z);
            this.grpDescomprimir.Controls.Add(this.txtArchivo7z);
            this.grpDescomprimir.Controls.Add(this.lblArchivo7z);
            this.grpDescomprimir.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpDescomprimir.ForeColor = System.Drawing.Color.FromArgb(20, 120, 70);
            this.grpDescomprimir.Location = new System.Drawing.Point(12, 134);
            this.grpDescomprimir.Name = "grpDescomprimir";
            this.grpDescomprimir.Size = new System.Drawing.Size(576, 158);
            this.grpDescomprimir.TabIndex = 1;
            this.grpDescomprimir.TabStop = false;
            this.grpDescomprimir.Text = "  DESCOMPRIMIR";

            // lblArchivo7z
            this.lblArchivo7z.AutoSize = true;
            this.lblArchivo7z.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblArchivo7z.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblArchivo7z.Location = new System.Drawing.Point(10, 26);
            this.lblArchivo7z.Name = "lblArchivo7z";
            this.lblArchivo7z.Size = new System.Drawing.Size(130, 15);
            this.lblArchivo7z.TabIndex = 0;
            this.lblArchivo7z.Text = "Archivo .7z a extraer:";

            // txtArchivo7z
            this.txtArchivo7z.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtArchivo7z.Location = new System.Drawing.Point(10, 44);
            this.txtArchivo7z.Name = "txtArchivo7z";
            this.txtArchivo7z.ReadOnly = true;
            this.txtArchivo7z.Size = new System.Drawing.Size(460, 23);
            this.txtArchivo7z.TabIndex = 1;

            // btnExaminar7z
            this.btnExaminar7z.BackColor = System.Drawing.Color.FromArgb(210, 240, 220);
            this.btnExaminar7z.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExaminar7z.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnExaminar7z.ForeColor = System.Drawing.Color.FromArgb(20, 120, 70);
            this.btnExaminar7z.Location = new System.Drawing.Point(478, 43);
            this.btnExaminar7z.Name = "btnExaminar7z";
            this.btnExaminar7z.Size = new System.Drawing.Size(86, 25);
            this.btnExaminar7z.TabIndex = 2;
            this.btnExaminar7z.Text = "📂 Buscar";
            this.btnExaminar7z.UseVisualStyleBackColor = false;
            this.btnExaminar7z.Click += new System.EventHandler(this.btnExaminar7z_Click);

            // lblCarpetaDestino
            this.lblCarpetaDestino.AutoSize = true;
            this.lblCarpetaDestino.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCarpetaDestino.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblCarpetaDestino.Location = new System.Drawing.Point(10, 76);
            this.lblCarpetaDestino.Name = "lblCarpetaDestino";
            this.lblCarpetaDestino.Size = new System.Drawing.Size(120, 15);
            this.lblCarpetaDestino.TabIndex = 3;
            this.lblCarpetaDestino.Text = "Carpeta de destino:";

            // txtCarpetaDestino
            this.txtCarpetaDestino.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtCarpetaDestino.Location = new System.Drawing.Point(10, 94);
            this.txtCarpetaDestino.Name = "txtCarpetaDestino";
            this.txtCarpetaDestino.ReadOnly = true;
            this.txtCarpetaDestino.Size = new System.Drawing.Size(460, 23);
            this.txtCarpetaDestino.TabIndex = 4;

            // btnExaminarCarpeta
            this.btnExaminarCarpeta.BackColor = System.Drawing.Color.FromArgb(210, 240, 220);
            this.btnExaminarCarpeta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExaminarCarpeta.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnExaminarCarpeta.ForeColor = System.Drawing.Color.FromArgb(20, 120, 70);
            this.btnExaminarCarpeta.Location = new System.Drawing.Point(478, 93);
            this.btnExaminarCarpeta.Name = "btnExaminarCarpeta";
            this.btnExaminarCarpeta.Size = new System.Drawing.Size(86, 25);
            this.btnExaminarCarpeta.TabIndex = 5;
            this.btnExaminarCarpeta.Text = "📁 Carpeta";
            this.btnExaminarCarpeta.UseVisualStyleBackColor = false;
            this.btnExaminarCarpeta.Click += new System.EventHandler(this.btnExaminarCarpeta_Click);

            // btnDescomprimir
            this.btnDescomprimir.BackColor = System.Drawing.Color.FromArgb(20, 120, 70);
            this.btnDescomprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDescomprimir.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDescomprimir.ForeColor = System.Drawing.Color.White;
            this.btnDescomprimir.Location = new System.Drawing.Point(10, 126);
            this.btnDescomprimir.Name = "btnDescomprimir";
            this.btnDescomprimir.Size = new System.Drawing.Size(554, 28);
            this.btnDescomprimir.TabIndex = 6;
            this.btnDescomprimir.Text = "▶  DESCOMPRIMIR  .7z  →  archivos";
            this.btnDescomprimir.UseVisualStyleBackColor = false;
            this.btnDescomprimir.Click += new System.EventHandler(this.btnDescomprimir_Click);

            // ── panelResultado ───────────────────────────────────────────────
            this.panelResultado.BackColor = System.Drawing.Color.FromArgb(245, 247, 252);
            this.panelResultado.Controls.Add(this.txtResultado);
            this.panelResultado.Controls.Add(this.lblEstadoTexto);
            this.panelResultado.Controls.Add(this.lblEstadoIcon);
            this.panelResultado.Controls.Add(this.progressBar);
            this.panelResultado.Location = new System.Drawing.Point(0, 382);
            this.panelResultado.Name = "panelResultado";
            this.panelResultado.Size = new System.Drawing.Size(600, 148);
            this.panelResultado.TabIndex = 2;

            // progressBar
            this.progressBar.Location = new System.Drawing.Point(12, 8);
            this.progressBar.MarqueeAnimationSpeed = 40;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(576, 10);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 0;
            this.progressBar.Visible = false;

            // lblEstadoIcon
            this.lblEstadoIcon.AutoSize = true;
            this.lblEstadoIcon.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblEstadoIcon.Location = new System.Drawing.Point(12, 22);
            this.lblEstadoIcon.Name = "lblEstadoIcon";
            this.lblEstadoIcon.Size = new System.Drawing.Size(20, 20);
            this.lblEstadoIcon.TabIndex = 1;
            this.lblEstadoIcon.Text = "⏳";

            // lblEstadoTexto
            this.lblEstadoTexto.AutoSize = true;
            this.lblEstadoTexto.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblEstadoTexto.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblEstadoTexto.Location = new System.Drawing.Point(36, 24);
            this.lblEstadoTexto.Name = "lblEstadoTexto";
            this.lblEstadoTexto.Size = new System.Drawing.Size(150, 17);
            this.lblEstadoTexto.TabIndex = 2;
            this.lblEstadoTexto.Text = "Listo para operar...";

            // txtResultado
            this.txtResultado.BackColor = System.Drawing.Color.FromArgb(30, 30, 40);
            this.txtResultado.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtResultado.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.txtResultado.ForeColor = System.Drawing.Color.FromArgb(140, 220, 140);
            this.txtResultado.Location = new System.Drawing.Point(12, 48);
            this.txtResultado.Multiline = true;
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.ReadOnly = true;
            this.txtResultado.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultado.Size = new System.Drawing.Size(576, 90);
            this.txtResultado.TabIndex = 3;
            this.txtResultado.Text = "> Esperando operación...";

            // ── Form1 ────────────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 252);
            this.ClientSize = new System.Drawing.Size(600, 530);
            this.Controls.Add(this.panelResultado);
            this.Controls.Add(this.panelCuerpo);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Compresor LZMA — Romero";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelCuerpo.ResumeLayout(false);
            this.grpComprimir.ResumeLayout(false);
            this.grpComprimir.PerformLayout();
            this.grpDescomprimir.ResumeLayout(false);
            this.grpDescomprimir.PerformLayout();
            this.panelResultado.ResumeLayout(false);
            this.panelResultado.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Panel panelCuerpo;
        private System.Windows.Forms.GroupBox grpComprimir;
        private System.Windows.Forms.Label lblArchivoEntrada;
        private System.Windows.Forms.TextBox txtArchivoEntrada;
        private System.Windows.Forms.Button btnExaminarEntrada;
        private System.Windows.Forms.Button btnComprimir;
        private System.Windows.Forms.GroupBox grpDescomprimir;
        private System.Windows.Forms.Label lblArchivo7z;
        private System.Windows.Forms.TextBox txtArchivo7z;
        private System.Windows.Forms.Button btnExaminar7z;
        private System.Windows.Forms.Label lblCarpetaDestino;
        private System.Windows.Forms.TextBox txtCarpetaDestino;
        private System.Windows.Forms.Button btnExaminarCarpeta;
        private System.Windows.Forms.Button btnDescomprimir;
        private System.Windows.Forms.Panel panelResultado;
        private System.Windows.Forms.Label lblEstadoIcon;
        private System.Windows.Forms.Label lblEstadoTexto;
        private System.Windows.Forms.TextBox txtResultado;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}
