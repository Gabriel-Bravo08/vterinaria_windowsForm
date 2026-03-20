using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaLogicaNegocio
{
    public class CN_HistorialMedico
    {
        private CD_HistorialMedico _objCapaDatos = new CD_HistorialMedico();

        public List<Cls_HistorialMedico> Listar() => _objCapaDatos.SelectAll();

        public string Registrar(Cls_HistorialMedico obj, int rolId, out int idResultado)
        {
            idResultado = 0;
            if (obj.CitaId == 0) return "Debe seleccionar una cita.";
            if (string.IsNullOrEmpty(obj.Diagnostico)) return "El diagnóstico es obligatorio.";

            return _objCapaDatos.Create(obj, rolId, out idResultado);
        }

        public bool Editar(Cls_HistorialMedico obj, int rolId, out string mensaje)
        {
            if (obj.CitaId == 0) { mensaje = "Debe seleccionar una cita."; return false; }
            if (string.IsNullOrEmpty(obj.Diagnostico)) { mensaje = "El diagnóstico es obligatorio."; return false; }

            return _objCapaDatos.Update(obj, rolId, out mensaje);
        }

        public bool Eliminar(int id, int rolId, out string mensaje) => _objCapaDatos.Delete(id, rolId, out mensaje);
    }
}
