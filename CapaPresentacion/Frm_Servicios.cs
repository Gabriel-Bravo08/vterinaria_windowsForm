using CapaEntidad;
using CapaLogicaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class Frm_Servicios : Form
    {
        private readonly CN_Servicios _negocio = new CN_Servicios();
        private BindingSource _bindingSource = new BindingSource();
        private List<Cls_Servicios> _listaOriginal = new List<Cls_Servicios>();
        private readonly Cls_Personal _usuarioActual;
        private int _servicioIdSeleccionado = 0;

        public Frm_Servicios(Cls_Personal usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
        }

        private void Frm_Servicios_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarCombos();
            ListarServicios();
        }

        private void ConfigurarGrid()
        {
            dgvData.AutoGenerateColumns = false;
            dgvData.Columns.Clear();
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ServicioId", Name = "servicioId", HeaderText = "ID", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreServicio", Name = "nombreServicio", HeaderText = "Servicio", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Descripcion", Name = "descripcion", HeaderText = "Descripción", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Precio", Name = "precio", HeaderText = "Precio" });
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

        private void ListarServicios()
        {
            _listaOriginal = _negocio.Listar();
            _bindingSource.DataSource = new BindingList<Cls_Servicios>(_listaOriginal);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje;
            decimal precio = 0;
            if (!decimal.TryParse(txtPrecio.Text, out precio))
            {
                MessageBox.Show("El precio debe ser un número válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Cls_Servicios obj = new Cls_Servicios()
            {
                ServicioId = _servicioIdSeleccionado,
                NombreServicio = txtServicio.Text.Trim(),
                Descripcion = txtDescripcion.Text.Trim(),
                Precio = precio,
                EstadoId = (int)((dynamic)cboEstado.SelectedItem).Valor
            };

            int rolId = _usuarioActual.RolId;

            if (obj.ServicioId == 0)
            {
                int idGenerado;
                mensaje = _negocio.Registrar(obj, rolId, out idGenerado);
                if (mensaje.Contains("correctamente") || idGenerado > 0)
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarServicios();
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
                    ListarServicios();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_servicioIdSeleccionado == 0) return;
            if (MessageBox.Show("¿Seguro de desactivar este servicio?", "Confirme", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string mensaje;
                if (_negocio.Eliminar(_servicioIdSeleccionado, _usuarioActual.RolId, out mensaje))
                {
                    MessageBox.Show("Servicio desactivado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarServicios();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            _servicioIdSeleccionado = 0;
            txtServicio.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();
            cboEstado.SelectedIndex = 0;
            txtServicio.Focus();
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                var s = (Cls_Servicios)dgvData.CurrentRow.DataBoundItem;
                _servicioIdSeleccionado = s.ServicioId;
                txtServicio.Text = s.NombreServicio;
                txtDescripcion.Text = s.Descripcion;
                txtPrecio.Text = s.Precio.ToString();

                int estadoId = s.EstadoId;
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

        private void btnLimpiar_Click(object sender, EventArgs e) => LimpiarFormulario();
    }
}
