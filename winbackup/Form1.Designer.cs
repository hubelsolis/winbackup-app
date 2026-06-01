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
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            lstCopias = new ListBox();
            label1 = new Label();
            panel1 = new Panel();
            panelCopias = new Panel();
            panelRegistro = new Panel();
            lblRegistroTitulo = new Label();
            btnNuevoBackup = new Button();
            btnEjecutarTodos = new Button();
            btnActualizarLista = new Button();
            btnHistorialBackups = new Button();
            btnLimpiarLista = new Button();
            btnEnviar = new Button();
            contextMenuStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            panel1.SuspendLayout();
            panelCopias.SuspendLayout();
            panelRegistro.SuspendLayout();
            SuspendLayout();
            // 
            // lstRegistro
            // 
            lstRegistro.BackColor = Color.White;
            lstRegistro.BorderStyle = BorderStyle.None;
            lstRegistro.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lstRegistro.ForeColor = Color.FromArgb(34, 34, 34);
            lstRegistro.FormattingEnabled = true;
            lstRegistro.ItemHeight = 15;
            lstRegistro.Location = new Point(12, 38);
            lstRegistro.Name = "lstRegistro";
            lstRegistro.Size = new Size(712, 196);
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
            menuStrip1.BackColor = Color.FromArgb(250, 250, 250);
            menuStrip1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            menuStrip1.Items.AddRange(new ToolStripItem[] { copiaSeguridadToolStripMenuItem, configuracionToolStripMenuItem, toolStripMenuItem2 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(760, 24);
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
            configuracionSistemaToolStripMenuItem.Click += configuracionSistemaToolStripMenuItem_Click;
            // 
            // configuracionDeCopiasToolStripMenuItem
            // 
            configuracionDeCopiasToolStripMenuItem.Name = "configuracionDeCopiasToolStripMenuItem";
            configuracionDeCopiasToolStripMenuItem.Size = new Size(205, 22);
            configuracionDeCopiasToolStripMenuItem.Text = "Configuracion de Copias";
            configuracionDeCopiasToolStripMenuItem.Click += configuracionDeCopiasToolStripMenuItem_Click;
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
            sttBarraEstado.BackColor = Color.FromArgb(250, 250, 250);
            sttBarraEstado.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            sttBarraEstado.Location = new Point(0, 695);
            sttBarraEstado.Name = "sttBarraEstado";
            sttBarraEstado.Size = new Size(760, 22);
            sttBarraEstado.TabIndex = 10;
            sttBarraEstado.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(31, 17);
            toolStripStatusLabel1.Text = "Listo";
            // 
            // lstCopias
            // 
            lstCopias.BackColor = Color.White;
            lstCopias.BorderStyle = BorderStyle.None;
            lstCopias.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lstCopias.ForeColor = Color.FromArgb(34, 34, 34);
            lstCopias.FormattingEnabled = true;
            lstCopias.ItemHeight = 15;
            lstCopias.Items.AddRange(new object[] { "DINAMICOS", "STORE.FDB         COPIA1        25/05/2026 12:50PM     1 DE 5 ENVIADOS    EN CURSO", "STORE.FDB         COPIA2        25/05/2026 02:50PM     5 DE 5 ENVIADOS    OK", "STORE.FDB         COPIA3        25/05/2026 01:50PM     5 DE 5 ENVIADOS    OK", "STORE.FDB         COPIA4        25/05/2026 07:00PM     5 DE 5 ENVIADOS    OK", "STORE.FDB         COPIA5        25/05/2026 05:00PM     5 DE 5 ENVIADOS    OK", "STORE.FDB         COPIA6        25/05/2026 04:50PM     5 DE 5 ENVIADOS    OK", "", "ESTATICOS", "FACTURA.FR3    COPIA1        25/05/2026  12:12AM    COPIA UNICA", "BOLETA.FR3       COPIA1        25/05/2026  12:12AM    COPIA UNICA", "", "" });
            lstCopias.Location = new Point(12, 40);
            lstCopias.Name = "lstCopias";
            lstCopias.Size = new Size(712, 224);
            lstCopias.TabIndex = 11;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.FromArgb(45, 50, 58);
            label1.Location = new Point(12, 14);
            label1.Name = "label1";
            label1.Size = new Size(102, 19);
            label1.TabIndex = 12;
            label1.Text = "Copias Activas";
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(btnNuevoBackup);
            panel1.Controls.Add(btnEjecutarTodos);
            panel1.Controls.Add(btnActualizarLista);
            panel1.Controls.Add(btnHistorialBackups);
            panel1.Controls.Add(btnLimpiarLista);
            panel1.Controls.Add(btnEnviar);
            panel1.Location = new Point(12, 27);
            panel1.Name = "panel1";
            panel1.Size = new Size(736, 70);
            panel1.TabIndex = 13;
            // 
            // panelCopias
            // 
            panelCopias.BackColor = Color.White;
            panelCopias.BorderStyle = BorderStyle.FixedSingle;
            panelCopias.Controls.Add(label1);
            panelCopias.Controls.Add(lstCopias);
            panelCopias.Location = new Point(12, 104);
            panelCopias.Name = "panelCopias";
            panelCopias.Size = new Size(736, 286);
            panelCopias.TabIndex = 14;
            // 
            // panelRegistro
            // 
            panelRegistro.BackColor = Color.White;
            panelRegistro.BorderStyle = BorderStyle.FixedSingle;
            panelRegistro.Controls.Add(lblRegistroTitulo);
            panelRegistro.Controls.Add(lstRegistro);
            panelRegistro.Location = new Point(12, 402);
            panelRegistro.Name = "panelRegistro";
            panelRegistro.Size = new Size(736, 260);
            panelRegistro.TabIndex = 15;
            // 
            // lblRegistroTitulo
            // 
            lblRegistroTitulo.AutoSize = true;
            lblRegistroTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblRegistroTitulo.ForeColor = Color.FromArgb(45, 50, 58);
            lblRegistroTitulo.Location = new Point(12, 14);
            lblRegistroTitulo.Name = "lblRegistroTitulo";
            lblRegistroTitulo.Size = new Size(142, 19);
            lblRegistroTitulo.TabIndex = 5;
            lblRegistroTitulo.Text = "Registro del Sistema";
            // 
            // btnNuevoBackup
            // 
            btnNuevoBackup.BackColor = Color.White;
            btnNuevoBackup.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
            btnNuevoBackup.FlatAppearance.BorderSize = 1;
            btnNuevoBackup.FlatStyle = FlatStyle.Flat;
            btnNuevoBackup.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnNuevoBackup.ForeColor = Color.FromArgb(45, 50, 58);
            btnNuevoBackup.Location = new Point(12, 16);
            btnNuevoBackup.Name = "btnNuevoBackup";
            btnNuevoBackup.Size = new Size(110, 38);
            btnNuevoBackup.TabIndex = 6;
            btnNuevoBackup.Text = "Nuevo Backup";
            btnNuevoBackup.UseVisualStyleBackColor = false;
            // 
            // btnEjecutarTodos
            // 
            btnEjecutarTodos.BackColor = Color.White;
            btnEjecutarTodos.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
            btnEjecutarTodos.FlatAppearance.BorderSize = 1;
            btnEjecutarTodos.FlatStyle = FlatStyle.Flat;
            btnEjecutarTodos.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnEjecutarTodos.ForeColor = Color.FromArgb(45, 50, 58);
            btnEjecutarTodos.Location = new Point(132, 16);
            btnEjecutarTodos.Name = "btnEjecutarTodos";
            btnEjecutarTodos.Size = new Size(110, 38);
            btnEjecutarTodos.TabIndex = 7;
            btnEjecutarTodos.Text = "Ejecutar Todos";
            btnEjecutarTodos.UseVisualStyleBackColor = false;
            // 
            // btnActualizarLista
            // 
            btnActualizarLista.BackColor = Color.White;
            btnActualizarLista.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
            btnActualizarLista.FlatAppearance.BorderSize = 1;
            btnActualizarLista.FlatStyle = FlatStyle.Flat;
            btnActualizarLista.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnActualizarLista.ForeColor = Color.FromArgb(45, 50, 58);
            btnActualizarLista.Location = new Point(252, 16);
            btnActualizarLista.Name = "btnActualizarLista";
            btnActualizarLista.Size = new Size(110, 38);
            btnActualizarLista.TabIndex = 8;
            btnActualizarLista.Text = "Actualizar Lista";
            btnActualizarLista.UseVisualStyleBackColor = false;
            // 
            // btnHistorialBackups
            // 
            btnHistorialBackups.BackColor = Color.White;
            btnHistorialBackups.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
            btnHistorialBackups.FlatAppearance.BorderSize = 1;
            btnHistorialBackups.FlatStyle = FlatStyle.Flat;
            btnHistorialBackups.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnHistorialBackups.ForeColor = Color.FromArgb(45, 50, 58);
            btnHistorialBackups.Location = new Point(372, 16);
            btnHistorialBackups.Name = "btnHistorialBackups";
            btnHistorialBackups.Size = new Size(110, 38);
            btnHistorialBackups.TabIndex = 9;
            btnHistorialBackups.Text = "Historial";
            btnHistorialBackups.UseVisualStyleBackColor = false;
            // 
            // btnLimpiarLista
            // 
            btnLimpiarLista.BackColor = Color.White;
            btnLimpiarLista.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
            btnLimpiarLista.FlatAppearance.BorderSize = 1;
            btnLimpiarLista.FlatStyle = FlatStyle.Flat;
            btnLimpiarLista.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnLimpiarLista.ForeColor = Color.FromArgb(45, 50, 58);
            btnLimpiarLista.Location = new Point(492, 16);
            btnLimpiarLista.Name = "btnLimpiarLista";
            btnLimpiarLista.Size = new Size(110, 38);
            btnLimpiarLista.TabIndex = 10;
            btnLimpiarLista.Text = "Limpiar Lista";
            btnLimpiarLista.UseVisualStyleBackColor = false;
            // 
            // btnEnviar
            // 
            btnEnviar.BackColor = Color.FromArgb(0, 120, 215);
            btnEnviar.FlatAppearance.BorderSize = 0;
            btnEnviar.FlatStyle = FlatStyle.Flat;
            btnEnviar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnEnviar.ForeColor = Color.White;
            btnEnviar.Location = new Point(612, 14);
            btnEnviar.Name = "btnEnviar";
            btnEnviar.Size = new Size(110, 42);
            btnEnviar.TabIndex = 5;
            btnEnviar.Text = "Enviar";
            btnEnviar.UseVisualStyleBackColor = false;
            btnEnviar.Click += btnEnviar_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(760, 717);
            Controls.Add(panelRegistro);
            Controls.Add(panelCopias);
            Controls.Add(panel1);
            Controls.Add(sttBarraEstado);
            Controls.Add(menuStrip1);
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
            panelCopias.ResumeLayout(false);
            panelCopias.PerformLayout();
            panelRegistro.ResumeLayout(false);
            panelRegistro.PerformLayout();
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
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ListBox lstCopias;
        private Label label1;
        private Panel panel1;
        private Panel panelCopias;
        private Panel panelRegistro;
        private Label lblRegistroTitulo;
        private Button btnNuevoBackup;
        private Button btnEjecutarTodos;
        private Button btnActualizarLista;
        private Button btnHistorialBackups;
        private Button btnLimpiarLista;
        private Button btnEnviar;
    }
}
