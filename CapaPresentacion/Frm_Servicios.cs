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
    public partial class Frm_Servicios : Form
    {
        private readonly CN_Servicios _negocio = new CN_Servicios();
        private List<Cls_Servicios> _listaOriginal = new List<Cls_Servicios>();
        private BindingSource _bindingSource = new BindingSource();
        private readonly Cls_Personal _usuarioActual;

        public Frm_Servicios(Cls_Personal usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
        }

        private void Frm_Servicios_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            ListarServicios();
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

            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ServicioId", Name = "servicioId", HeaderText = "ID", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreServicio", Name = "nombreServicio", HeaderText = "Servicio", Width = 200 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Descripcion", Name = "descripcion", HeaderText = "Descripción", Width = 300 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Precio", Name = "precio", HeaderText = "Precio", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreEstado", Name = "nombreEstado", HeaderText = "Estado", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            dgvData.DataSource = _bindingSource;
        }

        private void ListarServicios()
        {
            _listaOriginal = _negocio.Listar();
            _bindingSource.DataSource = new BindingList<Cls_Servicios>(_listaOriginal);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using (var modal = new Frm_Servicios_Modal())
            {
                if (modal.ShowDialog() == DialogResult.OK)
                {
                    string mensaje;
                    int idGenerado;
                    mensaje = _negocio.Registrar(modal.ObjetoResultado, _usuarioActual.RolId, out idGenerado);
                    if (mensaje.Contains("correctamente") || idGenerado > 0)
                    {
                        MessageBox.Show("Servicio guardado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ListarServicios();
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
                var s = (Cls_Servicios)dgvData.CurrentRow.DataBoundItem;
                using (var modal = new Frm_Servicios_Modal(s))
                {
                    if (modal.ShowDialog() == DialogResult.OK)
                    {
                        string mensaje;
                        if (_negocio.Editar(modal.ObjetoResultado, _usuarioActual.RolId, out mensaje))
                        {
                            MessageBox.Show("Servicio actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ListarServicios();
                        }
                        else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (dgvData.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                var s = (Cls_Servicios)dgvData.CurrentRow.DataBoundItem;
                if (MessageBox.Show($"¿Desea eliminar el servicio '{s.NombreServicio}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje;
                    if (_negocio.Eliminar(s.ServicioId, _usuarioActual.RolId, out mensaje))
                    {
                        MessageBox.Show("Servicio eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ListarServicios();
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
                _bindingSource.DataSource = new BindingList<Cls_Servicios>(_listaOriginal);
            }
            else
            {
                var filtrada = _listaOriginal.Where(x =>
                    (x.NombreServicio != null && x.NombreServicio.ToLower().Contains(busqueda)) ||
                    (x.Descripcion != null && x.Descripcion.ToLower().Contains(busqueda))
                ).ToList();
                _bindingSource.DataSource = new BindingList<Cls_Servicios>(filtrada);
            }
        }
    }
}
