using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaLogicaNegocio
{
    public class CN_Roles
    {
        private CD_Roles _objCapaDatos = new CD_Roles();

        public List<Cls_Roles> Listar()
        {
            return _objCapaDatos.SelectAll();
        }

        public string Registrar(Cls_Roles obj, out int idGenerado)
        {
            idGenerado = 0;
            if (string.IsNullOrEmpty(obj.NombreRol))
                return "El nombre del rol no puede estar vacío.";

            return _objCapaDatos.Create(obj, out idGenerado);
        }

        public bool Editar(Cls_Roles obj, out string mensaje)
        {
            if (string.IsNullOrEmpty(obj.NombreRol))
            {
                mensaje = "El nombre del rol no puede estar vacío.";
                return false;
            }
            return _objCapaDatos.Update(obj, out mensaje);
        }

        public bool Eliminar(int id, out string mensaje)
        {
            return _objCapaDatos.Delete(id, out mensaje);
        }
    }
}
