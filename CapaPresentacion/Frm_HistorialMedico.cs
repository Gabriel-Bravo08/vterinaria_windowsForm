using CapaEntidad;
using CapaLogicaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class Frm_HistorialMedico : Form
    {
        private readonly CN_HistorialMedico _negocio = new CN_HistorialMedico();
        private readonly CN_Citas _negocioCitas = new CN_Citas();
        private BindingSource _bindingSource = new BindingSource();
        private List<Cls_HistorialMedico> _listaOriginal = new List<Cls_HistorialMedico>();
        private readonly Cls_Personal _usuarioActual;
        private int _historialIdSeleccionado = 0;

        public Frm_HistorialMedico(Cls_Personal usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
        }

        private void Frm_HistorialMedico_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarCombos();
            ListarHistorial();
        }

        private void ConfigurarGrid()
        {
            dgvData.AutoGenerateColumns = false;
            dgvData.Columns.Clear();
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "HistorialMedicoId", Name = "historialMedicoId", HeaderText = "ID", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CitaId", Name = "citaId", HeaderText = "CitaId", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreMascota", Name = "nombreMascota", HeaderText = "Mascota" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreVeterinario", Name = "nombreVeterinario", HeaderText = "Veterinario" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Diagnostico", Name = "diagnostico", HeaderText = "Diagnóstico", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Tratamiento", Name = "tratamiento", HeaderText = "Tratamiento" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Observaciones", Name = "observaciones", HeaderText = "Observaciones" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ProximaVisita", Name = "proximaVisita", HeaderText = "Próxima Visita" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EstadoId", Name = "estadoId", HeaderText = "EstadoId", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreEstado", Name = "nombreEstado", HeaderText = "Estado" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FechaCreacion", Name = "fechaCreacion", HeaderText = "Fecha Registro" });

            dgvData.DataSource = _bindingSource;
        }

        private void CargarCombos()
        {
            cboCita.DataSource = _negocioCitas.Listar();
            cboCita.DisplayMember = "CitaId";
            cboCita.ValueMember = "CitaId";

            cboEstado.Items.Add(new { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new { Valor = 2, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;
        }

        private void ListarHistorial()
        {
            _listaOriginal = _negocio.Listar();
            _bindingSource.DataSource = new BindingList<Cls_HistorialMedico>(_listaOriginal);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje;
            Cls_HistorialMedico obj = new Cls_HistorialMedico()
            {
                HistorialMedicoId = _historialIdSeleccionado,
                CitaId = (int)cboCita.SelectedValue,
                Diagnostico = txtDiagnostico.Text.Trim(),
                Tratamiento = txtTratamiento.Text.Trim(),
                Observaciones = txtObservaciones.Text.Trim(),
                ProximaVisita = dtpProximaVisita.Checked ? dtpProximaVisita.Value : (DateTime?)null,
                EstadoId = (int)((dynamic)cboEstado.SelectedItem).Valor
            };

            int rolId = _usuarioActual.RolId;

            if (obj.HistorialMedicoId == 0)
            {
                int idGenerado;
                mensaje = _negocio.Registrar(obj, rolId, out idGenerado);
                if (mensaje.Contains("correctamente") || idGenerado > 0)
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarHistorial();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                bool resultado = _negocio.Editar(obj, rolId, out mensaje);
                if (resultado)
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarHistorial();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_historialIdSeleccionado == 0) return;
            if (MessageBox.Show("¿Seguro de eliminar este registro?", "Confirme", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string mensaje;
                if (_negocio.Eliminar(_historialIdSeleccionado, _usuarioActual.RolId, out mensaje))
                {
                    MessageBox.Show("Registro eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarHistorial();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            _historialIdSeleccionado = 0;
            txtDiagnostico.Clear();
            txtTratamiento.Clear();
            txtObservaciones.Clear();
            dtpProximaVisita.Checked = false;
            if (cboCita.Items.Count > 0) cboCita.SelectedIndex = 0;
            cboEstado.SelectedIndex = 0;
            txtDiagnostico.Focus();
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                var h = (Cls_HistorialMedico)dgvData.CurrentRow.DataBoundItem;
                _historialIdSeleccionado = h.HistorialMedicoId;
                cboCita.SelectedValue = h.CitaId;
                txtDiagnostico.Text = h.Diagnostico;
                txtTratamiento.Text = h.Tratamiento;
                txtObservaciones.Text = h.Observaciones;

                if (h.ProximaVisita.HasValue)
                {
                    dtpProximaVisita.Checked = true;
                    dtpProximaVisita.Value = h.ProximaVisita.Value;
                }
                else dtpProximaVisita.Checked = false;

                int estadoId = h.EstadoId;
                for (int i = 0; i < cboEstado.Items.Count; i++)
                {
                    if ((int)((dynamic)cboEstado.Items[i]).Valor == estadoId)
                    {
                        cboEstado.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string busqueda = txtBusqueda.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(busqueda))
            {
                _bindingSource.DataSource = new BindingList<Cls_HistorialMedico>(_listaOriginal);
            }
            else
            {
                var filtrada = _listaOriginal.Where(x => 
                    (x.NombreMascota != null && x.NombreMascota.ToLower().Contains(busqueda)) ||
                    (x.NombreVeterinario != null && x.NombreVeterinario.ToLower().Contains(busqueda)) ||
                    (x.Diagnostico != null && x.Diagnostico.ToLower().Contains(busqueda)) ||
                    (x.Tratamiento != null && x.Tratamiento.ToLower().Contains(busqueda))
                ).ToList();
                _bindingSource.DataSource = new BindingList<Cls_HistorialMedico>(filtrada);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e) => LimpiarFormulario();
    }
}
