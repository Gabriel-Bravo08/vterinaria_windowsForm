using System;
using System.Windows.Forms;
using CapaLogicaNegocio;
using CapaEntidad;

namespace CapaPresentacion
{
    public partial class Frm_Login : Form
    {
        private readonly CN_Login _negocio = new CN_Login();

        public Frm_Login()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = txtUsuario.Text.Trim();
                string clave = txtClave.Text.Trim();

                Cls_Personal personal = _negocio.ValidarLogin(usuario, clave);

                if (personal != null)
                {
                    MessageBox.Show($"Bienvenido, {personal.PrimerNombre} {personal.PrimerApellido}!", "Inicio de Sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Abrir el Home pasando el objeto personal (o al menos el RolId)
                    Frm_Home home = new Frm_Home(personal);
                    this.Hide();
                    txtUsuario.Text = "";
                    txtClave.Text = "";
                    home.Show();
                    home.FormClosed += (s, args) =>
                    {
                        if (home.Tag != null && home.Tag.ToString() == "logout")
                        {
                            this.Show(); // Reaparece el Login si fue Logout
                        }
                        else
                        {
                            this.Close(); // Cierra todo si se cerró directo la X
                        }
                    };
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
