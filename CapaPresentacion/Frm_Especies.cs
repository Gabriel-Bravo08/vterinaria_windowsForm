using CapaEntidad;
using CapaLogicaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class Frm_Especies : Form
    {
        private readonly CN_Especies _negocio = new CN_Especies();
        private BindingSource _bindingSource = new BindingSource();
        private List<Cls_Especies> _listaOriginal = new List<Cls_Especies>();
        private readonly Cls_Personal _usuarioActual;
        private int _especieIdSeleccionada = 0;

        public Frm_Especies(Cls_Personal usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
        }

        private void Frm_Especies_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarEstados();
            Listar();
        }

        private void ConfigurarGrid()
        {
            dgvData.AutoGenerateColumns = false;
            dgvData.Columns.Clear();
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EspecieId", Name = "especieId", HeaderText = "ID", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreEspecie", Name = "nombreEspecie", HeaderText = "Especie", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
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

        private void Listar()
        {
            _listaOriginal = _negocio.Listar();
            _bindingSource.DataSource = new BindingList<Cls_Especies>(_listaOriginal);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            Cls_Especies obj = new Cls_Especies()
            {
                EspecieId = _especieIdSeleccionada,
                NombreEspecie = txtNombre.Text.Trim(),
                EstadoId = (int)((dynamic)cboEstado.SelectedItem).Valor
            };

            int rolId = _usuarioActual.RolId;

            if (obj.EspecieId == 0)
            {
                int idResultado;
                mensaje = _negocio.Registrar(obj, rolId, out idResultado);
                if (idResultado > 0 || mensaje.Contains("correctamente"))
                {
                    MessageBox.Show("Especie registrada correctamente.", "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Limpiar();
                    Listar();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                bool resultado = _negocio.Editar(obj, rolId, out mensaje);
                if (resultado)
                {
                    MessageBox.Show("Especie actualizada correctamente.", "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Limpiar();
                    Listar();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_especieIdSeleccionada == 0) return;

            if (MessageBox.Show("¿Está seguro de eliminar esta especie?", "Confirmar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                string mensaje;
                int rolId = _usuarioActual.RolId;
                bool resultado = _negocio.Eliminar(_especieIdSeleccionada, rolId, out mensaje);

                if (resultado)
                {
                    MessageBox.Show("Especie eliminada correctamente.", "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Limpiar();
                    Listar();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Limpiar()
        {
            _especieIdSeleccionada = 0;
            txtNombre.Text = "";
            cboEstado.SelectedIndex = 0;
            txtNombre.Focus();
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                var esp = (Cls_Especies)dgvData.CurrentRow.DataBoundItem;
                _especieIdSeleccionada = esp.EspecieId;
                txtNombre.Text = esp.NombreEspecie;

                int idEstado = esp.EstadoId;
                for (int i = 0; i < cboEstado.Items.Count; i++)
                {
                    if ((int)((dynamic)cboEstado.Items[i]).Valor == idEstado)
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
                _bindingSource.DataSource = new BindingList<Cls_Especies>(_listaOriginal);
            }
            else
            {
                var filtrada = _listaOriginal.Where(x => 
                    (x.NombreEspecie != null && x.NombreEspecie.ToLower().Contains(busqueda))
                ).ToList();
                _bindingSource.DataSource = new BindingList<Cls_Especies>(filtrada);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e) => Limpiar();
    }
}
