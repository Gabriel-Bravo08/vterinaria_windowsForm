using CapaEntidad;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CapaPresentacion.Modals
{
    public partial class Frm_Servicios_Modal : BaseModal
    {
        private TextBox txtNombre, txtDescripcion, txtPrecio;
        private ComboBox cboEstado;
        private Label lblNom, lblDes, lblPre, lblEst;

        public Cls_Servicios ObjetoResultado { get; set; }
        private int _servicioId = 0;

        public Frm_Servicios_Modal(Cls_Servicios obj = null)
        {
            InitializeComponentCustom();
            if (obj != null)
            {
                lblModalTitle.Text = "EDITAR SERVICIO";
                _servicioId = obj.ServicioId;
                txtNombre.Text = obj.NombreServicio;
                txtDescripcion.Text = obj.Descripcion;
                txtPrecio.Text = obj.Precio.ToString("0.00");
                SetComboValue(obj.EstadoId);
            }
            else
            {
                lblModalTitle.Text = "NUEVO SERVICIO";
                _servicioId = 0;
            }
        }

        private void InitializeComponentCustom()
        {
            this.Size = new Size(400, 480);

            int startX = 25;
            int startY = 20;
            int fullW = 330;

            lblNom = new Label { Text = "Nombre Servicio:", Location = new Point(startX, startY), AutoSize = true, Font = new Font("Segoe UI", 9) };
            txtNombre = new TextBox { Location = new Point(startX, startY + 20), Width = fullW, Font = new Font("Segoe UI", 10) };

            lblDes = new Label { Text = "Descripción:", Location = new Point(startX, startY + 70), AutoSize = true, Font = new Font("Segoe UI", 9) };
            txtDescripcion = new TextBox { Location = new Point(startX, startY + 90), Width = fullW, Height = 80, Multiline = true, Font = new Font("Segoe UI", 10) };

            lblPre = new Label { Text = "Precio:", Location = new Point(startX, startY + 185), AutoSize = true, Font = new Font("Segoe UI", 9) };
            txtPrecio = new TextBox { Location = new Point(startX, startY + 205), Width = fullW, Font = new Font("Segoe UI", 10) };

            lblEst = new Label { Text = "Estado:", Location = new Point(startX, startY + 255), AutoSize = true, Font = new Font("Segoe UI", 9) };
            cboEstado = new ComboBox { Location = new Point(startX, startY + 275), Width = fullW, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };

            cboEstado.Items.Add(new { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new { Valor = 2, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            pnlContent.Controls.AddRange(new Control[] { lblNom, txtNombre, lblDes, txtDescripcion, lblPre, txtPrecio, lblEst, cboEstado });

            btnAccept.Click += BtnAccept_Click;
        }

        private void SetComboValue(int id)
        {
            for (int i = 0; i < cboEstado.Items.Count; i++)
                if (Convert.ToInt32(((dynamic)cboEstado.Items[i]).Valor) == id) { cboEstado.SelectedIndex = i; break; }
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            decimal precio;
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || !decimal.TryParse(txtPrecio.Text, out precio))
            {
                MessageBox.Show("El nombre y un precio válido son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ObjetoResultado = new Cls_Servicios
            {
                ServicioId = _servicioId,
                NombreServicio = txtNombre.Text.Trim(),
                Descripcion = txtDescripcion.Text.Trim(),
                Precio = precio,
                EstadoId = Convert.ToInt32(((dynamic)cboEstado.SelectedItem).Valor),
                NombreEstado = cboEstado.Text
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
