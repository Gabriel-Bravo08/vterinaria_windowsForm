using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cls_HistorialMedico
    {
        private int _historialMedicoId;
        private int _citaId;
        private string _diagnostico;
        private string _tratamiento;
        private string _observaciones;
        private DateTime? _proximaVisita;
        private int _estadoId;
        private DateTime _fechaCreacion;

        public int HistorialMedicoId
        {
            get { return _historialMedicoId; }
            set { _historialMedicoId = value; }
        }

        public int CitaId
        {
            get { return _citaId; }
            set { _citaId = value; }
        }

        public string Diagnostico
        {
            get { return _diagnostico; }
            set { _diagnostico = value; }
        }

        public string Tratamiento
        {
            get { return _tratamiento; }
            set { _tratamiento = value; }
        }

        public string Observaciones
        {
            get { return _observaciones; }
            set { _observaciones = value; }
        }

        public DateTime? ProximaVisita
        {
            get { return _proximaVisita; }
            set { _proximaVisita = value; }
        }

        public int EstadoId
        {
            get { return _estadoId; }
            set { _estadoId = value; }
        }

        public DateTime FechaCreacion
        {
            get { return _fechaCreacion; }
            set { _fechaCreacion = value; }
        }

        public string NombreMascota { get; set; }
        public string NombreVeterinario { get; set; }
        public string NombreEstado { get; set; }
    }
}
