using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cls_Menus
    {
        private int _menuId;
        private string _nombre;
        private string _descripcion;
        private int _estadoId;
        private DateTime _fechaCreacion;

        public int MenuId
        {
            get { return _menuId; }
            set { _menuId = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        public int EstadoId
        {
            get { return _estadoId; }
            set { _estadoId = value; }
        }

        public DateTime FechaCreacion
        {
            get { return _fechaCreacion; }
            set { _fechaCreacion = value; }
        }
    }
}
