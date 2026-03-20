using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using CapaLogicaNegocio;
using CapaEntidad;

namespace CapaPresentacion
{
    public partial class Frm_Login : Form
    {
        private readonly CN_Login _negocio = new CN_Login();

        // Para mover la ventana sin bordes
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        public Frm_Login()
        {
            InitializeComponent();
            // Suscribir eventos de mouse para mover el formulario
            this.MouseDown += Frm_Login_MouseDown;
        }

        private void Frm_Login_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = txtUsuario.Text.Trim();
                string clave = txtClave.Text.Trim();

                if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(clave))
                {
                    MessageBox.Show("Por favor ingrese usuario y contraseña.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Cls_Personal personal = _negocio.ValidarLogin(usuario, clave);

                if (personal != null)
                {
                    MessageBox.Show($"Bienvenido, {personal.PrimerNombre} {personal.PrimerApellido}!", "Inicio de Sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    Frm_Home home = new Frm_Home(personal);
                    this.Hide();
                    txtUsuario.Text = "";
                    txtClave.Text = "";
                    home.Show();
                    home.FormClosed += (s, args) =>
                    {
                        if (home.Tag != null && home.Tag.ToString() == "logout")
                        {
                            this.Show(); 
                        }
                        else
                        {
                            Application.Exit(); // Cierre total
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
