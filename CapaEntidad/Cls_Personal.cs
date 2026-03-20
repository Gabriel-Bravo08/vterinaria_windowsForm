using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cls_Personal
    {
        private int _personalId;
        private string _primerNombre;
        private string _segundoNombre;
        private string _primerApellido;
        private string _segundoApellido;
        private string _telefono;
        private string _email;
        private string _nombreUsuario;
        private string _clave;
        private int _rolId;
        private int _estadoId;
        private DateTime _fechaCreacion;

        public int PersonalId
        {
            get { return _personalId; }
            set { _personalId = value; }
        }

        public string PrimerNombre
        {
            get { return _primerNombre; }
            set { _primerNombre = value; }
        }

        public string SegundoNombre
        {
            get { return _segundoNombre; }
            set { _segundoNombre = value; }
        }

        public string PrimerApellido
        {
            get { return _primerApellido; }
            set { _primerApellido = value; }
        }

        public string SegundoApellido
        {
            get { return _segundoApellido; }
            set { _segundoApellido = value; }
        }

        public string Telefono
        {
            get { return _telefono; }
            set { _telefono = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string NombreUsuario
        {
            get { return _nombreUsuario; }
            set { _nombreUsuario = value; }
        }

        public string Clave
        {
            get { return _clave; }
            set { _clave = value; }
        }

        public int RolId
        {
            get { return _rolId; }
            set { _rolId = value; }
        }

        public int EstadoId
        {
            get { return _estadoId; }
            set { _estadoId = value; }
        }

        public string NombreRol { get; set; }
        public string NombreEstado { get; set; }

        public DateTime FechaCreacion
        {
            get { return _fechaCreacion; }
            set { _fechaCreacion = value; }
        }
    }
}
