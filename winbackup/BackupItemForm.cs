using System;
using System.Drawing;
using System.Windows.Forms;

namespace winbackup
{
    public class BackupItemForm : Form
    {
        private Label lblNombre = null!;
        private TextBox txtNombre = null!;
        private Label lblTipo = null!;
        private TextBox txtTipo = null!;
        private Label lblOrigen = null!;
        private TextBox txtOrigen = null!;
        private Label lblDestino = null!;
        private TextBox txtDestino = null!;
        private Button btnAceptar = null!;
        private Button btnCancelar = null!;
        private readonly BackupItem? _initialItem;

        public BackupItem Item { get; private set; }

        public BackupItemForm(BackupItem? item = null)
        {
            _initialItem = item;
            InitializeComponent();
            Item = item != null
                ? new BackupItem
                {
                    Nombre = item.Nombre,
                    Tipo = item.Tipo,
                    Origen = item.Origen,
                    Destino = item.Destino
                }
                : new BackupItem();

            if (_initialItem != null)
            {
                txtNombre.Text = _initialItem.Nombre ?? string.Empty;
                txtTipo.Text = _initialItem.Tipo ?? string.Empty;
                txtOrigen.Text = _initialItem.Origen ?? string.Empty;
                txtDestino.Text = _initialItem.Destino ?? string.Empty;
            }
        }

        private void InitializeComponent()
        {
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblTipo = new Label();
            txtTipo = new TextBox();
            lblOrigen = new Label();
            txtOrigen = new TextBox();
            lblDestino = new Label();
            txtDestino = new TextBox();
            btnAceptar = new Button();
            btnCancelar = new Button();

            SuspendLayout();
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(24, 24);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(50, 15);
            lblNombre.Text = "Nombre";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(24, 44);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(400, 23);
            // 
            // lblTipo
            // 
            lblTipo.AutoSize = true;
            lblTipo.Location = new Point(24, 80);
            lblTipo.Name = "lblTipo";
            lblTipo.Size = new Size(30, 15);
            lblTipo.Text = "Tipo";
            // 
            // txtTipo
            // 
            txtTipo.Location = new Point(24, 100);
            txtTipo.Name = "txtTipo";
            txtTipo.Size = new Size(400, 23);
            // 
            // lblOrigen
            // 
            lblOrigen.AutoSize = true;
            lblOrigen.Location = new Point(24, 136);
            lblOrigen.Name = "lblOrigen";
            lblOrigen.Size = new Size(40, 15);
            lblOrigen.Text = "Origen";
            // 
            // txtOrigen
            // 
            txtOrigen.Location = new Point(24, 156);
            txtOrigen.Name = "txtOrigen";
            txtOrigen.Size = new Size(400, 23);
            // 
            // lblDestino
            // 
            lblDestino.AutoSize = true;
            lblDestino.Location = new Point(24, 192);
            lblDestino.Name = "lblDestino";
            lblDestino.Size = new Size(50, 15);
            lblDestino.Text = "Destino";
            // 
            // txtDestino
            // 
            txtDestino.Location = new Point(24, 212);
            txtDestino.Name = "txtDestino";
            txtDestino.Size = new Size(400, 23);
            // 
            // btnAceptar
            // 
            btnAceptar.BackColor = Color.FromArgb(0, 120, 215);
            btnAceptar.FlatStyle = FlatStyle.Flat;
            btnAceptar.FlatAppearance.BorderSize = 0;
            btnAceptar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnAceptar.ForeColor = Color.White;
            btnAceptar.Location = new Point(248, 252);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(88, 32);
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = false;
            btnAceptar.Click += BtnAceptar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(240, 240, 240);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnCancelar.FlatAppearance.BorderSize = 1;
            btnCancelar.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnCancelar.ForeColor = Color.FromArgb(45, 50, 58);
            btnCancelar.Location = new Point(346, 252);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(88, 32);
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += (sender, e) => Close();
            // 
            // BackupItemForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(456, 302);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(txtDestino);
            Controls.Add(lblDestino);
            Controls.Add(txtOrigen);
            Controls.Add(lblOrigen);
            Controls.Add(txtTipo);
            Controls.Add(lblTipo);
            Controls.Add(txtNombre);
            Controls.Add(lblNombre);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BackupItemForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = _initialItem == null ? "Agregar copia" : "Editar copia";
            ResumeLayout(false);
            PerformLayout();
        }

        private void BtnAceptar_Click(object? sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string tipo = txtTipo.Text.Trim();
            string origen = txtOrigen.Text.Trim();
            string destino = txtDestino.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(tipo) || string.IsNullOrWhiteSpace(origen) || string.IsNullOrWhiteSpace(destino))
            {
                MessageBox.Show(this, "Complete todos los campos antes de continuar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Item.Nombre = nombre;
            Item.Tipo = tipo;
            Item.Origen = origen;
            Item.Destino = destino;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
