using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cls_Especialidades
    {
        private int _especialidadId;
        private string _nombreEspecialidad;
        private int _estadoId;

        public int EspecialidadId
        {
            get { return _especialidadId; }
            set { _especialidadId = value; }
        }

        public string NombreEspecialidad
        {
            get { return _nombreEspecialidad; }
            set { _nombreEspecialidad = value; }
        }

        public int EstadoId
        {
            get { return _estadoId; }
            set { _estadoId = value; }
        }
    }
}
