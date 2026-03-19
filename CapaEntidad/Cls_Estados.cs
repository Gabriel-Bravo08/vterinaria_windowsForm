using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cls_Estados
    {
        private int _estadoId;
        private string _nombreEstado;

        public int EstadoId
        {
            get { return _estadoId; }
            set { _estadoId = value; }
        }

        public string NombreEstado
        {
            get { return _nombreEstado; }
            set { _nombreEstado = value; }
        }
    }
}
