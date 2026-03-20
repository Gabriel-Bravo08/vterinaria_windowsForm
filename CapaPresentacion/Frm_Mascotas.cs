using CapaEntidad;
using CapaLogicaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class Frm_Mascotas : Form
    {
        private readonly CN_Mascotas _negocio = new CN_Mascotas();
        private readonly CN_Clientes _negocioClientes = new CN_Clientes();
        private readonly CN_Catalogos _negocioCatalogos = new CN_Catalogos();
        private BindingSource _bindingSource = new BindingSource();
        private List<Cls_Mascotas> _listaOriginal = new List<Cls_Mascotas>();
        private readonly Cls_Personal _usuarioActual;
        private int _mascotaIdSeleccionado = 0;

        public Frm_Mascotas(Cls_Personal usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
        }

        private void Frm_Mascotas_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarCombos();
            ListarMascotas();
        }

        private void ConfigurarGrid()
        {
            dgvData.AutoGenerateColumns = false;
            dgvData.Columns.Clear();
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MascotaId", Name = "mascotaId", HeaderText = "ID", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreMascota", Name = "nombreMascota", HeaderText = "Nombre", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ClienteId", Name = "clienteId", HeaderText = "ClienteId", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreCliente", Name = "nombreCliente", HeaderText = "Dueño" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EspecieId", Name = "especieId", HeaderText = "EspecieId", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreEspecie", Name = "nombreEspecie", HeaderText = "Especie" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FechaNacimiento", Name = "fechaNacimiento", HeaderText = "F. Nac." });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Peso", Name = "peso", HeaderText = "Peso (kg)" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Color", Name = "color", HeaderText = "Color" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EstadoId", Name = "estadoId", HeaderText = "EstadoId", Visible = false });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreEstado", Name = "nombreEstado", HeaderText = "Estado" });

            dgvData.DataSource = _bindingSource;
        }

        private void CargarCombos()
        {
            cboCliente.DataSource = _negocioClientes.Listar();
            cboCliente.DisplayMember = "Nombre";
            cboCliente.ValueMember = "ClienteId";

            cboEspecie.DataSource = _negocioCatalogos.ListarEspecies();
            cboEspecie.DisplayMember = "NombreEspecie";
            cboEspecie.ValueMember = "EspecieId";

            cboEstado.Items.Add(new { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new { Valor = 2, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;
        }

        private void ListarMascotas()
        {
            _listaOriginal = _negocio.Listar();
            _bindingSource.DataSource = new BindingList<Cls_Mascotas>(_listaOriginal);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje;
            Cls_Mascotas obj = new Cls_Mascotas()
            {
                MascotaId = _mascotaIdSeleccionado,
                NombreMascota = txtNombreMascota.Text.Trim(),
                ClienteId = (int)cboCliente.SelectedValue,
                EspecieId = (int)cboEspecie.SelectedValue,
                FechaNacimiento = dtpFechaNac.Value,
                Peso = numPeso.Value,
                Color = txtColor.Text.Trim(),
                EstadoId = (int)((dynamic)cboEstado.SelectedItem).Valor
            };

            int rolId = _usuarioActual.RolId;

            if (obj.MascotaId == 0)
            {
                int idGenerado;
                mensaje = _negocio.Registrar(obj, rolId, out idGenerado);
                if (mensaje.Contains("correctamente") || idGenerado > 0)
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarMascotas();
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
                    ListarMascotas();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_mascotaIdSeleccionado == 0) return;
            if (MessageBox.Show("¿Seguro de desactivar?", "Confirme", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string mensaje;
                if (_negocio.Eliminar(_mascotaIdSeleccionado, _usuarioActual.RolId, out mensaje))
                {
                    MessageBox.Show("Mascota desactivada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarMascotas();
                    LimpiarFormulario();
                }
                else MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            _mascotaIdSeleccionado = 0;
            txtNombreMascota.Clear();
            txtColor.Clear();
            numPeso.Value = 0;
            dtpFechaNac.Value = DateTime.Now;
            cboEstado.SelectedIndex = 0;
            txtNombreMascota.Focus();
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                var m = (Cls_Mascotas)dgvData.CurrentRow.DataBoundItem;
                _mascotaIdSeleccionado = m.MascotaId;
                txtNombreMascota.Text = m.NombreMascota;
                cboCliente.SelectedValue = m.ClienteId;
                cboEspecie.SelectedValue = m.EspecieId;
                txtColor.Text = m.Color;
                numPeso.Value = m.Peso ?? 0;
                if (m.FechaNacimiento.HasValue)
                {
                    dtpFechaNac.Value = m.FechaNacimiento.Value;
                    dtpFechaNac.Checked = true;
                }
                else
                {
                    dtpFechaNac.Checked = false;
                }

                int estadoId = m.EstadoId;
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
                _bindingSource.DataSource = new BindingList<Cls_Mascotas>(_listaOriginal);
            }
            else
            {
                var filtrada = _listaOriginal.Where(x => 
                    (x.NombreMascota != null && x.NombreMascota.ToLower().Contains(busqueda)) ||
                    (x.NombreCliente != null && x.NombreCliente.ToLower().Contains(busqueda)) ||
                    (x.NombreEspecie != null && x.NombreEspecie.ToLower().Contains(busqueda)) ||
                    (x.Color != null && x.Color.ToLower().Contains(busqueda))
                ).ToList();
                _bindingSource.DataSource = new BindingList<Cls_Mascotas>(filtrada);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e) => LimpiarFormulario();
    }
}
