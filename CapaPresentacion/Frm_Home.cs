using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CapaLogicaNegocio;
using CapaEntidad;

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
            tslUsuarioLabel.Text = $"Conectado como: {_usuarioActual.NombreUsuario}";
        }

        private void Frm_Home_Load(object sender, EventArgs e)
        {
            CargarMenuDinamico();
        }

private void CargarMenuDinamico() {
    try {
        List<Cls_Menus> menus = _negocioMenu.ObtenerMenusPorRol(_usuarioActual.RolId);

        if (menus == null || menus.Count == 0) {
            MessageBox.Show($"No se encontraron menús para el Rol ID: {_usuarioActual.RolId}.", 
                            "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Aquí restauramos el bucle que crea los botones
        foreach (var menu in menus) {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = menu.Nombre;
            menuItem.ToolTipText = menu.Descripcion;
            menuItem.BackColor = System.Drawing.Color.White;
            menuItem.Click += (s, args) => {
                MessageBox.Show($"Abriendo el módulo {menu.Nombre}...", "Sistemas", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            // Los insertamos al inicio para que no choquen con el botón de "Cerrar Sesión" que está a la derecha
            menuStripPrincipal.Items.Insert(0, menuItem);
        }
    } catch (Exception ex) {
        MessageBox.Show($"Error al cargar el menú: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}



        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cerrar la sesión?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Tag = "logout"; // Marca para identificar que fue un logout
                this.Close();
            }
        }
    }
}
