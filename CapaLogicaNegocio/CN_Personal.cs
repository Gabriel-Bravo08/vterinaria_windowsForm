using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogicaNegocio
{
    public class CN_Personal
    {
        private CD_Personal _objCapaDatos = new CD_Personal();

        public List<Cls_Personal> Listar() => _objCapaDatos.SelectAll();

        public string Registrar(Cls_Personal obj, int rolExecId, out int idResultado)
        {
            idResultado = 0;
            if (string.IsNullOrEmpty(obj.PrimerNombre)) return "El primer nombre es obligatorio.";
            if (string.IsNullOrEmpty(obj.PrimerApellido)) return "El primer apellido es obligatorio.";
            if (string.IsNullOrEmpty(obj.NombreUsuario)) return "El nombre de usuario es obligatorio.";
            if (string.IsNullOrEmpty(obj.Clave)) return "La clave es obligatoria.";
            if (obj.RolId == 0) return "Debe seleccionar un rol.";

            return _objCapaDatos.Create(obj, rolExecId, out idResultado);
        }

        public bool Editar(Cls_Personal obj, int rolExecId, out string mensaje)
        {
            if (string.IsNullOrEmpty(obj.PrimerNombre)) { mensaje = "El primer nombre es obligatorio."; return false; }
            if (string.IsNullOrEmpty(obj.PrimerApellido)) { mensaje = "El primer apellido es obligatorio."; return false; }
            if (string.IsNullOrEmpty(obj.NombreUsuario)) { mensaje = "El nombre de usuario es obligatorio."; return false; }
            if (obj.RolId == 0) { mensaje = "Debe seleccionar un rol."; return false; }

            return _objCapaDatos.Update(obj, rolExecId, out mensaje);
        }

        public bool Eliminar(int id, int rolExecId, out string mensaje) => _objCapaDatos.Delete(id, rolExecId, out mensaje);
    }
}
