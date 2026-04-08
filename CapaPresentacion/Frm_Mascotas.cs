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
    public partial class Frm_Mascotas : Form
    {
        private readonly CN_Mascotas _negocio = new CN_Mascotas();
        private List<Cls_Mascotas> _listaOriginal = new List<Cls_Mascotas>();
        private BindingSource _bindingSource = new BindingSource();
        private readonly Cls_Personal _usuarioActual;

        public Frm_Mascotas(Cls_Personal usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
        }

        private void Frm_Mascotas_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            ListarMascotas();
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

            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MascotaId", Name = "mascotaId", HeaderText = "ID", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreMascota", Name = "nombreMascota", HeaderText = "Mascota", Width = 150 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreCliente", Name = "nombreCliente", HeaderText = "Dueño", Width = 180 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreEspecie", Name = "nombreEspecie", HeaderText = "Especie", Width = 120 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Color", Name = "color", HeaderText = "Color", Width = 100 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Peso", Name = "peso", HeaderText = "Peso", Width = 80 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreEstado", Name = "nombreEstado", HeaderText = "Estado", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            dgvData.DataSource = _bindingSource;
        }

        private void ListarMascotas()
        {
            _listaOriginal = _negocio.Listar();
            _bindingSource.DataSource = new BindingList<Cls_Mascotas>(_listaOriginal);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using (var modal = new Frm_Mascotas_Modal())
            {
                if (modal.ShowDialog() == DialogResult.OK)
                {
                    string mensaje;
                    int idGenerado;
                    mensaje = _negocio.Registrar(modal.ObjetoResultado, _usuarioActual.RolId, out idGenerado);
                    if (mensaje.Contains("correctamente") || idGenerado > 0)
                    {
                        MessageBox.Show("Mascota guardada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ListarMascotas();
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
                var m = (Cls_Mascotas)dgvData.CurrentRow.DataBoundItem;
                using (var modal = new Frm_Mascotas_Modal(m))
                {
                    if (modal.ShowDialog() == DialogResult.OK)
                    {
                        string mensaje;
                        if (_negocio.Editar(modal.ObjetoResultado, _usuarioActual.RolId, out mensaje))
                        {
                            MessageBox.Show("Mascota actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ListarMascotas();
                        }
                        else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (dgvData.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                var m = (Cls_Mascotas)dgvData.CurrentRow.DataBoundItem;
                if (MessageBox.Show($"¿Desea eliminar a '{m.NombreMascota}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje;
                    if (_negocio.Eliminar(m.MascotaId, _usuarioActual.RolId, out mensaje))
                    {
                        MessageBox.Show("Mascota eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ListarMascotas();
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
                _bindingSource.DataSource = new BindingList<Cls_Mascotas>(_listaOriginal);
            }
            else
            {
                var filtrada = _listaOriginal.Where(x =>
                    (x.NombreMascota != null && x.NombreMascota.ToLower().Contains(busqueda)) ||
                    (x.NombreCliente != null && x.NombreCliente.ToLower().Contains(busqueda)) ||
                    (x.NombreEspecie != null && x.NombreEspecie.ToLower().Contains(busqueda))
                ).ToList();
                _bindingSource.DataSource = new BindingList<Cls_Mascotas>(filtrada);
            }
        }
    }
}
