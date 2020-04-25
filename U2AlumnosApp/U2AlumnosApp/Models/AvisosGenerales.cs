using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace U2AlumnosApp.Models
{
    public class AvisosGenerales
    {
        [PrimaryKey]
        public int IdAvisosGenerales { get; set; }

        public string Titulo { get; set; }

        public string Contenido { get; set; }

    
        public DateTimeOffset? FechaEnviado { get; set; }

   
        public DateTimeOffset? FechaCaducidad { get; set; }

        public string NombreEscuela { get; set; }
    }
}
