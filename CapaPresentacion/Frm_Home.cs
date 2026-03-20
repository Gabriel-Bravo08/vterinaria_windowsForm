using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CapaLogicaNegocio;
using CapaEntidad;
using System.Drawing;
using System.IO;

namespace CapaPresentacion
{
    public partial class Frm_Home : Form
    {
        private readonly Cls_Personal _usuarioActual;
        private readonly CN_Menu _negocioMenu = new CN_Menu();

        public Frm_Home(Cls_Personal usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
            tslUsuarioLabel.Text = $"Conectado como: {_usuarioActual.NombreUsuario} ({_usuarioActual.NombreRol})";
            CargarLogo();
        }

        private void CargarLogo()
        {
            try
            {
                string pathLogo = Path.Combine(Application.StartupPath, "assets", "adcivet_logo.png");
                if (File.Exists(pathLogo))
                {
                    picLogo.Image = Image.FromFile(pathLogo);
                }
            }
            catch { /* Silencioso si falla la carga del logo */ }
        }

        private void Frm_Home_Load(object sender, EventArgs e)
        {
            CargarMenuDinamico();
        }

        private void CargarMenuDinamico() {
            try {
                List<Cls_Menus> menus = _negocioMenu.ObtenerMenusPorRol(_usuarioActual.RolId);

                if (menus == null || menus.Count == 0) {
                    MessageBox.Show($"No se encontraron menús asignados para su rol.", 
                                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Invertimos para que aparezcan en el orden correcto si se usa Insert(0,...)
                // O simplemente usamos Add para orden normal.
                foreach (var menu in menus) {
                    ToolStripMenuItem menuItem = new ToolStripMenuItem();
                    menuItem.Text = menu.Nombre;
                    menuItem.ToolTipText = menu.Descripcion;
                    menuItem.BackColor = Color.White;
                    
                    // Si el nombre coincide con categorías, añadimos submenús manuales para esta fase
                    AgregarSubmenus(menuItem, menu.Nombre);

                    menuStripPrincipal.Items.Insert(menuStripPrincipal.Items.Count - 1, menuItem);
                }
            } catch (Exception ex) {
                MessageBox.Show($"Error al cargar el menú: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AgregarSubmenus(ToolStripMenuItem padre, string categoria)
        {
            switch (categoria)
            {
                case "Seguridades":
                    padre.DropDownItems.Add("Personal", null, (s, e) => AbrirModulo("Personal"));
                    padre.DropDownItems.Add("Roles", null, (s, e) => AbrirModulo("Roles"));
                    break;
                case "Catálogos":
                    padre.DropDownItems.Add("Especies", null, (s, e) => AbrirModulo("Especies"));
                    padre.DropDownItems.Add("Razas", null, (s, e) => AbrirModulo("Razas"));
                    padre.DropDownItems.Add("Servicios", null, (s, e) => AbrirModulo("Servicios"));
                    padre.DropDownItems.Add("Especialidades", null, (s, e) => AbrirModulo("Especialidades"));
                    break;
                case "Gestión Veterinaria":
                    padre.DropDownItems.Add("Clientes", null, (s, e) => AbrirModulo("Clientes"));
                    padre.DropDownItems.Add("Mascotas", null, (s, e) => AbrirModulo("Mascotas"));
                    break;
                case "Citas y Consultas":
                    padre.DropDownItems.Add("Ver Citas", null, (s, e) => AbrirModulo("Citas"));
                    break;
                case "Historial Médico":
                    padre.DropDownItems.Add("Ver Historiales", null, (s, e) => AbrirModulo("Historial Médico"));
                    break;
                default:
                    padre.Click += (s, args) => AbrirModulo(categoria);
                    break;
            }
        }

        private void AbrirModulo(string nombreModulo)
        {
            Form formulario = null;

            switch (nombreModulo)
            {
                case "Clientes":
                    formulario = new Frm_Clientes(_usuarioActual);
                    break;
                case "Mascotas":
                    formulario = new Frm_Mascotas(_usuarioActual);
                    break;
                case "Personal":
                    formulario = new Frm_Personal(_usuarioActual);
                    break;
                case "Citas":
                    formulario = new Frm_Citas(_usuarioActual);
                    break;
                case "Historial Médico":
                    formulario = new Frm_HistorialMedico(_usuarioActual);
                    break;
                case "Servicios":
                    formulario = new Frm_Servicios(_usuarioActual);
                    break;
                case "Especies":
                    formulario = new Frm_Especies(_usuarioActual);
                    break;
                case "Razas":
                    formulario = new Frm_Razas(_usuarioActual);
                    break;
                case "Especialidades":
                    formulario = new Frm_Especialidades(_usuarioActual);
                    break;
                case "Roles":
                    formulario = new Frm_Roles(_usuarioActual);
                    break;
                default:
                    MessageBox.Show($"El módulo {nombreModulo} aún no ha sido implementado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
            }

            if (formulario != null)
            {
                if (pnlContainer.Controls.Count > 0)
                    pnlContainer.Controls.Clear();

                formulario.TopLevel = false;
                formulario.FormBorderStyle = FormBorderStyle.None;
                formulario.Dock = DockStyle.Fill;
                pnlContainer.Controls.Add(formulario);
                formulario.Show();
            }
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cerrar la sesión?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Tag = "logout";
                this.Close();
            }
        }
    }
}
