using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaLogicaNegocio
{
    public class CN_Razas
    {
        private CD_Razas _objCapaDatos = new CD_Razas();

        public List<Cls_Razas> Listar() => _objCapaDatos.SelectAll();

        public string Registrar(Cls_Razas obj, int rolId, out int idResultado)
        {
            idResultado = 0;
            if (obj.EspecieId == 0) return "Debe seleccionar una especie.";
            if (string.IsNullOrEmpty(obj.NombreRaza)) return "El nombre de la raza es obligatorio.";

            return _objCapaDatos.Create(obj, rolId, out idResultado);
        }

        public bool Editar(Cls_Razas obj, int rolId, out string mensaje)
        {
            if (obj.EspecieId == 0) { mensaje = "Debe seleccionar una especie."; return false; }
            if (string.IsNullOrEmpty(obj.NombreRaza)) { mensaje = "El nombre de la raza es obligatorio."; return false; }

            return _objCapaDatos.Update(obj, rolId, out mensaje);
        }

        public bool Eliminar(int id, int rolId, out string mensaje) => _objCapaDatos.Delete(id, rolId, out mensaje);
    }
}
