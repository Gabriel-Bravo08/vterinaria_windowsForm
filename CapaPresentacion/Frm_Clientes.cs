using CapaEntidad;
using CapaLogicaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class Frm_Clientes : Form
    {
        private readonly CN_Clientes _negocio = new CN_Clientes();
        private BindingSource _bindingSource = new BindingSource();
        private List<Cls_Clientes> _listaOriginal = new List<Cls_Clientes>();
        private readonly Cls_Personal _usuarioActual;
        private int _clienteIdSeleccionado = 0;

        public Frm_Clientes(Cls_Personal usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
        }

        private void Frm_Clientes_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarEstados();
            ListarClientes();
        }

        private void ConfigurarGrid()
        {
            dgvData.AutoGenerateColumns = false;
            dgvData.Columns.Clear();
            
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ClienteId", Name = "clienteId", HeaderText = "ID", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nombre", Name = "nombre", HeaderText = "Nombre", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Apellido", Name = "apellido", HeaderText = "Apellido", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Telefono", Name = "telefono", HeaderText = "Teléfono" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", Name = "email", HeaderText = "Email" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Direccion", Name = "direccion", HeaderText = "Dirección" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EstadoId", Name = "estadoId", HeaderText = "EstadoId", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreEstado", Name = "nombreEstado", HeaderText = "Estado" });

            dgvData.DataSource = _bindingSource;
        }

        private void CargarEstados()
        {
            cboEstado.Items.Add(new { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new { Valor = 2, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;
        }

        private void ListarClientes()
        {
            _listaOriginal = _negocio.Listar();
            _bindingSource.DataSource = new BindingList<Cls_Clientes>(_listaOriginal);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            Cls_Clientes obj = new Cls_Clientes()
            {
                ClienteId = _clienteIdSeleccionado,
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Direccion = txtDireccion.Text.Trim(),
                EstadoId = (int)((dynamic)cboEstado.SelectedItem).Valor
            };

            int rolId = _usuarioActual.RolId;

            if (obj.ClienteId == 0) // Nuevo
            {
                int idGenerado;
                mensaje = _negocio.Registrar(obj, rolId, out idGenerado);
                if (idGenerado > 0 || mensaje.Contains("exitosamente"))
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarClientes();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else // Editar
            {
                bool resultado = _negocio.Editar(obj, rolId, out mensaje);
                if (resultado)
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarClientes();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_clienteIdSeleccionado == 0) return;
            if (MessageBox.Show("¿Seguro de desactivar?", "Confirmación", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string mensaje;
                if (_negocio.Eliminar(_clienteIdSeleccionado, _usuarioActual.RolId, out mensaje))
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarClientes();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e) => LimpiarFormulario();

        private void LimpiarFormulario()
        {
            _clienteIdSeleccionado = 0;
            txtNombre.Clear();
            txtApellido.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtDireccion.Clear();
            cboEstado.SelectedIndex = 0;
            txtNombre.Focus();
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                var cli = (Cls_Clientes)dgvData.CurrentRow.DataBoundItem;
                _clienteIdSeleccionado = cli.ClienteId;
                txtNombre.Text = cli.Nombre;
                txtApellido.Text = cli.Apellido;
                txtTelefono.Text = cli.Telefono;
                txtEmail.Text = cli.Email;
                txtDireccion.Text = cli.Direccion;

                int estadoId = cli.EstadoId;
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
                _bindingSource.DataSource = new BindingList<Cls_Clientes>(_listaOriginal);
            }
            else
            {
                var filtrada = _listaOriginal.Where(x => 
                    (x.Nombre != null && x.Nombre.ToLower().Contains(busqueda)) ||
                    (x.Apellido != null && x.Apellido.ToLower().Contains(busqueda)) ||
                    (x.Telefono != null && x.Telefono.ToLower().Contains(busqueda)) ||
                    (x.Email != null && x.Email.ToLower().Contains(busqueda))
                ).ToList();
                _bindingSource.DataSource = new BindingList<Cls_Clientes>(filtrada);
            }
        }
    }
}
