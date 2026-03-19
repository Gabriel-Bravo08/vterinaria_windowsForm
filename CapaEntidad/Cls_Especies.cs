using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cls_Especies
    {
        private int _especieId;
        private string _nombreEspecie;
        private int _estadoId;

        public int EspecieId
        {
            get { return _especieId; }
            set { _especieId = value; }
        }

        public string NombreEspecie
        {
            get { return _nombreEspecie; }
            set { _nombreEspecie = value; }
        }

        public int EstadoId
        {
            get { return _estadoId; }
            set { _estadoId = value; }
        }
    }
}
