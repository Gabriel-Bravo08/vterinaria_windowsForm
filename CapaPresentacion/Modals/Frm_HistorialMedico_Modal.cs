using CapaEntidad;
using CapaLogicaNegocio;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CapaPresentacion.Modals
{
    public partial class Frm_HistorialMedico_Modal : BaseModal
    {
        private ComboBox cboCita, cboEstado;
        private TextBox txtDiagnostico, txtTratamiento, txtObservaciones;
        private DateTimePicker dtpProximaVisita;
        private Label lblCit, lblDia, lblTra, lblObs, lblPrx, lblEst;

        public Cls_HistorialMedico ObjetoResultado { get; set; }
        private int _historialId = 0;
        private readonly CN_Citas _negocioCitas = new CN_Citas();

        public Frm_HistorialMedico_Modal(Cls_HistorialMedico obj = null)
        {
            InitializeComponentCustom();
            CargarComboCitas();

            if (obj != null)
            {
                lblModalTitle.Text = "EDITAR HISTORIAL";
                _historialId = obj.HistorialMedicoId;
                if (cboCita.DataSource != null) cboCita.SelectedValue = obj.CitaId;
                txtDiagnostico.Text = obj.Diagnostico;
                txtTratamiento.Text = obj.Tratamiento;
                txtObservaciones.Text = obj.Observaciones;
                if (obj.ProximaVisita.HasValue)
                {
                    dtpProximaVisita.Checked = true;
                    dtpProximaVisita.Value = obj.ProximaVisita.Value;
                }
                SetComboValueStatus(obj.EstadoId);
            }
            else
            {
                lblModalTitle.Text = "NUEVO REGISTRO MÉDICO";
                _historialId = 0;
            }
        }

        private void InitializeComponentCustom()
        {
            this.Size = new Size(500, 620);

            int startX = 25;
            int startY = 20;
            int fullW = 430;

            lblCit = new Label { Text = "ID de Cita/Evento:", Location = new Point(startX, startY), AutoSize = true, Font = new Font("Segoe UI", 9) };
            cboCita = new ComboBox { Location = new Point(startX, startY + 20), Width = fullW, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };

            lblDia = new Label { Text = "Diagnóstico:", Location = new Point(startX, startY + 70), AutoSize = true, Font = new Font("Segoe UI", 9) };
            txtDiagnostico = new TextBox { Location = new Point(startX, startY + 90), Width = fullW, Height = 60, Multiline = true, Font = new Font("Segoe UI", 10) };

            lblTra = new Label { Text = "Tratamiento:", Location = new Point(startX, startY + 160), AutoSize = true, Font = new Font("Segoe UI", 9) };
            txtTratamiento = new TextBox { Location = new Point(startX, startY + 180), Width = fullW, Height = 60, Multiline = true, Font = new Font("Segoe UI", 10) };

            lblObs = new Label { Text = "Observaciones:", Location = new Point(startX, startY + 250), AutoSize = true, Font = new Font("Segoe UI", 9) };
            txtObservaciones = new TextBox { Location = new Point(startX, startY + 270), Width = fullW, Height = 60, Multiline = true, Font = new Font("Segoe UI", 10) };

            lblPrx = new Label { Text = "Próxima Visita (Opcional):", Location = new Point(startX, startY + 340), AutoSize = true, Font = new Font("Segoe UI", 9) };
            dtpProximaVisita = new DateTimePicker { Location = new Point(startX, startY + 360), Width = fullW, ShowCheckBox = true, Checked = false, Font = new Font("Segoe UI", 10) };

            lblEst = new Label { Text = "Estado:", Location = new Point(startX, startY + 410), AutoSize = true, Font = new Font("Segoe UI", 9) };
            cboEstado = new ComboBox { Location = new Point(startX, startY + 430), Width = fullW, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };

            cboEstado.Items.Add(new { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new { Valor = 2, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            pnlContent.Controls.AddRange(new Control[] { lblCit, cboCita, lblDia, txtDiagnostico, lblTra, txtTratamiento, lblObs, txtObservaciones, lblPrx, dtpProximaVisita, lblEst, cboEstado });

            btnAccept.Click += BtnAccept_Click;
        }

        private void CargarComboCitas()
        {
            var citas = _negocioCitas.Listar();
            cboCita.DataSource = citas;
            cboCita.DisplayMember = "CitaId";
            cboCita.ValueMember = "CitaId";
        }

        private void SetComboValueStatus(int id)
        {
            for (int i = 0; i < cboEstado.Items.Count; i++)
                if (Convert.ToInt32(((dynamic)cboEstado.Items[i]).Valor) == id) { cboEstado.SelectedIndex = i; break; }
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            if (cboCita.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtDiagnostico.Text))
            {
                MessageBox.Show("Cita y Diagnóstico son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ObjetoResultado = new Cls_HistorialMedico
            {
                HistorialMedicoId = _historialId,
                CitaId = (int)cboCita.SelectedValue,
                Diagnostico = txtDiagnostico.Text.Trim(),
                Tratamiento = txtTratamiento.Text.Trim(),
                Observaciones = txtObservaciones.Text.Trim(),
                ProximaVisita = dtpProximaVisita.Checked ? dtpProximaVisita.Value : (DateTime?)null,
                EstadoId = Convert.ToInt32(((dynamic)cboEstado.SelectedItem).Valor),
                NombreEstado = cboEstado.Text
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
