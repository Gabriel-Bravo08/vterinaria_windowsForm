using CapaEntidad;
using CapaLogicaNegocio;
using CapaPresentacion.Modals;
using CapaPresentacion.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class Frm_HistorialMedico : Form
    {
        private readonly CN_HistorialMedico _negocio = new CN_HistorialMedico();
        private List<Cls_HistorialMedico> _listaOriginal = new List<Cls_HistorialMedico>();
        private BindingSource _bindingSource = new BindingSource();
        private readonly Cls_Personal _usuarioActual;

        public Frm_HistorialMedico(Cls_Personal usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
        }

        private void Frm_HistorialMedico_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            ListarHistorial();
            VisualStyle.ApplyGridStyle(dgvData);
        }

        private void ConfigurarGrid()
        {
            dgvData.RowTemplate.Height = 35;
            dgvData.GridColor = System.Drawing.Color.FromArgb(235, 239, 242);
            dgvData.AutoGenerateColumns = false;
            dgvData.Columns.Clear();

            var btnEditar = new DataGridViewButtonColumn
            {
                HeaderText = "",
                Text = "📝",
                Name = "btnEditar",
                UseColumnTextForButtonValue = true,
                Width = 35,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                FlatStyle = FlatStyle.Flat
            };
            btnEditar.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvData.Columns.Add(btnEditar);

            var btnEliminar = new DataGridViewButtonColumn
            {
                HeaderText = "",
                Text = "🗑️",
                Name = "btnEliminar",
                UseColumnTextForButtonValue = true,
                Width = 35,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                FlatStyle = FlatStyle.Flat
            };
            btnEliminar.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvData.Columns.Add(btnEliminar);

            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "HistorialMedicoId", Name = "historialMedicoId", HeaderText = "ID", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreMascota", Name = "nombreMascota", HeaderText = "Mascota", Width = 150 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreVeterinario", Name = "nombreVeterinario", HeaderText = "Veterinario", Width = 180 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Diagnostico", Name = "diagnostico", HeaderText = "Diagnóstico", Width = 250 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ProximaVisita", Name = "proximaVisita", HeaderText = "Próxima Visita", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" } });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreEstado", Name = "nombreEstado", HeaderText = "Estado", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            dgvData.DataSource = _bindingSource;
        }

        private void ListarHistorial()
        {
            _listaOriginal = _negocio.Listar();
            _bindingSource.DataSource = new BindingList<Cls_HistorialMedico>(_listaOriginal);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using (var modal = new Frm_HistorialMedico_Modal())
            {
                if (modal.ShowDialog() == DialogResult.OK)
                {
                    string mensaje;
                    int idGenerado;
                    mensaje = _negocio.Registrar(modal.ObjetoResultado, _usuarioActual.RolId, out idGenerado);
                    if (mensaje.Contains("correctamente") || idGenerado > 0)
                    {
                        MessageBox.Show("Registro médico guardado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ListarHistorial();
                    }
                    else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvData.Columns[e.ColumnIndex].Name == "btnEditar")
            {
                var h = (Cls_HistorialMedico)dgvData.CurrentRow.DataBoundItem;
                using (var modal = new Frm_HistorialMedico_Modal(h))
                {
                    if (modal.ShowDialog() == DialogResult.OK)
                    {
                        string mensaje;
                        if (_negocio.Editar(modal.ObjetoResultado, _usuarioActual.RolId, out mensaje))
                        {
                            MessageBox.Show("Registro médico actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ListarHistorial();
                        }
                        else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (dgvData.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                var h = (Cls_HistorialMedico)dgvData.CurrentRow.DataBoundItem;
                if (MessageBox.Show($"¿Desea eliminar el historial de '{h.NombreMascota}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje;
                    if (_negocio.Eliminar(h.HistorialMedicoId, _usuarioActual.RolId, out mensaje))
                    {
                        MessageBox.Show("Registro médico eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ListarHistorial();
                    }
                    else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    (x.Diagnostico != null && x.Diagnostico.ToLower().Contains(busqueda))
                ).ToList();
                _bindingSource.DataSource = new BindingList<Cls_HistorialMedico>(filtrada);
            }
        }
    }
}
