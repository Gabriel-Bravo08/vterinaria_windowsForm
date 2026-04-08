using CapaEntidad;
using CapaLogicaNegocio;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CapaPresentacion.Modals
{
    public partial class Frm_Mascotas_Modal : BaseModal
    {
        private TextBox txtNombre, txtColor;
        private ComboBox cboCliente, cboEspecie, cboEstado;
        private NumericUpDown numPeso;
        private DateTimePicker dtpFechaNac;
        private Label lblNom, lblCli, lblEsp, lblPes, lblFec, lblCol, lblEst;

        public Cls_Mascotas ObjetoResultado { get; set; }
        private int _mascotaId = 0;
        private readonly CN_Clientes _negocioCli = new CN_Clientes();
        private readonly CN_Especies _negocioEsp = new CN_Especies();

        public Frm_Mascotas_Modal(Cls_Mascotas obj = null)
        {
            InitializeComponentCustom();
            CargarCombos();

            if (obj != null)
            {
                lblModalTitle.Text = "EDITAR MASCOTA";
                _mascotaId = obj.MascotaId;
                txtNombre.Text = obj.NombreMascota;
                txtColor.Text = obj.Color;
                numPeso.Value = obj.Peso ?? 0;
                dtpFechaNac.Value = obj.FechaNacimiento ?? DateTime.Now;
                SetComboValue(cboCliente, obj.ClienteId);
                SetComboValue(cboEspecie, obj.EspecieId);
                SetComboValue(cboEstado, obj.EstadoId);
            }
            else
            {
                lblModalTitle.Text = "NUEVA MASCOTA";
                _mascotaId = 0;
            }
        }

        private void InitializeComponentCustom()
        {
            this.Size = new Size(500, 550);

            int startY = 20, col1X = 25, col2X = 250, colW = 200;

            lblNom = CreateLabel("Nombre:", col1X, startY);
            txtNombre = CreateTextBox(col1X, startY + 20, colW);

            lblCli = CreateLabel("Cliente (Dueño):", col2X, startY);
            cboCliente = CreateComboBox(col2X, startY + 20, colW);

            lblEsp = CreateLabel("Especie:", col1X, startY + 70);
            cboEspecie = CreateComboBox(col1X, startY + 90, colW);

            lblCol = CreateLabel("Color:", col2X, startY + 70);
            txtColor = CreateTextBox(col2X, startY + 90, colW);

            lblPes = CreateLabel("Peso (Kg):", col1X, startY + 140);
            numPeso = new NumericUpDown { Location = new Point(col1X, startY + 160), Width = colW, DecimalPlaces = 2, Font = new Font("Segoe UI", 10) };

            lblFec = CreateLabel("Fecha Nacimiento:", col2X, startY + 140);
            dtpFechaNac = new DateTimePicker { Location = new Point(col2X, startY + 160), Width = colW, Format = DateTimePickerFormat.Short, Font = new Font("Segoe UI", 10) };

            lblEst = CreateLabel("Estado:", col1X, startY + 210);
            cboEstado = CreateComboBox(col1X, startY + 230, colW);

            cboEstado.Items.Add(new { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new { Valor = 2, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            pnlContent.Controls.AddRange(new Control[] { lblNom, txtNombre, lblCli, cboCliente, lblEsp, cboEspecie, lblCol, txtColor, lblPes, numPeso, lblFec, dtpFechaNac, lblEst, cboEstado });

            btnAccept.Click += BtnAccept_Click;
        }

        private Label CreateLabel(string text, int x, int y) => new Label { Text = text, Location = new Point(x, y), AutoSize = true, Font = new Font("Segoe UI", 9) };
        private TextBox CreateTextBox(int x, int y, int w) => new TextBox { Location = new Point(x, y), Width = w, Font = new Font("Segoe UI", 10) };
        private ComboBox CreateComboBox(int x, int y, int w) => new ComboBox { Location = new Point(x, y), Width = w, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };

        private void CargarCombos()
        {
            cboCliente.DataSource = _negocioCli.Listar();
            cboCliente.DisplayMember = "NombreCompleto";
            cboCliente.ValueMember = "ClienteId";

            cboEspecie.DataSource = _negocioEsp.Listar();
            cboEspecie.DisplayMember = "NombreEspecie";
            cboEspecie.ValueMember = "EspecieId";
        }

        private void SetComboValue(ComboBox cbo, int id)
        {
            if (cbo.DataSource != null) cbo.SelectedValue = id;
            else
            {
                for (int i = 0; i < cbo.Items.Count; i++)
                    if (Convert.ToInt32(((dynamic)cbo.Items[i]).Valor) == id) { cbo.SelectedIndex = i; break; }
            }
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || cboCliente.SelectedIndex == -1)
            {
                MessageBox.Show("Nombre y Cliente son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ObjetoResultado = new Cls_Mascotas
            {
                MascotaId = _mascotaId,
                NombreMascota = txtNombre.Text.Trim(),
                ClienteId = Convert.ToInt32(cboCliente.SelectedValue),
                EspecieId = Convert.ToInt32(cboEspecie.SelectedValue),
                Color = txtColor.Text.Trim(),
                Peso = numPeso.Value,
                FechaNacimiento = dtpFechaNac.Value,
                EstadoId = Convert.ToInt32(((dynamic)cboEstado.SelectedItem).Valor),
                NombreCliente = cboCliente.Text,
                NombreEspecie = cboEspecie.Text,
                NombreEstado = cboEstado.Text
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
