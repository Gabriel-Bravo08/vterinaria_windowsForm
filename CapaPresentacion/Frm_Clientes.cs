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
    public partial class Frm_Clientes : Form
    {
        private readonly CN_Clientes _negocio = new CN_Clientes();
        private List<Cls_Clientes> _listaOriginal = new List<Cls_Clientes>();
        private BindingSource _bindingSource = new BindingSource();
        private readonly Cls_Personal _usuarioActual;

        public Frm_Clientes(Cls_Personal usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
        }

        private void Frm_Clientes_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            ListarClientes();
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

            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ClienteId", Name = "clienteId", HeaderText = "ID", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreCompleto", Name = "nombreCompleto", HeaderText = "Nombre Completo", Width = 200 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Telefono", Name = "telefono", HeaderText = "Teléfono", Width = 120 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", Name = "email", HeaderText = "Email", Width = 150 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Direccion", Name = "direccion", HeaderText = "Dirección", Width = 200 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreEstado", Name = "nombreEstado", HeaderText = "Estado", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            dgvData.DataSource = _bindingSource;
        }

        private void ListarClientes()
        {
            _listaOriginal = _negocio.Listar();
            _bindingSource.DataSource = new BindingList<Cls_Clientes>(_listaOriginal);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using (var modal = new Frm_Clientes_Modal())
            {
                if (modal.ShowDialog() == DialogResult.OK)
                {
                    string mensaje;
                    int idGenerado;
                    mensaje = _negocio.Registrar(modal.ObjetoResultado, _usuarioActual.RolId, out idGenerado);
                    if (mensaje.Contains("correctamente") || idGenerado > 0)
                    {
                        MessageBox.Show("Cliente guardado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ListarClientes();
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
                var c = (Cls_Clientes)dgvData.CurrentRow.DataBoundItem;
                using (var modal = new Frm_Clientes_Modal(c))
                {
                    if (modal.ShowDialog() == DialogResult.OK)
                    {
                        string mensaje;
                        if (_negocio.Editar(modal.ObjetoResultado, _usuarioActual.RolId, out mensaje))
                        {
                            MessageBox.Show("Cliente actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ListarClientes();
                        }
                        else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (dgvData.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                var c = (Cls_Clientes)dgvData.CurrentRow.DataBoundItem;
                if (MessageBox.Show($"¿Desea eliminar al cliente '{c.NombreCompleto}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje;
                    if (_negocio.Eliminar(c.ClienteId, _usuarioActual.RolId, out mensaje))
                    {
                        MessageBox.Show("Cliente eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ListarClientes();
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
                _bindingSource.DataSource = new BindingList<Cls_Clientes>(_listaOriginal);
            }
            else
            {
                var filtrada = _listaOriginal.Where(x =>
                    (x.Nombre != null && x.Nombre.ToLower().Contains(busqueda)) ||
                    (x.Apellido != null && x.Apellido.ToLower().Contains(busqueda)) ||
                    (x.Email != null && x.Email.ToLower().Contains(busqueda))
                ).ToList();
                _bindingSource.DataSource = new BindingList<Cls_Clientes>(filtrada);
            }
        }
    }
}
