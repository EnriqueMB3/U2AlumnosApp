using System;
using System.Collections.Generic;
using System.Text;

namespace U2AlumnosApp
{
    public interface INotificacion
    {
        void Notificar(string titulo, string contenido, string nombreMaestro, int id);
    }
}
