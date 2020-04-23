using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using U2AlumnosApp.Models;

namespace U2AlumnosApp.ViewModels
{
    public class AvisosViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        void Actualizar([CallerMemberName]string nombre = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }


        private bool lleno;
        public bool Lleno
        {
            get { return lleno; }
            set { lleno = value; Actualizar(); }
        }

        private bool vacio;
        public bool Vacio
        {
            get { return vacio; }
            set { vacio = value; Actualizar(); }
        }

        public ObservableCollection<Alumno> Alumnos { get; set; }
        List<Alumno> AlumnosApp;

        public AvisosViewModel()
        {
            Alumnos = new ObservableCollection<Alumno>();
            AlumnosApp = App.AvisosPrim.GetAlumnosIniciados();
            if (AlumnosApp == null)
            {
                Vacio = false;
            }
            else
            {
                AlumnosApp.ForEach(x => Alumnos.Add(x));
                Lleno = true;
            }
        }
    }
}
