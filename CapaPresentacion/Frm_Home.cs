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
        private bool _sidebarExpand = true;


        public Frm_Home(Cls_Personal usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
            tslUsuarioLabel.Text = $"Conectado como: {_usuarioActual.NombreUsuario} ({_usuarioActual.NombreRol})";
            lblUserName.Text = _usuarioActual.NombreUsuario;
            lblUserRole.Text = _usuarioActual.NombreRol;
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
                    picSidebarLogo.Image = picLogo.Image;
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
                flpMenu.Controls.Clear();
                List<Cls_Menus> menus = _negocioMenu.ObtenerMenusPorRol(_usuarioActual.RolId);

                if (menus == null || menus.Count == 0) {
                    MessageBox.Show($"No se encontraron menús asignados para su rol.", 
                                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                foreach (var menu in menus) {
                    // Si el nombre coincide con categorías, añadimos botones secundarios o solo el principal
                    CrearSeccionMenu(menu);
                }
            } catch (Exception ex) {
                MessageBox.Show($"Error al cargar el menú: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CrearSeccionMenu(Cls_Menus menu)
        {
            // Título de la sección (opcional, como "OPERACIONES" en el diseño)
            Label lblSeccion = new Label();
            lblSeccion.Text = menu.Nombre.ToUpper();
            lblSeccion.ForeColor = Color.FromArgb(150, 255, 255, 255);
            lblSeccion.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            lblSeccion.Margin = new Padding(15, 15, 0, 5);
            lblSeccion.AutoSize = true;
            flpMenu.Controls.Add(lblSeccion);

            // Submenús específicos
            switch (menu.Nombre)
            {
                case "Seguridades":
                    flpMenu.Controls.Add(CrearBotonMenu("Personal", (s, e) => AbrirModulo("Personal")));
                    flpMenu.Controls.Add(CrearBotonMenu("Roles", (s, e) => AbrirModulo("Roles")));
                    break;
                case "Catálogos":
                    flpMenu.Controls.Add(CrearBotonMenu("Especies", (s, e) => AbrirModulo("Especies")));
                    flpMenu.Controls.Add(CrearBotonMenu("Razas", (s, e) => AbrirModulo("Razas")));
                    flpMenu.Controls.Add(CrearBotonMenu("Servicios", (s, e) => AbrirModulo("Servicios")));
                    flpMenu.Controls.Add(CrearBotonMenu("Especialidades", (s, e) => AbrirModulo("Especialidades")));
                    break;
                case "Gestión Veterinaria":
                    flpMenu.Controls.Add(CrearBotonMenu("Clientes", (s, e) => AbrirModulo("Clientes")));
                    flpMenu.Controls.Add(CrearBotonMenu("Mascotas", (s, e) => AbrirModulo("Mascotas")));
                    break;
                case "Citas y Consultas":
                    flpMenu.Controls.Add(CrearBotonMenu("Ver Citas", (s, e) => AbrirModulo("Citas")));
                    break;
                case "Historial Médico":
                    flpMenu.Controls.Add(CrearBotonMenu("Ver Historiales", (s, e) => AbrirModulo("Historial Médico")));
                    break;
                default:
                    flpMenu.Controls.Add(CrearBotonMenu(menu.Nombre, (s, e) => AbrirModulo(menu.Nombre)));
                    break;
            }
        }

        private Button CrearBotonMenu(string texto, EventHandler onClick)
        {
            Button btn = new Button();
            btn.Text = "    " + GetMenuIcon(texto) + "    " + texto;
            btn.Tag = texto; // Guardar texto original
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Size = new Size(240, 45);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(28, 64, 107);
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(38, 74, 117);
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            btn.Cursor = Cursors.Hand;
            btn.Margin = new Padding(0, 2, 0, 2);
            btn.Click += (s, e) => {
                ResetButtonStyles();
                btn.BackColor = Color.FromArgb(100, 181, 246);
                btn.ForeColor = Color.FromArgb(13, 40, 71);
                btn.Font = new Font(btn.Font, FontStyle.Bold);
                onClick(s, e);
            };
            return btn;
        }

        private string GetMenuIcon(string texto)
        {
            switch (texto)
            {
                case "Personal": return "👤";
                case "Roles": return "🔑";
                case "Especies": return "🐾";
                case "Razas": return "🏷️";
                case "Servicios": return "🩺";
                case "Especialidades": return "🎓";
                case "Clientes": return "👥";
                case "Mascotas": return "🐕";
                case "Ver Citas": return "📅";
                case "Ver Historiales": return "📋";
                default: return "🔹";
            }
        }


        private void ResetButtonStyles()
        {
            foreach (Control ctrl in flpMenu.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.BackColor = Color.Transparent;
                    btn.ForeColor = Color.White;
                    btn.Font = new Font(btn.Font, FontStyle.Regular);
                }
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
                lblModuleTitle.Text = nombreModulo;
                
                // Intentar ocultar el header interno del formulario cargado
                foreach (Control c in formulario.Controls) {
                    if (c.Name == "pnlHeader") c.Visible = false;
                }

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

        private void btnToggleMenu_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();
        }

        private void sidebarTimer_Tick(object sender, EventArgs e)
        {
            if (_sidebarExpand)
            {
                pnlSidebar.Width -= 10;
                if (pnlSidebar.Width <= 60)
                {
                    _sidebarExpand = false;
                    sidebarTimer.Stop();
                    lblAppName.Visible = false;
                    flpMenu.Padding = new Padding(5, 20, 5, 0);
                    foreach (Control ctrl in flpMenu.Controls)
                    {
                        if (ctrl is Button btn) btn.Text = "    " + GetMenuIcon(btn.Tag.ToString());
                        if (ctrl is Label lbl) lbl.Visible = false;
                    }
                }
            }
            else
            {
                pnlSidebar.Width += 10;
                if (pnlSidebar.Width >= 260)
                {
                    _sidebarExpand = true;
                    sidebarTimer.Stop();
                    lblAppName.Visible = true;
                    flpMenu.Padding = new Padding(10, 20, 10, 0);
                    foreach (Control ctrl in flpMenu.Controls)
                    {
                        if (ctrl is Button btn) btn.Text = "    " + GetMenuIcon(btn.Tag.ToString()) + "    " + btn.Tag.ToString();
                        if (ctrl is Label lbl) lbl.Visible = true;
                    }
                }
            }
        }

    }
}
