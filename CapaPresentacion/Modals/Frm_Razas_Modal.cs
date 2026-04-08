using CapaEntidad;
using CapaLogicaNegocio;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CapaPresentacion.Modals
{
    public partial class Frm_Razas_Modal : BaseModal
    {
        private TextBox txtNombre;
        private ComboBox cboEspecie, cboEstado;
        private Label lblNom, lblEsp, lblEst;

        public Cls_Razas ObjetoResultado { get; set; }
        private int _razaId = 0;
        private readonly CN_Especies _negocioEsp = new CN_Especies();

        public Frm_Razas_Modal(Cls_Razas obj = null)
        {
            InitializeComponentCustom();
            CargarComboEspecies();

            if (obj != null)
            {
                lblModalTitle.Text = "EDITAR RAZA";
                _razaId = obj.RazaId;
                txtNombre.Text = obj.NombreRaza;
                SetComboValue(cboEspecie, obj.EspecieId);
                SetComboValue(cboEstado, obj.EstadoId);
            }
            else
            {
                lblModalTitle.Text = "NUEVA RAZA";
                _razaId = 0;
            }
        }

        private void InitializeComponentCustom()
        {
            this.Size = new Size(400, 420);

            int startX = 25;
            int startY = 20;
            int fullW = 330;

            lblEsp = new Label { Text = "Especie:", Location = new Point(startX, startY), AutoSize = true, Font = new Font("Segoe UI", 9) };
            cboEspecie = new ComboBox { Location = new Point(startX, startY + 20), Width = fullW, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };

            lblNom = new Label { Text = "Nombre Raza:", Location = new Point(startX, startY + 70), AutoSize = true, Font = new Font("Segoe UI", 9) };
            txtNombre = new TextBox { Location = new Point(startX, startY + 90), Width = fullW, Font = new Font("Segoe UI", 10) };

            lblEst = new Label { Text = "Estado:", Location = new Point(startX, startY + 140), AutoSize = true, Font = new Font("Segoe UI", 9) };
            cboEstado = new ComboBox { Location = new Point(startX, startY + 160), Width = fullW, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };

            cboEstado.Items.Add(new { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new { Valor = 2, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            pnlContent.Controls.AddRange(new Control[] { lblEsp, cboEspecie, lblNom, txtNombre, lblEst, cboEstado });

            btnAccept.Click += BtnAccept_Click;
        }

        private void CargarComboEspecies()
        {
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
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || cboEspecie.SelectedIndex == -1)
            {
                MessageBox.Show("Especie y Raza son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ObjetoResultado = new Cls_Razas
            {
                RazaId = _razaId,
                EspecieId = Convert.ToInt32(cboEspecie.SelectedValue),
                NombreRaza = txtNombre.Text.Trim(),
                EstadoId = Convert.ToInt32(((dynamic)cboEstado.SelectedItem).Valor),
                NombreEspecie = cboEspecie.Text,
                NombreEstado = cboEstado.Text
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
