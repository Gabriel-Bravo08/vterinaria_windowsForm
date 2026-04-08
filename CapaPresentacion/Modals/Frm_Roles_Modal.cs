using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion.Modals
{
    public partial class Frm_Roles_Modal : BaseModal
    {
        private TextBox txtNombre;
        private ComboBox cboEstado;
        private Label lblNombre;
        private Label lblEstado;
        public Cls_Roles ObjetoResultado { get; set; }
        private int _rolId = 0;

        public Frm_Roles_Modal(Cls_Roles obj = null)
        {
            InitializeComponentCustom();
            if (obj != null)
            {
                lblModalTitle.Text = "EDITAR ROL";
                _rolId = obj.RolId;
                txtNombre.Text = obj.NombreRol;
                SetComboValue(obj.EstadoId);
            }
            else
            {
                lblModalTitle.Text = "NUEVO ROL";
                _rolId = 0;
            }
        }

        private void InitializeComponentCustom()
        {
            this.Size = new Size(400, 350);

            lblNombre = new Label { Text = "Nombre del Rol:", Location = new Point(25, 25), AutoSize = true, Font = new Font("Segoe UI", 9) };
            txtNombre = new TextBox { Location = new Point(25, 45), Width = 330, Font = new Font("Segoe UI", 10) };

            lblEstado = new Label { Text = "Estado:", Location = new Point(25, 90), AutoSize = true, Font = new Font("Segoe UI", 9) };
            cboEstado = new ComboBox { Location = new Point(25, 110), Width = 330, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };

            cboEstado.Items.Add(new { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new { Valor = 2, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            pnlContent.Controls.Add(lblNombre);
            pnlContent.Controls.Add(txtNombre);
            pnlContent.Controls.Add(lblEstado);
            pnlContent.Controls.Add(cboEstado);

            btnAccept.Click += BtnAccept_Click;
        }

        private void SetComboValue(int id)
        {
            for (int i = 0; i < cboEstado.Items.Count; i++)
            {
                if (Convert.ToInt32(((dynamic)cboEstado.Items[i]).Valor) == id)
                {
                    cboEstado.SelectedIndex = i;
                    break;
                }
            }
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Debe ingresar un nombre.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ObjetoResultado = new Cls_Roles
            {
                RolId = _rolId,
                NombreRol = txtNombre.Text.Trim(),
                EstadoId = Convert.ToInt32(((dynamic)cboEstado.SelectedItem).Valor),
                NombreEstado = ((dynamic)cboEstado.SelectedItem).Texto
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
