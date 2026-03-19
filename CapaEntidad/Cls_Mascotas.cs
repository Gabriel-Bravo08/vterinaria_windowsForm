using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cls_Mascotas
    {
        private int _mascotaId;
        private int _clienteId;
        private int _especieId;
        private string _nombreMascota;
        private DateTime? _fechaNacimiento;
        private decimal? _peso;
        private string _color;
        private int _estadoId;

        public int MascotaId
        {
            get { return _mascotaId; }
            set { _mascotaId = value; }
        }

        public int ClienteId
        {
            get { return _clienteId; }
            set { _clienteId = value; }
        }

        public int EspecieId
        {
            get { return _especieId; }
            set { _especieId = value; }
        }

        public string NombreMascota
        {
            get { return _nombreMascota; }
            set { _nombreMascota = value; }
        }

        public DateTime? FechaNacimiento
        {
            get { return _fechaNacimiento; }
            set { _fechaNacimiento = value; }
        }

        public decimal? Peso
        {
            get { return _peso; }
            set { _peso = value; }
        }

        public string Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public int EstadoId
        {
            get { return _estadoId; }
            set { _estadoId = value; }
        }
    }
}
