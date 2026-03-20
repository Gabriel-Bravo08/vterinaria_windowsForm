using CapaEntidad;
using CapaLogicaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class Frm_Personal : Form
    {
        private readonly CN_Personal _negocio = new CN_Personal();
        private readonly CN_Catalogos _negocioCatalogos = new CN_Catalogos();
        private BindingSource _bindingSource = new BindingSource();
        private List<Cls_Personal> _listaOriginal = new List<Cls_Personal>();
        private readonly Cls_Personal _usuarioActual;
        private int _personalIdSeleccionado = 0;

        public Frm_Personal(Cls_Personal usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
        }

        private void Frm_Personal_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarCombos();
            ListarPersonal();
        }

        private void ConfigurarGrid()
        {
            dgvData.AutoGenerateColumns = false;
            dgvData.Columns.Clear();
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PersonalId", Name = "personalId", HeaderText = "ID", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PrimerNombre", Name = "primerNombre", HeaderText = "1er Nombre" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SegundoNombre", Name = "segundoNombre", HeaderText = "2do Nombre" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PrimerApellido", Name = "primerApellido", HeaderText = "1er Apellido" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SegundoApellido", Name = "segundoApellido", HeaderText = "2do Apellido" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Telefono", Name = "telefono", HeaderText = "Teléfono" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", Name = "email", HeaderText = "Email" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreUsuario", Name = "nombreUsuario", HeaderText = "Usuario" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "RolId", Name = "rolId", HeaderText = "RolId", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreRol", Name = "nombreRol", HeaderText = "Rol" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EstadoId", Name = "estadoId", HeaderText = "EstadoId", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreEstado", Name = "nombreEstado", HeaderText = "Estado" });

            dgvData.DataSource = _bindingSource;
        }

        private void CargarCombos()
        {
            cboRol.DataSource = _negocioCatalogos.ListarRoles();
            cboRol.DisplayMember = "NombreRol";
            cboRol.ValueMember = "RolId";

            cboEstado.Items.Add(new { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new { Valor = 2, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;
        }

        private void ListarPersonal()
        {
            _listaOriginal = _negocio.Listar();
            _bindingSource.DataSource = new BindingList<Cls_Personal>(_listaOriginal);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje;
            Cls_Personal obj = new Cls_Personal()
            {
                PersonalId = _personalIdSeleccionado,
                PrimerNombre = txtPrimerNombre.Text.Trim(),
                SegundoNombre = txtSegundoNombre.Text.Trim(),
                PrimerApellido = txtPrimerApellido.Text.Trim(),
                SegundoApellido = txtSegundoApellido.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                NombreUsuario = txtUsuario.Text.Trim(),
                Clave = txtClave.Text.Trim(),
                RolId = (int)cboRol.SelectedValue,
                EstadoId = (int)((dynamic)cboEstado.SelectedItem).Valor
            };

            int rolExecId = _usuarioActual.RolId;
            if (obj.PersonalId == 0)
            {
                int idGenerado;
                mensaje = _negocio.Registrar(obj, rolExecId, out idGenerado);
                if (mensaje.Contains("correctamente") || idGenerado > 0)
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarPersonal();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                bool resultado = _negocio.Editar(obj, rolExecId, out mensaje);
                if (resultado)
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarPersonal();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_personalIdSeleccionado == 0) return;
            if (MessageBox.Show("¿Seguro de desactivar?", "Confirme", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string mensaje;
                if (_negocio.Eliminar(_personalIdSeleccionado, _usuarioActual.RolId, out mensaje))
                {
                    MessageBox.Show("Personal desactivado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarPersonal();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e) => LimpiarFormulario();

        private void LimpiarFormulario()
        {
            _personalIdSeleccionado = 0;
            txtPrimerNombre.Clear();
            txtSegundoNombre.Clear();
            txtPrimerApellido.Clear();
            txtSegundoApellido.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtUsuario.Clear();
            txtClave.Clear();
            cboRol.SelectedIndex = 0;
            cboEstado.SelectedIndex = 0;
            txtPrimerNombre.Focus();
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                var per = (Cls_Personal)dgvData.CurrentRow.DataBoundItem;
                _personalIdSeleccionado = per.PersonalId;
                txtPrimerNombre.Text = per.PrimerNombre;
                txtSegundoNombre.Text = per.SegundoNombre;
                txtPrimerApellido.Text = per.PrimerApellido;
                txtSegundoApellido.Text = per.SegundoApellido;
                txtTelefono.Text = per.Telefono;
                txtEmail.Text = per.Email;
                txtUsuario.Text = per.NombreUsuario;
                cboRol.SelectedValue = per.RolId;

                int estadoId = per.EstadoId;
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
                _bindingSource.DataSource = new BindingList<Cls_Personal>(_listaOriginal);
            }
            else
            {
                var filtrada = _listaOriginal.Where(x => 
                    (x.PrimerNombre != null && x.PrimerNombre.ToLower().Contains(busqueda)) ||
                    (x.PrimerApellido != null && x.PrimerApellido.ToLower().Contains(busqueda)) ||
                    (x.NombreUsuario != null && x.NombreUsuario.ToLower().Contains(busqueda)) ||
                    (x.Telefono != null && x.Telefono.ToLower().Contains(busqueda))
                ).ToList();
                _bindingSource.DataSource = new BindingList<Cls_Personal>(filtrada);
            }
        }
    }
}
