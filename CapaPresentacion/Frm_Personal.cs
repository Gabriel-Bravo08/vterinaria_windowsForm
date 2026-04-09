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
    public partial class Frm_Personal : Form
    {
        private readonly CN_Personal _negocio = new CN_Personal();
        private List<Cls_Personal> _listaOriginal = new List<Cls_Personal>();
        private BindingSource _bindingSource = new BindingSource();
        private readonly Cls_Personal _usuarioActual;

        public Frm_Personal(Cls_Personal usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
        }

        private void Frm_Personal_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            ListarPersonal();
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

            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PersonalId", Name = "personalId", HeaderText = "ID", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreCompleto", Name = "nombreCompleto", HeaderText = "Nombre Completo", Width = 200 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreRol", Name = "nombreRol", HeaderText = "Rol", Width = 120 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreEspecialidad", Name = "nombreEspecialidad", HeaderText = "Especialidad", Width = 150 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreUsuario", Name = "nombreUsuario", HeaderText = "Usuario", Width = 120 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Telefono", Name = "telefono", HeaderText = "Teléfono", Width = 120 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", Name = "email", HeaderText = "Email", Width = 150 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreEstado", Name = "nombreEstado", HeaderText = "Estado", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            dgvData.DataSource = _bindingSource;
        }

        private void ListarPersonal()
        {
            _listaOriginal = _negocio.Listar();
            _bindingSource.DataSource = new BindingList<Cls_Personal>(_listaOriginal);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using (var modal = new Frm_Personal_Modal())
            {
                if (modal.ShowDialog() == DialogResult.OK)
                {
                    string mensaje;
                    int idGenerado;
                    mensaje = _negocio.Registrar(modal.ObjetoResultado, _usuarioActual.RolId, out idGenerado);
                    if (mensaje.Contains("correctamente") || idGenerado > 0)
                    {
                        MessageBox.Show("Personal guardado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ListarPersonal();
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
                var p = (Cls_Personal)dgvData.CurrentRow.DataBoundItem;
                using (var modal = new Frm_Personal_Modal(p))
                {
                    if (modal.ShowDialog() == DialogResult.OK)
                    {
                        string mensaje;
                        if (_negocio.Editar(modal.ObjetoResultado, _usuarioActual.RolId, out mensaje))
                        {
                            MessageBox.Show("Personal actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ListarPersonal();
                        }
                        else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (dgvData.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                var p = (Cls_Personal)dgvData.CurrentRow.DataBoundItem;
                if (MessageBox.Show($"¿Desea eliminar a '{p.NombreCompleto}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje;
                    if (_negocio.Eliminar(p.PersonalId, _usuarioActual.RolId, out mensaje))
                    {
                        MessageBox.Show("Personal de baja correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ListarPersonal();
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
                _bindingSource.DataSource = new BindingList<Cls_Personal>(_listaOriginal);
            }
            else
            {
                var filtrada = _listaOriginal.Where(x =>
                    (x.NombreCompleto != null && x.NombreCompleto.ToLower().Contains(busqueda)) ||
                    (x.NombreUsuario != null && x.NombreUsuario.ToLower().Contains(busqueda)) ||
                    (x.NombreRol != null && x.NombreRol.ToLower().Contains(busqueda)) ||
                    (x.NombreEspecialidad != null && x.NombreEspecialidad.ToLower().Contains(busqueda))
                ).ToList();
                _bindingSource.DataSource = new BindingList<Cls_Personal>(filtrada);
            }
        }
    }
}
