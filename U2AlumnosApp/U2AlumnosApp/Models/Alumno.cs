using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace U2AlumnosApp.Models
{
   public class Alumno
    {


		
		private int idAlumno;
		[PrimaryKey]
		public int IdAlumno
		{
			get { return idAlumno; } 
			set { idAlumno = value; }
		}

		private string nombreEscuela;

		public string NombreEscuela
		{
			get { return nombreEscuela; }
			set { nombreEscuela = value; }
		}
		private string clave;

		public string Clave
		{
			get { return clave; }
			set { clave = value; }
		}

		private string nombre;

		public string Nombre
		{
			get { return nombre; }
			set { nombre = value; }
		}

		public bool Activo { get; set; }

	}
}
