using CapaEntidad;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CapaPresentacion.Modals
{
    public partial class Frm_Clientes_Modal : BaseModal
    {
        private TextBox txtNombre, txtApellido, txtTelefono, txtEmail, txtDireccion;
        private ComboBox cboEstado;
        private Label lblNom, lblApe, lblTel, lblMail, lblDir, lblEst;

        private void InitializeComponent()
        {
            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnMaximize
            // 
            this.btnMaximize.FlatAppearance.BorderSize = 0;
            // 
            // pnlFooter
            // 
            this.pnlFooter.Location = new System.Drawing.Point(2, 735);
            // 
            // btnAccept
            // 
            this.btnAccept.FlatAppearance.BorderSize = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderSize = 0;
            // 
            // pnlContent
            // 
            this.pnlContent.Size = new System.Drawing.Size(496, 678);
            // 
            // Frm_Clientes_Modal
            // 
            this.ClientSize = new System.Drawing.Size(500, 807);
            this.Name = "Frm_Clientes_Modal";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        public Cls_Clientes ObjetoResultado { get; set; }
        private int _clienteId = 0;

        public Frm_Clientes_Modal(Cls_Clientes obj = null)
        {
            InitializeComponentCustom();
            if (obj != null)
            {
                lblModalTitle.Text = "EDITAR CLIENTE";
                _clienteId = obj.ClienteId;
                txtNombre.Text = obj.Nombre;
                txtApellido.Text = obj.Apellido;
                txtTelefono.Text = obj.Telefono;
                txtEmail.Text = obj.Email;
                txtDireccion.Text = obj.Direccion;
                SetComboValue(obj.EstadoId);
            }
            else
            {
                lblModalTitle.Text = "NUEVO CLIENTE";
                _clienteId = 0;
            }
        }

        private void InitializeComponentCustom()
        {
            this.Size = new Size(450, 520);

            int startX = 25;
            int startY = 20;
            int fullW = 380;

            lblNom = CreateLabel("Nombre:", startX, startY);
            txtNombre = CreateTextBox(startX, startY + 20, fullW);

            lblApe = CreateLabel("Apellido:", startX, startY + 70);
            txtApellido = CreateTextBox(startX, startY + 90, fullW);

            lblTel = CreateLabel("Teléfono:", startX, startY + 140);
            txtTelefono = CreateTextBox(startX, startY + 160, fullW);

            lblMail = CreateLabel("Email:", startX, startY + 210);
            txtEmail = CreateTextBox(startX, startY + 230, fullW);

            lblDir = CreateLabel("Dirección:", startX, startY + 280);
            txtDireccion = CreateTextBox(startX, startY + 300, fullW);

            lblEst = CreateLabel("Estado:", startX, startY + 350);
            cboEstado = CreateComboBox(startX, startY + 370, fullW);

            cboEstado.Items.Add(new { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new { Valor = 2, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            pnlContent.Controls.AddRange(new Control[] { lblNom, txtNombre, lblApe, txtApellido, lblTel, txtTelefono, lblMail, txtEmail, lblDir, txtDireccion, lblEst, cboEstado });

            btnAccept.Click += BtnAccept_Click;
        }

        private Label CreateLabel(string text, int x, int y) => new Label { Text = text, Location = new Point(x, y), AutoSize = true, Font = new Font("Segoe UI", 9) };
        private TextBox CreateTextBox(int x, int y, int w) => new TextBox { Location = new Point(x, y), Width = w, Font = new Font("Segoe UI", 10) };
        private ComboBox CreateComboBox(int x, int y, int w) => new ComboBox { Location = new Point(x, y), Width = w, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };

        private void SetComboValue(int id)
        {
            for (int i = 0; i < cboEstado.Items.Count; i++)
                if (Convert.ToInt32(((dynamic)cboEstado.Items[i]).Valor) == id) { cboEstado.SelectedIndex = i; break; }
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("Nombre y Apellido son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ObjetoResultado = new Cls_Clientes
            {
                ClienteId = _clienteId,
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Direccion = txtDireccion.Text.Trim(),
                EstadoId = Convert.ToInt32(((dynamic)cboEstado.SelectedItem).Valor),
                NombreEstado = cboEstado.Text
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
