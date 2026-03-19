using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaLogicaNegocio
{
    public class CN_Menu
    {
        private readonly CD_Menu _capaDatos = new CD_Menu();

        public List<Cls_Menus> ObtenerMenusPorRol(int rolId)
        {
            return _capaDatos.ObtenerMenusPorRol(rolId);
        }
    }
}
