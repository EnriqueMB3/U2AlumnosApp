using System;
using System.Collections.Generic;
using System.Text;

namespace U2AlumnosApp.Models
{
    public class Aviso
    {
        public int IdAvisosEnviados { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public int Estatus { get; set; }
        public int ClaveMaestro { get; set; }
        public string NombreMaestro { get; set; }

        public string ClaveAlumno { get; set; }
        public DateTime FechaEnviar { get; set; }
        public DateTime? FechaRecibido { get; set; }
        public DateTime? FechaLeido { get; set; }

        
    }
}
