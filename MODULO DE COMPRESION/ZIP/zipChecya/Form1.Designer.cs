namespace zipChecya
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnEntrada = new Button();
            btnSalida = new Button();
            lblEntrada = new Label();
            lblSalida = new Label();
            txtEntrada = new TextBox();
            txtSalida = new TextBox();
            btnComprimir = new Button();
            btnDescomprimir = new Button();
            btnVerContenido = new Button();
            lstResultado = new ListBox();
            SuspendLayout();
            // 
            // btnEntrada
            // 
            btnEntrada.Location = new Point(452, 38);
            btnEntrada.Name = "btnEntrada";
            btnEntrada.Size = new Size(75, 23);
            btnEntrada.TabIndex = 0;
            btnEntrada.Text = "...";
            btnEntrada.UseVisualStyleBackColor = true;
            btnEntrada.Click += btnEntrada_Click;
            // 
            // btnSalida
            // 
            btnSalida.Location = new Point(452, 84);
            btnSalida.Name = "btnSalida";
            btnSalida.Size = new Size(75, 23);
            btnSalida.TabIndex = 1;
            btnSalida.Text = "...";
            btnSalida.UseVisualStyleBackColor = true;
            btnSalida.Click += btnSalida_Click;
            // 
            // lblEntrada
            // 
            lblEntrada.AutoSize = true;
            lblEntrada.Location = new Point(39, 42);
            lblEntrada.Name = "lblEntrada";
            lblEntrada.Size = new Size(93, 15);
            lblEntrada.TabIndex = 2;
            lblEntrada.Text = "Ruta de Entrada:";
            // 
            // lblSalida
            // 
            lblSalida.AutoSize = true;
            lblSalida.Location = new Point(39, 88);
            lblSalida.Name = "lblSalida";
            lblSalida.Size = new Size(116, 15);
            lblSalida.TabIndex = 3;
            lblSalida.Text = "Ruta de Salida (.zip) :";
            // 
            // txtEntrada
            // 
            txtEntrada.Location = new Point(166, 39);
            txtEntrada.Name = "txtEntrada";
            txtEntrada.Size = new Size(271, 23);
            txtEntrada.TabIndex = 4;
            // 
            // txtSalida
            // 
            txtSalida.Location = new Point(166, 85);
            txtSalida.Name = "txtSalida";
            txtSalida.Size = new Size(271, 23);
            txtSalida.TabIndex = 5;
            // 
            // btnComprimir
            // 
            btnComprimir.Location = new Point(26, 188);
            btnComprimir.Name = "btnComprimir";
            btnComprimir.Size = new Size(158, 48);
            btnComprimir.TabIndex = 6;
            btnComprimir.Text = "Comprimir";
            btnComprimir.UseVisualStyleBackColor = true;
            btnComprimir.Click += btnComprimir_Click;
            // 
            // btnDescomprimir
            // 
            btnDescomprimir.Location = new Point(190, 188);
            btnDescomprimir.Name = "btnDescomprimir";
            btnDescomprimir.Size = new Size(147, 48);
            btnDescomprimir.TabIndex = 7;
            btnDescomprimir.Text = "Descomprimir";
            btnDescomprimir.UseVisualStyleBackColor = true;
            btnDescomprimir.Click += btnDescomprimir_Click;
            // 
            // btnVerContenido
            // 
            btnVerContenido.Location = new Point(343, 190);
            btnVerContenido.Name = "btnVerContenido";
            btnVerContenido.Size = new Size(141, 46);
            btnVerContenido.TabIndex = 8;
            btnVerContenido.Text = "Ver Contenido";
            btnVerContenido.UseVisualStyleBackColor = true;
            btnVerContenido.Click += btnVerContenido_Click;
            // 
            // lstResultado
            // 
            lstResultado.FormattingEnabled = true;
            lstResultado.Location = new Point(507, 141);
            lstResultado.Name = "lstResultado";
            lstResultado.Size = new Size(196, 259);
            lstResultado.TabIndex = 9;
            lstResultado.SelectedIndexChanged += lstResultado_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(715, 416);
            Controls.Add(lstResultado);
            Controls.Add(btnVerContenido);
            Controls.Add(btnDescomprimir);
            Controls.Add(btnComprimir);
            Controls.Add(txtSalida);
            Controls.Add(txtEntrada);
            Controls.Add(lblSalida);
            Controls.Add(lblEntrada);
            Controls.Add(btnSalida);
            Controls.Add(btnEntrada);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnEntrada;
        private Button btnSalida;
        private Label lblEntrada;
        private Label lblSalida;
        private TextBox txtEntrada;
        private TextBox txtSalida;
        private Button btnComprimir;
        private Button btnDescomprimir;
        private Button btnVerContenido;
        private ListBox lstResultado;
    }
}
