using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cls_PersonalEspecialidades
    {
        private int _personalId;
        private int _especialidadId;

        public int PersonalId
        {
            get { return _personalId; }
            set { _personalId = value; }
        }

        public int EspecialidadId
        {
            get { return _especialidadId; }
            set { _especialidadId = value; }
        }
    }
}
