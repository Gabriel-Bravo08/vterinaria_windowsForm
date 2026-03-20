using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaLogicaNegocio
{
    public class CN_Especialidades
    {
        private CD_Especialidades _objCapaDatos = new CD_Especialidades();

        public List<Cls_Especialidades> Listar() => _objCapaDatos.SelectAll();

        public string Registrar(Cls_Especialidades obj, int rolId, out int idResultado)
        {
            idResultado = 0;
            if (string.IsNullOrEmpty(obj.NombreEspecialidad)) return "El nombre de la especialidad es obligatorio.";

            return _objCapaDatos.Create(obj, rolId, out idResultado);
        }

        public bool Editar(Cls_Especialidades obj, int rolId, out string mensaje)
        {
            if (string.IsNullOrEmpty(obj.NombreEspecialidad)) { mensaje = "El nombre de la especialidad es obligatorio."; return false; }

            return _objCapaDatos.Update(obj, rolId, out mensaje);
        }

        public bool Eliminar(int id, int rolId, out string mensaje) => _objCapaDatos.Delete(id, rolId, out mensaje);
    }
}
