using CapaEntidad;
using CapaLogicaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class Frm_Especialidades : Form
    {
        private readonly CN_Especialidades _negocio = new CN_Especialidades();
        private BindingSource _bindingSource = new BindingSource();
        private List<Cls_Especialidades> _listaOriginal = new List<Cls_Especialidades>();
        private readonly Cls_Personal _usuarioActual;
        private int _especialidadIdSeleccionada = 0;

        public Frm_Especialidades(Cls_Personal usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
        }

        private void Frm_Especialidades_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarCombos();
            ListarEspecialidades();
        }

        private void ConfigurarGrid()
        {
            dgvData.AutoGenerateColumns = false;
            dgvData.Columns.Clear();
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EspecialidadId", Name = "especialidadId", HeaderText = "ID", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreEspecialidad", Name = "nombreEspecialidad", HeaderText = "Especialidad", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EstadoId", Name = "estadoId", HeaderText = "EstadoId", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreEstado", Name = "nombreEstado", HeaderText = "Estado" });

            dgvData.DataSource = _bindingSource;
        }

        private void CargarCombos()
        {
            cboEstado.Items.Add(new { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new { Valor = 2, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;
        }

        private void ListarEspecialidades()
        {
            _listaOriginal = _negocio.Listar();
            _bindingSource.DataSource = new BindingList<Cls_Especialidades>(_listaOriginal);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje;
            Cls_Especialidades obj = new Cls_Especialidades()
            {
                EspecialidadId = _especialidadIdSeleccionada,
                NombreEspecialidad = txtEspecialidad.Text.Trim(),
                EstadoId = (int)((dynamic)cboEstado.SelectedItem).Valor
            };

            int rolId = _usuarioActual.RolId;

            if (obj.EspecialidadId == 0)
            {
                int idGenerado;
                mensaje = _negocio.Registrar(obj, rolId, out idGenerado);
                if (mensaje.Contains("correctamente") || idGenerado > 0)
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarEspecialidades();
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
                    ListarEspecialidades();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_especialidadIdSeleccionada == 0) return;
            if (MessageBox.Show("¿Seguro de desactivar esta especialidad?", "Confirme", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string mensaje;
                if (_negocio.Eliminar(_especialidadIdSeleccionada, _usuarioActual.RolId, out mensaje))
                {
                    MessageBox.Show("Especialidad desactivada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarEspecialidades();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            _especialidadIdSeleccionada = 0;
            txtEspecialidad.Clear();
            cboEstado.SelectedIndex = 0;
            txtEspecialidad.Focus();
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                var esp = (Cls_Especialidades)dgvData.CurrentRow.DataBoundItem;
                _especialidadIdSeleccionada = esp.EspecialidadId;
                txtEspecialidad.Text = esp.NombreEspecialidad;

                int estadoId = esp.EstadoId;
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
                _bindingSource.DataSource = new BindingList<Cls_Especialidades>(_listaOriginal);
            }
            else
            {
                var filtrada = _listaOriginal.Where(x => 
                    (x.NombreEspecialidad != null && x.NombreEspecialidad.ToLower().Contains(busqueda))
                ).ToList();
                _bindingSource.DataSource = new BindingList<Cls_Especialidades>(filtrada);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e) => LimpiarFormulario();
    }
}
