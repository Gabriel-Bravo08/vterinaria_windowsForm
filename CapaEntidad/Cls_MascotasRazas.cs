using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cls_MascotasRazas
    {
        private int _mascotaId;
        private int _razaId;

        public int MascotaId
        {
            get { return _mascotaId; }
            set { _mascotaId = value; }
        }

        public int RazaId
        {
            get { return _razaId; }
            set { _razaId = value; }
        }
    }
}
