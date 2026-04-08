using CapaEntidad;
using CapaLogicaNegocio;
using CapaPresentacion.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class Frm_Home : Form
    {
        private static Cls_Personal usuarioActual;
        private static Form formularioActivo = null;

        public Frm_Home(Cls_Personal objUsuario)
        {
            usuarioActual = objUsuario;
            InitializeComponent();
            ConfigurarDisenoPremium();
        }

        private void Frm_Home_Load(object sender, EventArgs e)
        {
            lblUserName.Text = usuarioActual.NombreUsuario;
            lblUserRole.Text = usuarioActual.NombreRol;
            tslUsuarioLabel.Text = $"Usuario: {usuarioActual.NombreUsuario}";
            
            CargarFondo();
            ConfigurarMenu();
        }

        private void CargarFondo()
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, "assets", "adcivet_logo.png");
                if (!File.Exists(path))
                    path = @"c:\Users\creco\Documents\UDEM\I 2026\Programacion\PROYECTO\vterinaria_windowsForm\CapaPresentacion\assets\adcivet_logo.png";

                if (File.Exists(path))
                {
                    picLogo.Image = Image.FromFile(path);
                    picLogo.SizeMode = PictureBoxSizeMode.Zoom;
                    picLogo.BackColor = Color.FromArgb(248, 250, 252);
                }
            }
            catch { }
        }

        private void ConfigurarDisenoPremium()
        {
            tlpMain.ColumnStyles[0].Width = 260;
            lblAppName.Text = "ADCIVET";
            pnlUserProfile.BackColor = Color.Transparent;
        }

        private void ConfigurarMenu()
        {
            flpMenu.Controls.Clear();
            
            CN_Menu _negocioMenu = new CN_Menu();
            List<Cls_Menus> menusPermitidos = _negocioMenu.ObtenerMenusPorRol(usuarioActual.RolId);

            // 1. Sección Principal
            AgregarSeccionMenu("PRINCIPAL");
            if (menusPermitidos.Any(m => m.Nombre == "Dashboard"))
                AgregarBotonMenu("Dashboard", "🏠", (s, e) => AbrirModulo("Dashboard", null));

            // 2. Sección Gestión
            bool mostrarGestion = menusPermitidos.Any(m => new[] { "Clientes", "Mascotas", "Citas", "Historial" }.Contains(m.Nombre));
            if (mostrarGestion)
            {
                AgregarSeccionMenu("GESTIÓN");
                if (menusPermitidos.Any(m => m.Nombre == "Clientes"))
                    AgregarBotonMenu("Clientes", "👥", (s, e) => AbrirModulo("Clientes", new Frm_Clientes(usuarioActual)));
                
                if (menusPermitidos.Any(m => m.Nombre == "Mascotas"))
                    AgregarBotonMenu("Mascotas", "🐾", (s, e) => AbrirModulo("Mascotas", new Frm_Mascotas(usuarioActual)));
                
                if (menusPermitidos.Any(m => m.Nombre == "Citas"))
                    AgregarBotonMenu("Citas", "📅", (s, e) => AbrirModulo("Citas", new Frm_Citas(usuarioActual)));
                
                if (menusPermitidos.Any(m => m.Nombre == "Historial"))
                    AgregarBotonMenu("Historial", "📜", (s, e) => AbrirModulo("Historial Médico", new Frm_HistorialMedico(usuarioActual)));
            }

            // 3. Sección Catálogos
            bool mostrarCatalogos = menusPermitidos.Any(m => new[] { "Servicios", "Especies", "Razas", "Especialidades" }.Contains(m.Nombre));
            if (mostrarCatalogos)
            {
                AgregarSeccionMenu("CATÁLOGOS");
                if (menusPermitidos.Any(m => m.Nombre == "Servicios"))
                    AgregarBotonMenu("Servicios", "🛠️", (s, e) => AbrirModulo("Servicios", new Frm_Servicios(usuarioActual)));
                
                if (menusPermitidos.Any(m => m.Nombre == "Especies"))
                    AgregarBotonMenu("Especies", "🧬", (s, e) => AbrirModulo("Especies", new Frm_Especies(usuarioActual)));
                
                if (menusPermitidos.Any(m => m.Nombre == "Razas"))
                    AgregarBotonMenu("Razas", "🐕", (s, e) => AbrirModulo("Razas", new Frm_Razas(usuarioActual)));
                
                if (menusPermitidos.Any(m => m.Nombre == "Especialidades"))
                    AgregarBotonMenu("Especialidades", "🎓", (s, e) => AbrirModulo("Especialidades", new Frm_Especialidades(usuarioActual)));
            }

            // 4. Sección Sistema
            bool mostrarSistema = menusPermitidos.Any(m => new[] { "Personal", "Roles" }.Contains(m.Nombre));
            if (mostrarSistema)
            {
                AgregarSeccionMenu("SISTEMA");
                if (menusPermitidos.Any(m => m.Nombre == "Personal"))
                    AgregarBotonMenu("Personal", "👨‍⚕️", (s, e) => AbrirModulo("Personal", new Frm_Personal(usuarioActual)));
                
                if (menusPermitidos.Any(m => m.Nombre == "Roles"))
                    AgregarBotonMenu("Roles", "🛡️", (s, e) => AbrirModulo("Roles", new Frm_Roles(usuarioActual)));
            }
        }

        private void AgregarSeccionMenu(string titulo)
        {
            Label lbl = new Label
            {
                Text = titulo,
                ForeColor = Color.FromArgb(148, 163, 184),
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Size = new Size(240, 30),
                TextAlign = ContentAlignment.BottomLeft,
                Padding = new Padding(15, 0, 0, 5)
            };
            flpMenu.Controls.Add(lbl);
        }

        private void AgregarBotonMenu(string texto, string icono, EventHandler clickEvent)
        {
            Button btn = new Button
            {
                Text = $"   {icono}   {texto}",
                TextAlign = ContentAlignment.MiddleLeft,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                FlatStyle = FlatStyle.Flat,
                Height = 45,
                Width = 240,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Padding = new Padding(15, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 58, 138);
            if (clickEvent != null) btn.Click += clickEvent;
            flpMenu.Controls.Add(btn);
        }

        private void AbrirModulo(string nombre, Form formulario)
        {
            if (formulario == null)
            {
                if (formularioActivo != null) formularioActivo.Close();
                picLogo.Visible = true;
                lblModuleTitle.Text = "Panel Principal";
                btnGlobalAdd.Visible = false;
                return;
            }

            if (formularioActivo != null) formularioActivo.Close();
            formularioActivo = formulario;
            picLogo.Visible = false;

            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            pnlContainer.Controls.Add(formulario);
            formulario.BringToFront();
            formulario.Show();
            
            lblModuleTitle.Text = nombre;
            
            // Búsqueda exhaustiva del botón de agregar
            Button btnHijo = BuscarBotonRecursivo(formulario, "btnNuevo");
            if (btnHijo != null)
            {
                // En lugar de Ocultar (que rompe el PerformClick en algunos casos),
                // lo movemos fuera de la pantalla y le quitamos el tamaño.
                btnHijo.Location = new Point(-1000, -1000);
                btnHijo.Size = new Size(0, 0);
                btnGlobalAdd.Visible = true;
            }
            else
            {
                btnGlobalAdd.Visible = false;
            }
        }

        private Button BuscarBotonRecursivo(Control contenedor, string nombre)
        {
            foreach (Control c in contenedor.Controls)
            {
                if (c.Name == nombre && c is Button) return (Button)c;
                Control encontrado = BuscarBotonRecursivo(c, nombre);
                if (encontrado != null) return (Button)encontrado;
            }
            return null;
        }

        private void btnGlobalAdd_Click(object sender, EventArgs e)
        {
            if (formularioActivo == null) return;
            
            Button btnHijo = BuscarBotonRecursivo(formularioActivo, "btnNuevo");
            if (btnHijo != null)
            {
                // Disparar click
                btnHijo.PerformClick();
            }
        }

        private void btnToggleMenu_Click(object sender, EventArgs e)
        {
            if (tlpMain.ColumnStyles[0].Width >= 260)
                tlpMain.ColumnStyles[0].Width = 70;
            else
                tlpMain.ColumnStyles[0].Width = 260;
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar la sesión?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Tag = "logout";
                this.Close();
            }
        }
    }
}
