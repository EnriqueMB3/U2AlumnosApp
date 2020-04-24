using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using U2AlumnosApp.Models;

namespace U2AlumnosApp.ViewModels
{
   public class AvisoAlumnoViewModel:INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
    void Actualizar([CallerMemberName]string nombre = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }

    private Aviso aviso;
        public Aviso Aviso
        {
            get { return aviso; }
            set { aviso = value; Actualizar(); }
        }
    }
}
