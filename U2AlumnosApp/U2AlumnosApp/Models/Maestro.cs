using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace U2AlumnosApp.Models
{
    public class Maestro
    {
        [PrimaryKey]
       
        public int IdMaestro { get; set; }
        public int Clave { get; set; }
        public string Nombre { get; set; }
    }
}
