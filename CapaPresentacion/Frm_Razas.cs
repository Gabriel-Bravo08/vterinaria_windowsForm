using CapaEntidad;
using CapaLogicaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class Frm_Razas : Form
    {
        private readonly CN_Razas _negocio = new CN_Razas();
        private readonly CN_Especies _negocioEspecies = new CN_Especies();
        private BindingSource _bindingSource = new BindingSource();
        private List<Cls_Razas> _listaOriginal = new List<Cls_Razas>();
        private readonly Cls_Personal _usuarioActual;
        private int _razaIdSeleccionado = 0;

        public Frm_Razas(Cls_Personal usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
        }

        private void Frm_Razas_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarCombos();
            ListarRazas();
        }

        private void ConfigurarGrid()
        {
            dgvData.AutoGenerateColumns = false;
            dgvData.Columns.Clear();
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "RazaId", Name = "razaId", HeaderText = "ID", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EspecieId", Name = "especieId", HeaderText = "EspecieId", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreEspecie", Name = "nombreEspecie", HeaderText = "Especie", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreRaza", Name = "nombreRaza", HeaderText = "Raza", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EstadoId", Name = "estadoId", HeaderText = "EstadoId", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreEstado", Name = "nombreEstado", HeaderText = "Estado" });

            dgvData.DataSource = _bindingSource;
        }

        private void CargarCombos()
        {
            cboEspecie.DataSource = _negocioEspecies.Listar();
            cboEspecie.DisplayMember = "NombreEspecie";
            cboEspecie.ValueMember = "EspecieId";

            cboEstado.Items.Add(new { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new { Valor = 2, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;
        }

        private void ListarRazas()
        {
            _listaOriginal = _negocio.Listar();
            _bindingSource.DataSource = new BindingList<Cls_Razas>(_listaOriginal);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje;
            Cls_Razas obj = new Cls_Razas()
            {
                RazaId = _razaIdSeleccionado,
                EspecieId = (int)cboEspecie.SelectedValue,
                NombreRaza = txtRaza.Text.Trim(),
                EstadoId = (int)((dynamic)cboEstado.SelectedItem).Valor
            };

            int rolId = _usuarioActual.RolId;

            if (obj.RazaId == 0)
            {
                int idGenerado;
                mensaje = _negocio.Registrar(obj, rolId, out idGenerado);
                if (mensaje.Contains("correctamente") || idGenerado > 0)
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarRazas();
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
                    ListarRazas();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_razaIdSeleccionado == 0) return;
            if (MessageBox.Show("¿Seguro de desactivar esta raza?", "Confirme", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string mensaje;
                if (_negocio.Eliminar(_razaIdSeleccionado, _usuarioActual.RolId, out mensaje))
                {
                    MessageBox.Show("Raza desactivada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarRazas();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            _razaIdSeleccionado = 0;
            txtRaza.Clear();
            if (cboEspecie.Items.Count > 0) cboEspecie.SelectedIndex = 0;
            cboEstado.SelectedIndex = 0;
            txtRaza.Focus();
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                var r = (Cls_Razas)dgvData.CurrentRow.DataBoundItem;
                _razaIdSeleccionado = r.RazaId;
                cboEspecie.SelectedValue = r.EspecieId;
                txtRaza.Text = r.NombreRaza;

                int estadoId = r.EstadoId;
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
                _bindingSource.DataSource = new BindingList<Cls_Razas>(_listaOriginal);
            }
            else
            {
                var filtrada = _listaOriginal.Where(x => 
                    (x.NombreRaza != null && x.NombreRaza.ToLower().Contains(busqueda)) ||
                    (x.NombreEspecie != null && x.NombreEspecie.ToLower().Contains(busqueda))
                ).ToList();
                _bindingSource.DataSource = new BindingList<Cls_Razas>(filtrada);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e) => LimpiarFormulario();
    }
}
