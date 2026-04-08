using CapaEntidad;
using CapaLogicaNegocio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CapaPresentacion.Modals
{
    public partial class Frm_Personal_Modal : BaseModal
    {
        private TextBox txtPrimerNombre, txtSegundoNombre, txtPrimerApellido, txtSegundoApellido, txtTelefono, txtEmail, txtUsuario, txtClave;
        private ComboBox cboRol, cboEstado;
        private Label lblPName, lblSName, lblPApellido, lblSApellido, lblTel, lblMail, lblUser, lblPass, lblRol, lblEst;

        public Cls_Personal ObjetoResultado { get; set; }
        private int _personalId = 0;
        private readonly CN_Roles _negocioRol = new CN_Roles();

        public Frm_Personal_Modal(Cls_Personal obj = null)
        {
            InitializeComponentCustom();
            CargarCombos();

            if (obj != null)
            {
                lblModalTitle.Text = "EDITAR PERSONAL";
                _personalId = obj.PersonalId;
                txtPrimerNombre.Text = obj.PrimerNombre;
                txtSegundoNombre.Text = obj.SegundoNombre;
                txtPrimerApellido.Text = obj.PrimerApellido;
                txtSegundoApellido.Text = obj.SegundoApellido;
                txtTelefono.Text = obj.Telefono;
                txtEmail.Text = obj.Email;
                txtUsuario.Text = obj.NombreUsuario;
                txtClave.Text = "********"; // Placeholder for security
                txtClave.Enabled = false; // Disable password edit in this simplified modal
                SetComboValue(cboRol, obj.RolId);
                SetComboValue(cboEstado, obj.EstadoId);
            }
            else
            {
                lblModalTitle.Text = "NUEVO PERSONAL";
                _personalId = 0;
            }
        }

        private void InitializeComponentCustom()
        {
            this.Size = new Size(550, 580);

            int startX = 25;
            int startY = 20;
            int colWidth = 230;
            int rowHeight = 60;

            // Row 1
            lblPName = CreateLabel("Primer Nombre:", startX, startY);
            txtPrimerNombre = CreateTextBox(startX, startY + 20, colWidth);

            lblSName = CreateLabel("Segundo Nombre:", startX + colWidth + 20, startY);
            txtSegundoNombre = CreateTextBox(startX + colWidth + 20, startY + 20, colWidth);

            // Row 2
            lblPApellido = CreateLabel("Primer Apellido:", startX, startY + rowHeight);
            txtPrimerApellido = CreateTextBox(startX, startY + rowHeight + 20, colWidth);

            lblSApellido = CreateLabel("Segundo Apellido:", startX + colWidth + 20, startY + rowHeight);
            txtSegundoApellido = CreateTextBox(startX + colWidth + 20, startY + rowHeight + 20, colWidth);

            // Row 3
            lblTel = CreateLabel("Teléfono:", startX, startY + rowHeight * 2);
            txtTelefono = CreateTextBox(startX, startY + rowHeight * 2 + 20, colWidth);

            lblMail = CreateLabel("Email:", startX + colWidth + 20, startY + rowHeight * 2);
            txtEmail = CreateTextBox(startX + colWidth + 20, startY + rowHeight * 2 + 20, colWidth);

            // Row 4
            lblUser = CreateLabel("Usuario:", startX, startY + rowHeight * 3);
            txtUsuario = CreateTextBox(startX, startY + rowHeight * 3 + 20, colWidth);

            lblPass = CreateLabel("Clave:", startX + colWidth + 20, startY + rowHeight * 3);
            txtClave = CreateTextBox(startX + colWidth + 20, startY + rowHeight * 3 + 20, colWidth);
            txtClave.PasswordChar = '*';

            // Row 5
            lblRol = CreateLabel("Rol:", startX, startY + rowHeight * 4);
            cboRol = CreateComboBox(startX, startY + rowHeight * 4 + 20, colWidth);

            lblEst = CreateLabel("Estado:", startX + colWidth + 20, startY + rowHeight * 4);
            cboEstado = CreateComboBox(startX + colWidth + 20, startY + rowHeight * 4 + 20, colWidth);

            pnlContent.Controls.AddRange(new Control[] { lblPName, txtPrimerNombre, lblSName, txtSegundoNombre, lblPApellido, txtPrimerApellido, lblSApellido, txtSegundoApellido, lblTel, txtTelefono, lblMail, txtEmail, lblUser, txtUsuario, lblPass, txtClave, lblRol, cboRol, lblEst, cboEstado });

            btnAccept.Click += BtnAccept_Click;
        }

        private Label CreateLabel(string text, int x, int y) => new Label { Text = text, Location = new Point(x, y), AutoSize = true, Font = new Font("Segoe UI", 9) };
        private TextBox CreateTextBox(int x, int y, int w) => new TextBox { Location = new Point(x, y), Width = w, Font = new Font("Segoe UI", 10) };
        private ComboBox CreateComboBox(int x, int y, int w) => new ComboBox { Location = new Point(x, y), Width = w, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };

        private void CargarCombos()
        {
            var roles = _negocioRol.Listar();
            cboRol.DataSource = roles;
            cboRol.DisplayMember = "NombreRol";
            cboRol.ValueMember = "RolId";

            cboEstado.Items.Add(new { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new { Valor = 2, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;
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
            if (string.IsNullOrWhiteSpace(txtPrimerNombre.Text) || string.IsNullOrWhiteSpace(txtPrimerApellido.Text) || string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("Primer nombre, Primer apellido y Usuario son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ObjetoResultado = new Cls_Personal
            {
                PersonalId = _personalId,
                PrimerNombre = txtPrimerNombre.Text.Trim(),
                SegundoNombre = txtSegundoNombre.Text.Trim(),
                PrimerApellido = txtPrimerApellido.Text.Trim(),
                SegundoApellido = txtSegundoApellido.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                NombreUsuario = txtUsuario.Text.Trim(),
                Clave = txtClave.Text.Trim(),
                RolId = Convert.ToInt32(cboRol.SelectedValue),
                EstadoId = Convert.ToInt32(((dynamic)cboEstado.SelectedItem).Valor),
                NombreRol = cboRol.Text,
                NombreEstado = cboEstado.Text
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
