namespace winbackup
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            lstRegistro = new ListBox();
            notifyIcon1 = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            resturarToolStripMenuItem = new ToolStripMenuItem();
            salirToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1 = new MenuStrip();
            copiaSeguridadToolStripMenuItem = new ToolStripMenuItem();
            generarAhoraToolStripMenuItem = new ToolStripMenuItem();
            comprobarCambiosToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            salirToolStripMenuItem1 = new ToolStripMenuItem();
            configuracionToolStripMenuItem = new ToolStripMenuItem();
            configuracionSistemaToolStripMenuItem = new ToolStripMenuItem();
            configuracionDeCopiasToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            ayudaToolStripMenuItem = new ToolStripMenuItem();
            acercaDeToolStripMenuItem = new ToolStripMenuItem();
            sttBarraEstado = new StatusStrip();
            lstCopias = new ListBox();
            label1 = new Label();
            panel1 = new Panel();
            btnEnviar = new Button();
            contextMenuStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // lstRegistro
            // 
            lstRegistro.FormattingEnabled = true;
            lstRegistro.ItemHeight = 15;
            lstRegistro.Location = new Point(12, 352);
            lstRegistro.Name = "lstRegistro";
            lstRegistro.Size = new Size(469, 109);
            lstRegistro.TabIndex = 4;
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "Sistema de Respaldo V0.1";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { resturarToolStripMenuItem, salirToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(118, 48);
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            // 
            // resturarToolStripMenuItem
            // 
            resturarToolStripMenuItem.Name = "resturarToolStripMenuItem";
            resturarToolStripMenuItem.Size = new Size(117, 22);
            resturarToolStripMenuItem.Text = "Resturar";
            resturarToolStripMenuItem.Click += resturarToolStripMenuItem_Click;
            // 
            // salirToolStripMenuItem
            // 
            salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            salirToolStripMenuItem.Size = new Size(117, 22);
            salirToolStripMenuItem.Text = "Salir";
            salirToolStripMenuItem.Click += salirToolStripMenuItem_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { copiaSeguridadToolStripMenuItem, configuracionToolStripMenuItem, toolStripMenuItem2 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(493, 24);
            menuStrip1.TabIndex = 9;
            menuStrip1.Text = "menuStrip1";
            // 
            // copiaSeguridadToolStripMenuItem
            // 
            copiaSeguridadToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { generarAhoraToolStripMenuItem, comprobarCambiosToolStripMenuItem, toolStripMenuItem1, salirToolStripMenuItem1 });
            copiaSeguridadToolStripMenuItem.Name = "copiaSeguridadToolStripMenuItem";
            copiaSeguridadToolStripMenuItem.Size = new Size(106, 20);
            copiaSeguridadToolStripMenuItem.Text = "Copia Seguridad";
            // 
            // generarAhoraToolStripMenuItem
            // 
            generarAhoraToolStripMenuItem.Name = "generarAhoraToolStripMenuItem";
            generarAhoraToolStripMenuItem.Size = new Size(185, 22);
            generarAhoraToolStripMenuItem.Text = "Generar Ahora";
            // 
            // comprobarCambiosToolStripMenuItem
            // 
            comprobarCambiosToolStripMenuItem.Name = "comprobarCambiosToolStripMenuItem";
            comprobarCambiosToolStripMenuItem.Size = new Size(185, 22);
            comprobarCambiosToolStripMenuItem.Text = "Comprobar Cambios";
            comprobarCambiosToolStripMenuItem.Click += comprobarCambiosToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(182, 6);
            // 
            // salirToolStripMenuItem1
            // 
            salirToolStripMenuItem1.Name = "salirToolStripMenuItem1";
            salirToolStripMenuItem1.Size = new Size(185, 22);
            salirToolStripMenuItem1.Text = "Salir";
            // 
            // configuracionToolStripMenuItem
            // 
            configuracionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { configuracionSistemaToolStripMenuItem, configuracionDeCopiasToolStripMenuItem });
            configuracionToolStripMenuItem.Name = "configuracionToolStripMenuItem";
            configuracionToolStripMenuItem.Size = new Size(95, 20);
            configuracionToolStripMenuItem.Text = "Configuracion";
            // 
            // configuracionSistemaToolStripMenuItem
            // 
            configuracionSistemaToolStripMenuItem.Name = "configuracionSistemaToolStripMenuItem";
            configuracionSistemaToolStripMenuItem.Size = new Size(205, 22);
            configuracionSistemaToolStripMenuItem.Text = "Configuracion Sistema";
            // 
            // configuracionDeCopiasToolStripMenuItem
            // 
            configuracionDeCopiasToolStripMenuItem.Name = "configuracionDeCopiasToolStripMenuItem";
            configuracionDeCopiasToolStripMenuItem.Size = new Size(205, 22);
            configuracionDeCopiasToolStripMenuItem.Text = "Configuracion de Copias";
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.DropDownItems.AddRange(new ToolStripItem[] { ayudaToolStripMenuItem, acercaDeToolStripMenuItem });
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(24, 20);
            toolStripMenuItem2.Text = "?";
            // 
            // ayudaToolStripMenuItem
            // 
            ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            ayudaToolStripMenuItem.Size = new Size(138, 22);
            ayudaToolStripMenuItem.Text = "Ayuda";
            // 
            // acercaDeToolStripMenuItem
            // 
            acercaDeToolStripMenuItem.Name = "acercaDeToolStripMenuItem";
            acercaDeToolStripMenuItem.Size = new Size(138, 22);
            acercaDeToolStripMenuItem.Text = "Acerca de ...";
            // 
            // sttBarraEstado
            // 
            sttBarraEstado.Location = new Point(0, 477);
            sttBarraEstado.Name = "sttBarraEstado";
            sttBarraEstado.Size = new Size(493, 22);
            sttBarraEstado.TabIndex = 10;
            sttBarraEstado.Text = "statusStrip1";
            // 
            // lstCopias
            // 
            lstCopias.FormattingEnabled = true;
            lstCopias.ItemHeight = 15;
            lstCopias.Items.AddRange(new object[] { "DINAMICOS", "STORE.FDB         COPIA1        25/05/2026 12:50PM     1 DE 5 ENVIADOS    EN CURSO", "STORE.FDB         COPIA2        25/05/2026 02:50PM     5 DE 5 ENVIADOS    OK", "STORE.FDB         COPIA3        25/05/2026 01:50PM     5 DE 5 ENVIADOS    OK", "STORE.FDB         COPIA4        25/05/2026 07:00PM     5 DE 5 ENVIADOS    OK", "STORE.FDB         COPIA5        25/05/2026 05:00PM     5 DE 5 ENVIADOS    OK", "STORE.FDB         COPIA6        25/05/2026 04:50PM     5 DE 5 ENVIADOS    OK", "", "ESTATICOS", "FACTURA.FR3    COPIA1        25/05/2026  12:12AM    COPIA UNICA", "BOLETA.FR3       COPIA1        25/05/2026  12:12AM    COPIA UNICA", "", "" });
            lstCopias.Location = new Point(12, 117);
            lstCopias.Name = "lstCopias";
            lstCopias.Size = new Size(469, 229);
            lstCopias.TabIndex = 11;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 99);
            label1.Name = "label1";
            label1.Size = new Size(242, 15);
            label1.TabIndex = 12;
            label1.Text = "ARCHIVO        NRO   FECHA HORA   ESTATUS";
            // 
            // panel1
            // 
            panel1.Controls.Add(btnEnviar);
            panel1.Location = new Point(12, 27);
            panel1.Name = "panel1";
            panel1.Size = new Size(469, 64);
            panel1.TabIndex = 13;
            // 
            // btnEnviar
            // 
            btnEnviar.Location = new Point(14, 13);
            btnEnviar.Name = "btnEnviar";
            btnEnviar.Size = new Size(61, 36);
            btnEnviar.TabIndex = 5;
            btnEnviar.Text = "Enviar";
            btnEnviar.UseVisualStyleBackColor = true;
            btnEnviar.Click += btnEnviar_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(493, 499);
            Controls.Add(panel1);
            Controls.Add(label1);
            Controls.Add(lstCopias);
            Controls.Add(sttBarraEstado);
            Controls.Add(menuStrip1);
            Controls.Add(lstRegistro);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            ShowInTaskbar = false;
            Text = "Sistema de Backups";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            Resize += Form1_Resize;
            contextMenuStrip1.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ListBox lstRegistro;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem resturarToolStripMenuItem;
        private ToolStripMenuItem salirToolStripMenuItem;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem copiaSeguridadToolStripMenuItem;
        private ToolStripMenuItem generarAhoraToolStripMenuItem;
        private ToolStripMenuItem comprobarCambiosToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem salirToolStripMenuItem1;
        private ToolStripMenuItem configuracionToolStripMenuItem;
        private ToolStripMenuItem configuracionSistemaToolStripMenuItem;
        private ToolStripMenuItem configuracionDeCopiasToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem ayudaToolStripMenuItem;
        private ToolStripMenuItem acercaDeToolStripMenuItem;
        private StatusStrip sttBarraEstado;
        private ListBox lstCopias;
        private Label label1;
        private Panel panel1;
        private Button btnEnviar;
    }
}
