using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogicaNegocio
{
    public class CN_Mascotas
    {
        private CD_Mascotas _objCapaDatos = new CD_Mascotas();

        public List<Cls_Mascotas> Listar() => _objCapaDatos.SelectAll();

        public string Registrar(Cls_Mascotas obj, int rolId, out int idResultado)
        {
            idResultado = 0;
            if (string.IsNullOrEmpty(obj.NombreMascota)) return "El nombre de la mascota no puede estar vacío.";
            if (obj.ClienteId == 0) return "Debe seleccionar un cliente.";
            if (obj.EspecieId == 0) return "Debe seleccionar una especie.";

            return _objCapaDatos.Create(obj, rolId, out idResultado);
        }

        public bool Editar(Cls_Mascotas obj, int rolId, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.NombreMascota)) mensaje = "El nombre de la mascota no puede estar vacío.";
            if (obj.ClienteId == 0) mensaje = "Debe seleccionar un cliente.";
            if (obj.EspecieId == 0) mensaje = "Debe seleccionar una especie.";
            if (!string.IsNullOrEmpty(mensaje)) return false;

            return _objCapaDatos.Update(obj, rolId, out mensaje);
        }

        public bool Eliminar(int id, int rolId, out string mensaje) => _objCapaDatos.Delete(id, rolId, out mensaje);
    }
}
