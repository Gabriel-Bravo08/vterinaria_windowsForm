using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaLogicaNegocio
{
    public class CN_Especies
    {
        private CD_Especies _objCapaDatos = new CD_Especies();

        public List<Cls_Especies> Listar() => _objCapaDatos.SelectAll();

        public string Registrar(Cls_Especies obj, int rolId, out int idResultado)
        {
            idResultado = 0;
            if (string.IsNullOrEmpty(obj.NombreEspecie) || string.IsNullOrWhiteSpace(obj.NombreEspecie))
                return "Debe ingresar el nombre de la especie.";

            return _objCapaDatos.Create(obj, rolId, out idResultado);
        }

        public bool Editar(Cls_Especies obj, int rolId, out string mensaje)
        {
            if (string.IsNullOrEmpty(obj.NombreEspecie) || string.IsNullOrWhiteSpace(obj.NombreEspecie))
            {
                mensaje = "Debe ingresar el nombre de la especie.";
                return false;
            }

            return _objCapaDatos.Update(obj, rolId, out mensaje);
        }

        public bool Eliminar(int id, int rolId, out string mensaje) => _objCapaDatos.Delete(id, rolId, out mensaje);
    }
}
