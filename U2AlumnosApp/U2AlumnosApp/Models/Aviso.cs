using System;
using System.Collections.Generic;
using System.Text;

namespace U2AlumnosApp.Models
{
    public class Aviso
    {
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaEnviar { get; set; }
        public DateTime FechaCaducidad { get; set; }
        public int IdMaestro { get; set; }
    }
}
