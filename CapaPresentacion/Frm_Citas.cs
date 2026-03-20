using CapaEntidad;
using CapaLogicaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class Frm_Citas : Form
    {
        private readonly CN_Citas _negocio = new CN_Citas();
        private readonly CN_Mascotas _negocioMascotas = new CN_Mascotas();
        private readonly CN_Catalogos _negocioCatalogos = new CN_Catalogos();
        private BindingSource _bindingSource = new BindingSource();
        private List<Cls_Citas> _listaOriginal = new List<Cls_Citas>();
        private readonly Cls_Personal _usuarioActual;
        private int _citaIdSeleccionada = 0;

        public Frm_Citas(Cls_Personal usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
        }

        private void Frm_Citas_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarCombos();
            ListarCitas();
        }

        private void ConfigurarGrid()
        {
            dgvData.AutoGenerateColumns = false;
            dgvData.Columns.Clear();
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CitaId", Name = "citaId", HeaderText = "ID", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MascotaId", Name = "mascotaId", HeaderText = "MascotaId", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreMascota", Name = "nombreMascota", HeaderText = "Mascota", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "VeterinarioId", Name = "veterinarioId", HeaderText = "VeterinarioId", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreVeterinario", Name = "nombreVeterinario", HeaderText = "Veterinario", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FechaCita", Name = "fechaCita", HeaderText = "Fecha y Hora" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Notas", Name = "notas", HeaderText = "Notas" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EstadoId", Name = "estadoId", HeaderText = "EstadoId", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreEstado", Name = "nombreEstado", HeaderText = "Estado" });

            dgvData.DataSource = _bindingSource;
        }

        private void CargarCombos()
        {
            cboMascota.DataSource = _negocioMascotas.Listar();
            cboMascota.DisplayMember = "NombreMascota";
            cboMascota.ValueMember = "MascotaId";

            cboVeterinario.DataSource = _negocioCatalogos.ListarVeterinarios();
            cboVeterinario.DisplayMember = "PrimerNombre";
            cboVeterinario.ValueMember = "PersonalId";

            cboEstado.Items.Add(new { Valor = 3, Texto = "Pendiente" });
            cboEstado.Items.Add(new { Valor = 4, Texto = "Atendida" });
            cboEstado.Items.Add(new { Valor = 5, Texto = "Cancelada" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;
        }

        private void ListarCitas()
        {
            _listaOriginal = _negocio.Listar();
            _bindingSource.DataSource = new BindingList<Cls_Citas>(_listaOriginal);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje;
            Cls_Citas obj = new Cls_Citas()
            {
                CitaId = _citaIdSeleccionada,
                MascotaId = Convert.ToInt32(cboMascota.SelectedValue),
                VeterinarioId = Convert.ToInt32(cboVeterinario.SelectedValue),
                FechaCita = dtpFecha.Value,
                Notas = txtMotivo.Text.Trim(),
                EstadoId = 3,
                CreadoPor = _usuarioActual.PersonalId
            };

            int rolId = _usuarioActual.RolId;

            if (obj.CitaId == 0)
            {
                int idGenerado;
                mensaje = _negocio.Registrar(obj, rolId, out idGenerado);
                if (idGenerado > 0 || (mensaje != null && mensaje.Contains("correctamente")))
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarCitas();
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
                    ListarCitas();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_citaIdSeleccionada == 0) return;
            if (MessageBox.Show("¿Seguro de cancelar esta cita?", "Confirme", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string mensaje;
                if (_negocio.Eliminar(_citaIdSeleccionada, 1, out mensaje))
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarCitas();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            _citaIdSeleccionada = 0;
            txtMotivo.Clear();
            dtpFecha.Value = DateTime.Now;
            if (cboMascota.Items.Count > 0) cboMascota.SelectedIndex = 0;
            if (cboVeterinario.Items.Count > 0) cboVeterinario.SelectedIndex = 0;
            txtMotivo.Focus();
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                var c = (Cls_Citas)dgvData.CurrentRow.DataBoundItem;
                _citaIdSeleccionada = c.CitaId;
                cboMascota.SelectedValue = c.MascotaId;
                cboVeterinario.SelectedValue = c.VeterinarioId;
                dtpFecha.Value = c.FechaCita;
                txtMotivo.Text = c.Notas;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e) => LimpiarFormulario();

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string busqueda = txtBusqueda.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(busqueda))
            {
                _bindingSource.DataSource = new BindingList<Cls_Citas>(_listaOriginal);
            }
            else
            {
                var filtrada = _listaOriginal.Where(x => 
                    (x.NombreMascota != null && x.NombreMascota.ToLower().Contains(busqueda)) ||
                    (x.NombreVeterinario != null && x.NombreVeterinario.ToLower().Contains(busqueda)) ||
                    (x.Notas != null && x.Notas.ToLower().Contains(busqueda))
                ).ToList();
                _bindingSource.DataSource = new BindingList<Cls_Citas>(filtrada);
            }
        }
    }
}
