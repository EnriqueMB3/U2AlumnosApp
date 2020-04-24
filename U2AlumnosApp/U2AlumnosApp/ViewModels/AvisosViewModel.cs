using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using U2AlumnosApp.Models;
using Xamarin.Forms;

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

        public ObservableCollection<Aviso> Avisos { get; set; }
        public Command AvisoAlumnoCommand { get; private set; }

        List<Aviso> AvisosEnviados;

        public AvisosViewModel(string clave)
        {
            
            Avisos = new ObservableCollection<Aviso>();
            AvisosEnviados = App.AvisosPrim.GetAvisosEnviados(clave);
            if (AvisosEnviados.Count ==0)
            {
                Vacio = true;
            }
            else
            {
                foreach (var item in AvisosEnviados)
                {
                    Avisos.Add(item);
                }
                Lleno = true;
            }

            AvisoAlumnoCommand = new Command(AvisoAlumno);
        }

        private void AvisoAlumno()
        {
            AvisoAlumnoPage avisoAlumnoPage = new AvisoAlumnoPage();
            App.Current.MainPage.Navigation.PushAsync(avisoAlumnoPage);
        }
    }
}
