using CapaEntidad;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CapaPresentacion.Modals
{
    public partial class Frm_Especialidades_Modal : BaseModal
    {
        private TextBox txtNombre;
        private ComboBox cboEstado;
        private Label lblNom, lblEst;

        public Cls_Especialidades ObjetoResultado { get; set; }
        private int _especialidadId = 0;

        public Frm_Especialidades_Modal(Cls_Especialidades obj = null)
        {
            InitializeComponentCustom();
            if (obj != null)
            {
                lblModalTitle.Text = "EDITAR ESPECIALIDAD";
                _especialidadId = obj.EspecialidadId;
                txtNombre.Text = obj.NombreEspecialidad;
                SetComboValue(obj.EstadoId);
            }
            else
            {
                lblModalTitle.Text = "NUEVA ESPECIALIDAD";
                _especialidadId = 0;
            }
        }

        private void InitializeComponentCustom()
        {
            this.Size = new Size(400, 320);

            int startX = 25;
            int startY = 20;
            int fullW = 330;

            lblNom = new Label { Text = "Nombre Especialidad:", Location = new Point(startX, startY), AutoSize = true, Font = new Font("Segoe UI", 9) };
            txtNombre = new TextBox { Location = new Point(startX, startY + 20), Width = fullW, Font = new Font("Segoe UI", 10) };

            lblEst = new Label { Text = "Estado:", Location = new Point(startX, startY + 70), AutoSize = true, Font = new Font("Segoe UI", 9) };
            cboEstado = new ComboBox { Location = new Point(startX, startY + 90), Width = fullW, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };

            cboEstado.Items.Add(new { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new { Valor = 2, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            pnlContent.Controls.AddRange(new Control[] { lblNom, txtNombre, lblEst, cboEstado });

            btnAccept.Click += BtnAccept_Click;
        }

        private void SetComboValue(int id)
        {
            for (int i = 0; i < cboEstado.Items.Count; i++)
                if (Convert.ToInt32(((dynamic)cboEstado.Items[i]).Valor) == id) { cboEstado.SelectedIndex = i; break; }
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ObjetoResultado = new Cls_Especialidades
            {
                EspecialidadId = _especialidadId,
                NombreEspecialidad = txtNombre.Text.Trim(),
                EstadoId = Convert.ToInt32(((dynamic)cboEstado.SelectedItem).Valor),
                NombreEstado = cboEstado.Text
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
