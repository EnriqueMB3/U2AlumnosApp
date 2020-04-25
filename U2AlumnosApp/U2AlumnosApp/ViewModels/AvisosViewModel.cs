using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using U2AlumnosApp.Models;
using U2AlumnosApp.Views;
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

        private int avisosGeneralesCount;
        public int AvisosGeneralesCount
        {
            get { return avisosGeneralesCount; }
            set { avisosGeneralesCount = value; Actualizar(); }
        }

        public Command AvisoAlumnoCommand { get; private set; }
        public Command AvisosGeneralesCommand { get; private set; }

        public ObservableCollection<Aviso> Avisos { get; set; }
        List<Aviso> AvisosEnviados;

        public AvisosViewModel(AlumnoIniciado alumno)
        {
            Avisos = new ObservableCollection<Aviso>();
            AvisosEnviados = App.AvisosPrim.GetAvisosEnviados(alumno.ClaveAlumnoIniciado);
            AvisosGeneralesCount = App.AvisosPrim.CountGenerales(alumno.NombreEscuela); 
            if (AvisosEnviados.Count == 0)
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
            AvisosGeneralesCommand = new Command(AvisosGenerales);
        }

        private async void AvisosGenerales()
        {
            await Task.Run(() => App.AvisosPrim.AvisosGeneralesUpdate());
            
            await App.Current.MainPage.Navigation.PushAsync(new AvisosGeneralesPage());
        }

        private void AvisoAlumno()
        {
            AvisoAlumnoPage avisoAlumnoPage = new AvisoAlumnoPage();

            App.Current.MainPage.Navigation.PushAsync(avisoAlumnoPage);
        }
    }
}
