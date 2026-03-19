using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cls_Roles
    {
        private int _rolId;
        private string _nombreRol;
        private int _estadoId;

        public int RolId
        {
            get { return _rolId; }
            set { _rolId = value; }
        }

        public string NombreRol
        {
            get { return _nombreRol; }
            set { _nombreRol = value; }
        }

        public int EstadoId
        {
            get { return _estadoId; }
            set { _estadoId = value; }
        }
    }
}
