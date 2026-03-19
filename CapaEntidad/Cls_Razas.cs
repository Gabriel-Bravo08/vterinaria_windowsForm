using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cls_Razas
    {
        private int _razaId;
        private int _especieId;
        private string _nombreRaza;
        private int _estadoId;

        public int RazaId
        {
            get { return _razaId; }
            set { _razaId = value; }
        }

        public int EspecieId
        {
            get { return _especieId; }
            set { _especieId = value; }
        }

        public string NombreRaza
        {
            get { return _nombreRaza; }
            set { _nombreRaza = value; }
        }

        public int EstadoId
        {
            get { return _estadoId; }
            set { _estadoId = value; }
        }
    }
}
