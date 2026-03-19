using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cls_CitasServicios
    {
        private int _citaId;
        private int _servicioId;

        public int CitaId
        {
            get { return _citaId; }
            set { _citaId = value; }
        }

        public int ServicioId
        {
            get { return _servicioId; }
            set { _servicioId = value; }
        }
    }
}
