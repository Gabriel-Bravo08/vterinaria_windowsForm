using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogicaNegocio
{
    public class CN_Catalogos
    {
        private CD_Catalogos _objCapaDatos = new CD_Catalogos();

        public List<Cls_Especies> ListarEspecies() => _objCapaDatos.ListarEspecies();
        public List<Cls_Razas> ListarRazas(int especieId = 0) => _objCapaDatos.ListarRazas(especieId);
        public List<Cls_Especialidades> ListarEspecialidades() => _objCapaDatos.ListarEspecialidades();
        public List<Cls_Roles> ListarRoles() => _objCapaDatos.ListarRoles();
        public List<Cls_Personal> ListarVeterinarios() => _objCapaDatos.ListarVeterinarios();
    }
}
