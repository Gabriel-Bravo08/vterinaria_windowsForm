using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cls_Servicios
    {
        private int _servicioId;
        private string _nombreServicio;
        private string _descripcion;
        private decimal _precio;
        private int _estadoId;

        public int ServicioId
        {
            get { return _servicioId; }
            set { _servicioId = value; }
        }

        public string NombreServicio
        {
            get { return _nombreServicio; }
            set { _nombreServicio = value; }
        }

        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        public decimal Precio
        {
            get { return _precio; }
            set { _precio = value; }
        }

        public int EstadoId
        {
            get { return _estadoId; }
            set { _estadoId = value; }
        }

        public string NombreEstado { get; set; }
    }
}
