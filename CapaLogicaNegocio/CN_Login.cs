using CapaDatos;
using CapaEntidad;
using System;

namespace CapaLogicaNegocio
{
    public class CN_Login
    {
        private readonly CD_Login _capaDatos = new CD_Login();

        public Cls_Personal ValidarLogin(string usuario, string clave)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new Exception("El nombre de usuario es obligatorio.");
            if (string.IsNullOrWhiteSpace(clave))
                throw new Exception("La contraseña es obligatoria.");

            return _capaDatos.ValidarLogin(usuario, clave);
        }
    }
}
