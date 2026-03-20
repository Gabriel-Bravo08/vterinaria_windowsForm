using CapaEntidad;
using CapaLogicaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class Frm_Roles : Form
    {
        private readonly CN_Roles _negocio = new CN_Roles();
        private BindingSource _bindingSource = new BindingSource();
        private List<Cls_Roles> _listaOriginal = new List<Cls_Roles>();
        private readonly Cls_Personal _usuarioActual;
        private int _rolIdSeleccionado = 0;

        public Frm_Roles(Cls_Personal usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
        }

        private void Frm_Roles_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarEstados();
            ListarRoles();
        }

        private void ConfigurarGrid()
        {
            dgvData.AutoGenerateColumns = false;
            dgvData.Columns.Clear();
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "RolId", Name = "rolId", HeaderText = "ID", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreRol", Name = "nombreRol", HeaderText = "Nombre", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
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

        private void ListarRoles()
        {
            _listaOriginal = _negocio.Listar();
            _bindingSource.DataSource = new BindingList<Cls_Roles>(_listaOriginal);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje;
            Cls_Roles obj = new Cls_Roles()
            {
                RolId = _rolIdSeleccionado,
                NombreRol = txtNombre.Text.Trim(),
                EstadoId = Convert.ToInt32(((dynamic)cboEstado.SelectedItem).Valor)
            };

            if (obj.RolId == 0)
            {
                int idGenerado;
                mensaje = _negocio.Registrar(obj, out idGenerado);
                if (idGenerado > 0)
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarRoles();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                bool resultado = _negocio.Editar(obj, out mensaje);
                if (resultado)
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarRoles();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_rolIdSeleccionado == 0) return;
            if (MessageBox.Show("¿Seguro de eliminar este rol?", "Confirme", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string mensaje;
                if (_negocio.Eliminar(_rolIdSeleccionado, out mensaje))
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarRoles();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            _rolIdSeleccionado = 0;
            txtNombre.Clear();
            cboEstado.SelectedIndex = 0;
            txtNombre.Focus();
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                var r = (Cls_Roles)dgvData.CurrentRow.DataBoundItem;
                _rolIdSeleccionado = r.RolId;
                txtNombre.Text = r.NombreRol;
                int estadoId = r.EstadoId;

                for (int i = 0; i < cboEstado.Items.Count; i++)
                {
                    if (Convert.ToInt32(((dynamic)cboEstado.Items[i]).Valor) == estadoId)
                    {
                        cboEstado.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e) => LimpiarFormulario();

        public void btnBuscar_Click(object sender, EventArgs e)
        {
            string busqueda = (txtBusqueda != null) ? txtBusqueda.Text.Trim().ToLower() : "";
            if (string.IsNullOrEmpty(busqueda))
            {
                _bindingSource.DataSource = new BindingList<Cls_Roles>(_listaOriginal);
            }
            else
            {
                var filtrada = _listaOriginal.Where(x => 
                    (x.NombreRol != null && x.NombreRol.ToLower().Contains(busqueda))
                ).ToList();
                _bindingSource.DataSource = new BindingList<Cls_Roles>(filtrada);
            }
        }
    }
}
