using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaLogicaNegocio
{
    public class CN_Servicios
    {
        private CD_Servicios _objCapaDatos = new CD_Servicios();

        public List<Cls_Servicios> Listar() => _objCapaDatos.SelectAll();

        public string Registrar(Cls_Servicios obj, int rolId, out int idResultado)
        {
            idResultado = 0;
            if (string.IsNullOrEmpty(obj.NombreServicio)) return "El nombre del servicio es obligatorio.";
            if (obj.Precio < 0) return "El precio no puede ser negativo.";

            return _objCapaDatos.Create(obj, rolId, out idResultado);
        }

        public bool Editar(Cls_Servicios obj, int rolId, out string mensaje)
        {
            if (string.IsNullOrEmpty(obj.NombreServicio)) { mensaje = "El nombre del servicio es obligatorio."; return false; }
            if (obj.Precio < 0) { mensaje = "El precio no puede ser negativo."; return false; }

            return _objCapaDatos.Update(obj, rolId, out mensaje);
        }

        public bool Eliminar(int id, int rolId, out string mensaje) => _objCapaDatos.Delete(id, rolId, out mensaje);
    }
}
