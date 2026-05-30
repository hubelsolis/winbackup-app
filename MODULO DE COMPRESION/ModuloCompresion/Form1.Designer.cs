namespace ModuloCompresion
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
            lblTitulo = new Label();
            lstAlgoritmo = new ListBox();
            btnUsar = new Button();
            lblSubtitulo = new Label();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.BackColor = SystemColors.ButtonFace;
            lblTitulo.Location = new Point(103, 29);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(153, 15);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "MODULO DE COMPRESION";
            // 
            // lstAlgoritmo
            // 
            lstAlgoritmo.FormattingEnabled = true;
            lstAlgoritmo.Location = new Point(38, 102);
            lstAlgoritmo.Name = "lstAlgoritmo";
            lstAlgoritmo.Size = new Size(279, 184);
            lstAlgoritmo.TabIndex = 1;
            // 
            // btnUsar
            // 
            btnUsar.Location = new Point(103, 319);
            btnUsar.Name = "btnUsar";
            btnUsar.Size = new Size(143, 36);
            btnUsar.TabIndex = 2;
            btnUsar.Text = "USAR";
            btnUsar.UseVisualStyleBackColor = true;
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Location = new Point(12, 69);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(195, 15);
            lblSubtitulo.TabIndex = 3;
            lblSubtitulo.Text = "Seleccione el modo de compresion:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(356, 399);
            Controls.Add(lblSubtitulo);
            Controls.Add(btnUsar);
            Controls.Add(lstAlgoritmo);
            Controls.Add(lblTitulo);
            Name = "Form1";
            Text = "Form1";
            btnUsar.Click += new System.EventHandler(this.btnUsar_Click);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private ListBox lstAlgoritmo;
        private Button btnUsar;
        private Label lblSubtitulo;
    }
}
