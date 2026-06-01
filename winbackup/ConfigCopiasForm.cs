using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace winbackup
{
    public class ConfigCopiasForm : Form
    {
        private Label lblTitulo;
        private DataGridView dgvCopias;
        private Button btnAgregar;
        private Button btnEditar;
        private Button btnEliminar;
        private GroupBox grpOpciones;
        private Label lblCantidadCopias;
        private NumericUpDown nudCantidadCopias;
        private Label lblFrecuencia;
        private ComboBox cmbFrecuencia;
        private Label lblHoraProgramada;
        private DateTimePicker dtpHoraProgramada;
        private Button btnGuardar;
        private Button btnCancelar;
        private readonly List<BackupItem> _copias = new List<BackupItem>();

        public ConfigCopiasForm()
        {
            InitializeComponent();
            Load += ConfigCopiasForm_Load;
            btnAgregar.Click += BtnAgregar_Click;
            btnEditar.Click += BtnEditar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnGuardar.Click += BtnGuardar_Click;
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            dgvCopias = new DataGridView();
            btnAgregar = new Button();
            btnEditar = new Button();
            btnEliminar = new Button();
            grpOpciones = new GroupBox();
            lblCantidadCopias = new Label();
            nudCantidadCopias = new NumericUpDown();
            lblFrecuencia = new Label();
            cmbFrecuencia = new ComboBox();
            lblHoraProgramada = new Label();
            dtpHoraProgramada = new DateTimePicker();
            btnGuardar = new Button();
            btnCancelar = new Button();

            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitulo.Location = new Point(24, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(224, 25);
            lblTitulo.Text = "Configuración de Copias";
            // 
            // dgvCopias
            // 
            dgvCopias.AllowUserToAddRows = false;
            dgvCopias.AllowUserToDeleteRows = false;
            dgvCopias.AllowUserToResizeRows = false;
            dgvCopias.BackgroundColor = Color.White;
            dgvCopias.BorderStyle = BorderStyle.FixedSingle;
            dgvCopias.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCopias.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = "colNombre", HeaderText = "Nombre", Width = 260 },
                new DataGridViewTextBoxColumn { Name = "colTipo", HeaderText = "Tipo", Width = 120 },
                new DataGridViewTextBoxColumn { Name = "colOrigen", HeaderText = "Origen", Width = 240 },
                new DataGridViewTextBoxColumn { Name = "colDestino", HeaderText = "Destino", Width = 240 }
            });
            dgvCopias.Location = new Point(24, 64);
            dgvCopias.MultiSelect = false;
            dgvCopias.Name = "dgvCopias";
            dgvCopias.ReadOnly = true;
            dgvCopias.RowHeadersVisible = false;
            dgvCopias.RowTemplate.Height = 28;
            dgvCopias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCopias.Size = new Size(760, 220);
            // 
            // btnAgregar
            // 
            btnAgregar.BackColor = Color.White;
            btnAgregar.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
            btnAgregar.FlatAppearance.BorderSize = 1;
            btnAgregar.FlatStyle = FlatStyle.Flat;
            btnAgregar.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnAgregar.ForeColor = Color.FromArgb(45, 50, 58);
            btnAgregar.Location = new Point(24, 300);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(110, 36);
            btnAgregar.Text = "Agregar";
            btnAgregar.UseVisualStyleBackColor = false;
            // 
            // btnEditar
            // 
            btnEditar.BackColor = Color.White;
            btnEditar.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
            btnEditar.FlatAppearance.BorderSize = 1;
            btnEditar.FlatStyle = FlatStyle.Flat;
            btnEditar.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnEditar.ForeColor = Color.FromArgb(45, 50, 58);
            btnEditar.Location = new Point(144, 300);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(110, 36);
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = false;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.White;
            btnEliminar.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
            btnEliminar.FlatAppearance.BorderSize = 1;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnEliminar.ForeColor = Color.FromArgb(45, 50, 58);
            btnEliminar.Location = new Point(264, 300);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(110, 36);
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = false;
            // 
            // grpOpciones
            // 
            grpOpciones.Controls.Add(lblCantidadCopias);
            grpOpciones.Controls.Add(nudCantidadCopias);
            grpOpciones.Controls.Add(lblFrecuencia);
            grpOpciones.Controls.Add(cmbFrecuencia);
            grpOpciones.Controls.Add(lblHoraProgramada);
            grpOpciones.Controls.Add(dtpHoraProgramada);
            grpOpciones.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            grpOpciones.Location = new Point(24, 352);
            grpOpciones.Name = "grpOpciones";
            grpOpciones.Size = new Size(520, 120);
            grpOpciones.Text = "Opciones de copia";
            // 
            // lblCantidadCopias
            // 
            lblCantidadCopias.AutoSize = true;
            lblCantidadCopias.Location = new Point(20, 32);
            lblCantidadCopias.Name = "lblCantidadCopias";
            lblCantidadCopias.Size = new Size(104, 15);
            lblCantidadCopias.Text = "Cantidad de copias";
            // 
            // nudCantidadCopias
            // 
            nudCantidadCopias.Location = new Point(20, 54);
            nudCantidadCopias.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            nudCantidadCopias.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudCantidadCopias.Name = "nudCantidadCopias";
            nudCantidadCopias.Size = new Size(120, 23);
            nudCantidadCopias.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // lblFrecuencia
            // 
            lblFrecuencia.AutoSize = true;
            lblFrecuencia.Location = new Point(168, 32);
            lblFrecuencia.Name = "lblFrecuencia";
            lblFrecuencia.Size = new Size(65, 15);
            lblFrecuencia.Text = "Frecuencia";
            // 
            // cmbFrecuencia
            // 
            cmbFrecuencia.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFrecuencia.Items.AddRange(new object[] { "Diaria", "Semanal", "Mensual" });
            cmbFrecuencia.Location = new Point(168, 54);
            cmbFrecuencia.Name = "cmbFrecuencia";
            cmbFrecuencia.Size = new Size(160, 23);
            // 
            // lblHoraProgramada
            // 
            lblHoraProgramada.AutoSize = true;
            lblHoraProgramada.Location = new Point(340, 32);
            lblHoraProgramada.Name = "lblHoraProgramada";
            lblHoraProgramada.Size = new Size(101, 15);
            lblHoraProgramada.Text = "Hora programada";
            // 
            // dtpHoraProgramada
            // 
            dtpHoraProgramada.Format = DateTimePickerFormat.Time;
            dtpHoraProgramada.ShowUpDown = true;
            dtpHoraProgramada.Location = new Point(340, 54);
            dtpHoraProgramada.Name = "dtpHoraProgramada";
            dtpHoraProgramada.Size = new Size(150, 23);
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(0, 120, 215);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(664, 450);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(120, 34);
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(240, 240, 240);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnCancelar.FlatAppearance.BorderSize = 1;
            btnCancelar.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnCancelar.ForeColor = Color.FromArgb(45, 50, 58);
            btnCancelar.Location = new Point(528, 450);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 34);
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += (sender, e) => Close();
            // 
            // ConfigCopiasForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(812, 500);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(grpOpciones);
            Controls.Add(btnEliminar);
            Controls.Add(btnEditar);
            Controls.Add(btnAgregar);
            Controls.Add(dgvCopias);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConfigCopiasForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Configuración de Copias";
            ((System.ComponentModel.ISupportInitialize)nudCantidadCopias).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void ConfigCopiasForm_Load(object? sender, EventArgs e)
        {
            CargarConfiguracion();
        }

        private void CargarConfiguracion()
        {
            try
            {
                var config = GlobalData.Config ?? clconfiguracion.Cargar("config.json");
                var backups = config.Backups ?? new BackupsConfig();

                _copias.Clear();
                if (backups.Items != null)
                    _copias.AddRange(backups.Items);

                nudCantidadCopias.Value = backups.CantidadValor > 0 ? backups.CantidadValor : 5;
                cmbFrecuencia.SelectedItem = string.IsNullOrWhiteSpace(backups.Frecuencia) ? "Diaria" : backups.Frecuencia;

                if (DateTime.TryParse(backups.HoraProgramada, out DateTime horaProgramada))
                {
                    dtpHoraProgramada.Value = horaProgramada;
                }
                else
                {
                    dtpHoraProgramada.Value = DateTime.Now;
                }

                MostrarCopiasEnGrid();
            }
            catch
            {
                nudCantidadCopias.Value = 5;
                cmbFrecuencia.SelectedIndex = 0;
                dtpHoraProgramada.Value = DateTime.Now;
            }
        }

        private void MostrarCopiasEnGrid()
        {
            dgvCopias.Rows.Clear();
            if (_copias.Count == 0)
            {
                dgvCopias.Rows.Add("No hay copias configuradas", string.Empty, string.Empty, string.Empty);
                return;
            }

            foreach (var copia in _copias)
            {
                dgvCopias.Rows.Add(copia.Nombre, copia.Tipo, copia.Origen, copia.Destino);
            }
        }

        private void BtnAgregar_Click(object? sender, EventArgs e)
        {
            using var dialog = new BackupItemForm();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _copias.Add(dialog.Item);
                MostrarCopiasEnGrid();
            }
        }

        private void BtnEditar_Click(object? sender, EventArgs e)
        {
            if (dgvCopias.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Seleccione una copia para editar.", "Editar copia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int index = dgvCopias.SelectedRows[0].Index;
            if (index < 0 || index >= _copias.Count)
                return;

            var copia = _copias[index];
            using var dialog = new BackupItemForm(copia);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _copias[index] = dialog.Item;
                MostrarCopiasEnGrid();
            }
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (dgvCopias.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Seleccione una copia para eliminar.", "Eliminar copia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int index = dgvCopias.SelectedRows[0].Index;
            if (index < 0 || index >= _copias.Count)
                return;

            if (MessageBox.Show(this, "¿Confirma que desea eliminar esta copia?", "Eliminar copia", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _copias.RemoveAt(index);
                MostrarCopiasEnGrid();
            }
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (cmbFrecuencia.SelectedItem == null)
            {
                MessageBox.Show(this, "Seleccione una frecuencia para las copias.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var config = GlobalData.Config ?? clconfiguracion.Cargar("config.json");
                config.Backups = new BackupsConfig
                {
                    Cantidad = nudCantidadCopias.Value.ToString(),
                    Frecuencia = cmbFrecuencia.SelectedItem.ToString(),
                    HoraProgramada = dtpHoraProgramada.Value.ToString("HH:mm:ss"),
                    Items = new List<BackupItem>(_copias)
                };

                clconfiguracion.Guardar(config, "config.json");
                GlobalData.Config = config;

                MessageBox.Show(this, "Configuración de copias guardada correctamente.", "Guardar copias", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"No se pudo guardar la configuración. {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
