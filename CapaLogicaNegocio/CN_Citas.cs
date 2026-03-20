using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;

namespace CapaLogicaNegocio
{
    public class CN_Citas
    {
        private CD_Citas _objCapaDatos = new CD_Citas();

        public List<Cls_Citas> Listar() => _objCapaDatos.SelectAll();

        public string Registrar(Cls_Citas obj, int rolId, out int idResultado)
        {
            idResultado = 0;
            if (obj.MascotaId == 0) return "Debe seleccionar una mascota.";
            if (obj.VeterinarioId == 0) return "Debe seleccionar un veterinario.";
            if (obj.FechaCita < DateTime.Now) return "La fecha de la cita no puede ser anterior a la actual.";

            return _objCapaDatos.Create(obj, rolId, out idResultado);
        }

        public bool Editar(Cls_Citas obj, int rolId, out string mensaje)
        {
            if (obj.MascotaId == 0) { mensaje = "Debe seleccionar una mascota."; return false; }
            if (obj.VeterinarioId == 0) { mensaje = "Debe seleccionar un veterinario."; return false; }

            return _objCapaDatos.Update(obj, rolId, out mensaje);
        }

        public bool Eliminar(int id, int rolId, out string mensaje) => _objCapaDatos.Delete(id, rolId, out mensaje);
    }
}
