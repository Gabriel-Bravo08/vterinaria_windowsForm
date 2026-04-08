using CapaEntidad;
using CapaLogicaNegocio;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CapaPresentacion.Modals
{
    public partial class Frm_Citas_Modal : BaseModal
    {
        private ComboBox cboMascota, cboVeterinario, cboEstado;
        private DateTimePicker dtpFecha;
        private TextBox txtNotas;
        private Label lblMas, lblVet, lblFec, lblNot, lblEst;

        public Cls_Citas ObjetoResultado { get; set; }
        private int _citaId = 0;
        private readonly CN_Mascotas _negocioMas = new CN_Mascotas();
        private readonly CN_Personal _negocioPer = new CN_Personal();

        public Frm_Citas_Modal(Cls_Citas obj = null)
        {
            InitializeComponentCustom();
            CargarCombos();

            if (obj != null)
            {
                lblModalTitle.Text = "EDITAR CITA";
                _citaId = obj.CitaId;
                SetComboValue(cboMascota, obj.MascotaId);
                SetComboValue(cboVeterinario, obj.VeterinarioId);
                dtpFecha.Value = obj.FechaCita;
                txtNotas.Text = obj.Notas;
                SetComboValueStatus(cboEstado, obj.EstadoId);
            }
            else
            {
                lblModalTitle.Text = "AGENDAR CITA";
                _citaId = 0;
                dtpFecha.Value = DateTime.Now;
            }
        }

        private void InitializeComponentCustom()
        {
            this.Size = new Size(450, 520);

            int startX = 25;
            int startY = 20;
            int fullW = 380;

            lblMas = new Label { Text = "Mascota:", Location = new Point(startX, startY), AutoSize = true, Font = new Font("Segoe UI", 9) };
            cboMascota = new ComboBox { Location = new Point(startX, startY + 20), Width = fullW, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };

            lblVet = new Label { Text = "Veterinario:", Location = new Point(startX, startY + 70), AutoSize = true, Font = new Font("Segoe UI", 9) };
            cboVeterinario = new ComboBox { Location = new Point(startX, startY + 90), Width = fullW, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };

            lblFec = new Label { Text = "Fecha y Hora:", Location = new Point(startX, startY + 140), AutoSize = true, Font = new Font("Segoe UI", 9) };
            dtpFecha = new DateTimePicker { Location = new Point(startX, startY + 160), Width = fullW, CustomFormat = "dd/MM/yyyy HH:mm", Format = DateTimePickerFormat.Custom, Font = new Font("Segoe UI", 10) };

            lblNot = new Label { Text = "Notas:", Location = new Point(startX, startY + 210), AutoSize = true, Font = new Font("Segoe UI", 9) };
            txtNotas = new TextBox { Location = new Point(startX, startY + 230), Width = fullW, Height = 80, Multiline = true, Font = new Font("Segoe UI", 10) };

            lblEst = new Label { Text = "Estado:", Location = new Point(startX, startY + 320), AutoSize = true, Font = new Font("Segoe UI", 9) };
            cboEstado = new ComboBox { Location = new Point(startX, startY + 340), Width = fullW, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };

            cboEstado.Items.Add(new { Valor = 1, Texto = "Programada" });
            cboEstado.Items.Add(new { Valor = 2, Texto = "Completada" });
            cboEstado.Items.Add(new { Valor = 3, Texto = "Cancelada" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            pnlContent.Controls.AddRange(new Control[] { lblMas, cboMascota, lblVet, cboVeterinario, lblFec, dtpFecha, lblNot, txtNotas, lblEst, cboEstado });

            btnAccept.Click += BtnAccept_Click;
        }

        private void CargarCombos()
        {
            cboMascota.DataSource = _negocioMas.Listar();
            cboMascota.DisplayMember = "NombreMascota";
            cboMascota.ValueMember = "MascotaId";

            cboVeterinario.DataSource = _negocioPer.Listar().FindAll(x => x.NombreRol == "Veterinario");
            cboVeterinario.DisplayMember = "NombreCompleto";
            cboVeterinario.ValueMember = "PersonalId";
        }

        private void SetComboValue(ComboBox cbo, int id)
        {
            if (cbo.DataSource != null) cbo.SelectedValue = id;
        }

        private void SetComboValueStatus(ComboBox cbo, int id)
        {
            for (int i = 0; i < cbo.Items.Count; i++)
                if (Convert.ToInt32(((dynamic)cbo.Items[i]).Valor) == id) { cbo.SelectedIndex = i; break; }
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            if (cboMascota.SelectedIndex == -1 || cboVeterinario.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtNotas.Text))
            {
                MessageBox.Show("Mascota, Veterinario y Notas son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ObjetoResultado = new Cls_Citas
            {
                CitaId = _citaId,
                MascotaId = Convert.ToInt32(cboMascota.SelectedValue),
                VeterinarioId = Convert.ToInt32(cboVeterinario.SelectedValue),
                FechaCita = dtpFecha.Value,
                Notas = txtNotas.Text.Trim(),
                EstadoId = Convert.ToInt32(((dynamic)cboEstado.SelectedItem).Valor),
                NombreMascota = cboMascota.Text,
                NombreVeterinario = cboVeterinario.Text,
                NombreEstado = cboEstado.Text
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
