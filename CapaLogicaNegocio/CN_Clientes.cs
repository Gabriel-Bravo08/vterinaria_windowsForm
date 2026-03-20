using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogicaNegocio
{
    public class CN_Clientes
    {
        private CD_Clientes _objCapaDatos = new CD_Clientes();

        public List<Cls_Clientes> Listar()
        {
            return _objCapaDatos.SelectAll();
        }

        public string Registrar(Cls_Clientes obj, int rolId, out int idResultado)
        {
            idResultado = 0;
            if (string.IsNullOrEmpty(obj.Nombre)) return "El nombre no puede estar vacío.";
            if (string.IsNullOrEmpty(obj.Apellido)) return "El apellido no puede estar vacío.";

            return _objCapaDatos.Create(obj, rolId, out idResultado);
        }

        public bool Editar(Cls_Clientes obj, int rolId, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombre)) mensaje = "El nombre no puede estar vacío.";
            if (string.IsNullOrEmpty(obj.Apellido)) mensaje = "El apellido no puede estar vacío.";

            if (!string.IsNullOrEmpty(mensaje)) return false;

            return _objCapaDatos.Update(obj, rolId, out mensaje);
        }

        public bool Eliminar(int id, int rolId, out string mensaje)
        {
            return _objCapaDatos.Delete(id, rolId, out mensaje);
        }
    }
}
