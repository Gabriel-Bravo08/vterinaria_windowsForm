using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cls_Permisos
    {
        private int _permisoId;
        private int _rolId;
        private int _menuId;
        private int _estadoId;

        public int PermisoId
        {
            get { return _permisoId; }
            set { _permisoId = value; }
        }

        public int RolId
        {
            get { return _rolId; }
            set { _rolId = value; }
        }

        public int MenuId
        {
            get { return _menuId; }
            set { _menuId = value; }
        }

        public int EstadoId
        {
            get { return _estadoId; }
            set { _estadoId = value; }
        }
    }
}
