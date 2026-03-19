using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cls_Citas
    {
        private int _citaId;
        private int _mascotaId;
        private int _veterinarioId;
        private int? _creadoPor;
        private DateTime _fechaCita;
        private int _estadoId;
        private string _notas;

        public int CitaId
        {
            get { return _citaId; }
            set { _citaId = value; }
        }

        public int MascotaId
        {
            get { return _mascotaId; }
            set { _mascotaId = value; }
        }

        public int VeterinarioId
        {
            get { return _veterinarioId; }
            set { _veterinarioId = value; }
        }

        public int? CreadoPor
        {
            get { return _creadoPor; }
            set { _creadoPor = value; }
        }

        public DateTime FechaCita
        {
            get { return _fechaCita; }
            set { _fechaCita = value; }
        }

        public int EstadoId
        {
            get { return _estadoId; }
            set { _estadoId = value; }
        }

        public string Notas
        {
            get { return _notas; }
            set { _notas = value; }
        }
    }
}
